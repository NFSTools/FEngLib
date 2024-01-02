using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib.Packages;
using FEngLib.Structures;
using FEngRender.Data;
using FEngRender.Utils;
using SharpGL;
using SharpGL.Enumerations;
using SixLabors.Fonts;

namespace FEngRender.GL;

/// <summary>
/// Make sure to initialize an OpenGL context before using this.
/// </summary>
public class GLRenderTreeRenderer
{
    private readonly OpenGL _gl;
    private readonly Dictionary<string, Texture> _loadedTextures = new();
    private readonly Dictionary<RenderTreeNode, InternalRenderNode> _renderTreeToInternalNode = new();
    private readonly Dictionary<uint, Texture> _resourceRequestToTexture = new();

    private long _lastRenderTime;
    private NodeSorter _nodeSorter;

    private InternalRenderNode _selectedRenderNode;
    private Stopwatch _stopwatch;
    private List<RenderTreeNode> _treeRootNodes;

    public GLRenderTreeRenderer(OpenGL gl)
    {
        _gl = gl;
    }

    public RenderTreeNode SelectedNode { get; private set; }

    public void SelectNode(RenderTreeNode node)
    {
        _selectedRenderNode = null;
        SelectedNode = null;

        var queue = new Queue<InternalRenderNode>();
        foreach (var internalNode in _renderTreeToInternalNode.Values) queue.Enqueue(internalNode);

        while (queue.TryDequeue(out var internalNode))
        {
            if (internalNode.GetTreeNode() == node)
            {
                _selectedRenderNode = internalNode;
                SelectedNode = node;
                break;
            }

            if (internalNode is GroupInternalRenderNode grp)
                foreach (var child in grp)
                    queue.Enqueue(child);
        }
    }

    public void PrepareRender()
    {
        _gl.ClearColor(0.0f, 0f, 0.0f, 0f);

        _gl.Enable(OpenGL.GL_TEXTURE_2D);
    }

    public void LoadTextures(string directory)
    {
        _loadedTextures.Clear();
        foreach (var pngFile in Directory.GetFiles(directory, "*.png"))
        {
            var filename = Path.GetFileNameWithoutExtension(pngFile) ?? "";
            _loadedTextures.Add(filename.ToUpperInvariant(), new Texture(_gl, pngFile));
        }
    }

    public void SetTree(RenderTree tree)
    {
        _treeRootNodes = tree.ToList();
        _nodeSorter = new NodeSorter();
        _renderTreeToInternalNode.Clear();

        foreach (var rootNode in _treeRootNodes)
            _renderTreeToInternalNode.Add(rootNode, GenerateInternalRenderNode(rootNode));

        SelectedNode = null;
        _selectedRenderNode = null;
        _stopwatch = Stopwatch.StartNew();
    }

    public void Render()
    {
        _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        _gl.LoadIdentity();

        // disable depth
        _gl.DepthMask(0);

        var dt = (int)(_stopwatch.ElapsedMilliseconds - _lastRenderTime);

        DoNodeRender(dt);

        // todo: fix bounding boxes
        // _selectedRenderNode?.GetBoundingQuad().DrawBoundingBox(_gl);

        _gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
        _gl.Disable(OpenGL.GL_TEXTURE_2D);
        _gl.DrawText(10, 10, 1, 0, 0, "Consolas", 16, $"dt: {dt}ms");

        _gl.MatrixMode(MatrixMode.Projection);
        _gl.Ortho(-320, 320, -240, 240, -1, 1);
        _gl.Flush();
        _lastRenderTime = _stopwatch.ElapsedMilliseconds;
    }

    public void SetBackgroundColor(Color4 color)
    {
        Vector4 colorV = color;
        _gl.ClearColor(colorV.X, colorV.Y, colorV.Z, colorV.W);
    }

    private void DoNodeRender(int dt)
    {
        var renderNodes = new List<InternalRenderNode>(512);

        // Step 1: Reset object sorter
        _nodeSorter.Reset();

        // Step 2: Update nodes
        PrepareNodes(Matrix4x4.Identity, dt, renderNodes);

        // Step 3: Build render list
        foreach (var renderNode in renderNodes) renderNode.Init(_nodeSorter);

        // Step 3.5: Fix node order
        _nodeSorter.Finish();

        // Step 4: Render
        // Debug.WriteLine("Rendering nodes...");

        foreach (var renderNode in _nodeSorter)
            // Debug.WriteLine("Rendering 0x{0:X}", renderNode.GetTreeNode().GetObject().Guid);
            renderNode.Render(_gl);
    }

    private void PrepareNodes(Matrix4x4 viewMatrix, int timeDelta, ICollection<InternalRenderNode> renderNodeList)
    {
        Debug.WriteLine("Preparing nodes");
        var nodeUpdateQueue =
            new Queue<(IEnumerable<RenderTreeNode> nodes, RenderTreeNode parent, Matrix4x4 transform)>();

        nodeUpdateQueue.Enqueue((_treeRootNodes, null, viewMatrix));

        while (nodeUpdateQueue.TryDequeue(out var nodeListEntry))
        {
            var renderContext = new RenderContext(nodeListEntry.transform, nodeListEntry.parent);
            foreach (var node in nodeListEntry.nodes)
            {
                // Debug.WriteLine("Updating node: {0:X}", node.GetObject().Guid);
                // if (node.GetObject().Data == null) continue;
                node.Update(renderContext, timeDelta);

                if (node is RenderTreeGroup groupNode)
                    nodeUpdateQueue.Enqueue((groupNode, groupNode, groupNode.Transform));
            }
        }

        foreach (var treeRootNode in _treeRootNodes) renderNodeList.Add(_renderTreeToInternalNode[treeRootNode]);
    }

    private InternalRenderNode GenerateInternalRenderNode(RenderTreeNode node)
    {
        var texture = GetTexture(node.GetObject().ResourceRequest);
        return node switch
        {
            RenderTreeColoredImage ci => new ColoredImageInternalRenderNode(ci, texture),
            RenderTreeImage ri => new ImageInternalRenderNode(ri, texture),
            RenderTreeSimpleImage si => new SimpleImageInternalRenderNode(si),
            RenderTreeMultiImage mi => new MultiImageInternalRenderNode(mi, texture),
            RenderTreeText t => new TextInternalRenderNode(t),
            RenderTreeMovie m => new MovieInternalRenderNode(m),
            RenderTreeGroup g => new GroupInternalRenderNode(g, g.Select(GenerateInternalRenderNode).ToList()),
            _ => throw new NotImplementedException(
                $"Cannot convert render tree node of type [{node.GetType()}] to OpenGL render node")
        };
    }

    // private void RenderGroupBB(RenderTreeNode node, bool doBoundingBox = false)
    // {
    //     var otkMat4 = node.Transform;
    //
    //     var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
    //         1.0f,
    //         otkMat4,
    //         Vector2.Zero,
    //         Vector2.One,
    //         new Vector4[4]);
    //
    //     if (doBoundingBox)
    //         q.DrawBoundingBox(_gl);
    // }

    private Texture GetTexture(ResourceRequest resource)
    {
        if (resource is not { Type: ResourceType.Image }) return null;

        if (_resourceRequestToTexture.TryGetValue(resource.ID, out var texture)) return texture;

        var key = CleanResourcePath(resource.Name);
        if (!_loadedTextures.TryGetValue(key, out texture))
            Debug.WriteLine("Texture not found: {0}", new object[] { key });
        else
            _resourceRequestToTexture[resource.ID] = texture;

        //Debug.WriteLine("Texture not found: {0}", new object[] { key });
        return texture;
    }

    private static string CleanResourcePath(string path)
    {
        return path.Split('\\')[^1].Split('.')[0].ToUpperInvariant();
    }

    private abstract class InternalRenderNode
    {
        public abstract RenderTreeNode GetTreeNode();
        public abstract void Init(NodeSorter nodeSorter);
        public abstract void Render(OpenGL gl);
        public abstract Quad GetBoundingQuad();
    }

    private abstract class InternalRenderNode<TNode> : InternalRenderNode where TNode : RenderTreeNode
    {
        protected InternalRenderNode(TNode treeNode)
        {
            TreeNode = treeNode;
        }

        protected TNode TreeNode { get; }

        public override void Init(NodeSorter nodeSorter)
        {
            nodeSorter.Add(this, TreeNode.GetZ());
        }

        public override RenderTreeNode GetTreeNode()
        {
            return TreeNode;
        }
    }

    private class GroupInternalRenderNode : InternalRenderNode, IEnumerable<InternalRenderNode>
    {
        private readonly List<InternalRenderNode> _children;
        private readonly RenderTreeGroup _renderTreeGroup;

        public GroupInternalRenderNode(RenderTreeGroup renderTreeGroup, List<InternalRenderNode> children)
        {
            _renderTreeGroup = renderTreeGroup;
            _children = children;
        }

        public IEnumerator<InternalRenderNode> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override RenderTreeNode GetTreeNode()
        {
            return _renderTreeGroup;
        }

        public override void Init(NodeSorter nodeSorter)
        {
            foreach (var child in _children) child.Init(nodeSorter);
        }

        public override void Render(OpenGL gl)
        {
            throw new NotImplementedException("Groups should not be rendered directly!");
        }

        public override Quad GetBoundingQuad()
        {
            return Quad.MaxBox(_children.Select(c => c.GetBoundingQuad())) ?? new Quad(new VertexDeclaration[4]);
        }
    }

    private class ColoredImageInternalRenderNode : InternalRenderNode<RenderTreeColoredImage>
    {
        private readonly Texture _texture;
        private Quad _quad;

        public ColoredImageInternalRenderNode(RenderTreeColoredImage treeNode, Texture texture) : base(treeNode)
        {
            _texture = texture;
        }

        public override void Render(OpenGL gl)
        {
            var otkMat4 = TreeNode.Transform;

            // top left, top right, bottom right, bottom left
            var colors = new Vector4[4];

            colors[0] = TreeNode.TopLeft;
            colors[1] = TreeNode.TopRight;
            colors[2] = TreeNode.BottomRight;
            colors[3] = TreeNode.BottomLeft;

            _quad = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                TreeNode.UpperLeft,
                TreeNode.LowerRight,
                colors);

            _quad.Render(gl, _texture);
        }

        public override Quad GetBoundingQuad()
        {
            return _quad;
        }
    }

    private class ImageInternalRenderNode : InternalRenderNode<RenderTreeImage>
    {
        private readonly Texture _texture;
        private Quad _quad;

        public ImageInternalRenderNode(RenderTreeImage treeNode, Texture texture) : base(treeNode)
        {
            _texture = texture;
        }

        public override void Render(OpenGL gl)
        {
            var otkMat4 = TreeNode.Transform;

            // top left, top right, bottom right, bottom left
            var colors = new Vector4[4];

            colors[0] = TreeNode.BlendedColor;
            colors[1] = TreeNode.BlendedColor;
            colors[2] = TreeNode.BlendedColor;
            colors[3] = TreeNode.BlendedColor;

            _quad = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                TreeNode.UpperLeft,
                TreeNode.LowerRight,
                colors);
            _quad.Render(gl, _texture);
        }

        public override Quad GetBoundingQuad()
        {
            return _quad;
        }
    }

    private class SimpleImageInternalRenderNode : InternalRenderNode<RenderTreeSimpleImage>
    {
        private Quad _quad;

        public SimpleImageInternalRenderNode(RenderTreeSimpleImage treeNode) : base(treeNode)
        {
        }

        public override void Render(OpenGL gl)
        {
            // top left, top right, bottom right, bottom left
            var colors = new Vector4[4];
            colors[0] = colors[1] = colors[2] = colors[3] = TreeNode.BlendedColor;

            var otkMat4 = TreeNode.Transform;

            _quad = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                Vector2.Zero,
                Vector2.Zero,
                colors);
            _quad.Render(gl);
        }

        public override Quad GetBoundingQuad()
        {
            return _quad;
        }
    }

    private class TextInternalRenderNode : InternalRenderNode<RenderTreeText>
    {
        private GLGlyphRenderer _glGlyphRenderer;

        public TextInternalRenderNode(RenderTreeText treeNode) : base(treeNode)
        {
        }

        public override void Render(OpenGL gl)
        {
            _glGlyphRenderer = new GLGlyphRenderer(gl);
            var font = TextHelpers.GetFont(18);
            var str = TreeNode.FrontendObject;
            var (_, _, width, height) = TextHelpers.MeasureText(str.Value, new RendererOptions(font)
            {
                WrappingWidth = str.MaxWidth
            });
            var xOffset = TextHelpers.CalculateXOffset((uint)str.Formatting,
                width);
            var yOffset = TextHelpers.CalculateYOffset((uint)str.Formatting,
                height);

            _glGlyphRenderer.Transform = TreeNode.Transform;
            _glGlyphRenderer.Color = TreeNode.BlendedColor;
            _glGlyphRenderer.Formatting = str.Formatting;

            TextRenderer.RenderTextTo(_glGlyphRenderer, str.Value,
                new RendererOptions(font, new Vector2(xOffset, yOffset))
                {
                    WrappingWidth = str.MaxWidth
                }
            );
        }

        public override Quad GetBoundingQuad()
        {
            if (_glGlyphRenderer == null)
                return new Quad(new VertexDeclaration[4]);
            // top left, top right, bottom right, bottom left
            var vertices = new VertexDeclaration[4];
            vertices[0].Position = new Vector3(_glGlyphRenderer.MinX, _glGlyphRenderer.MinY, 0);
            vertices[1].Position = new Vector3(_glGlyphRenderer.MaxX, _glGlyphRenderer.MinY, 0);
            vertices[2].Position = new Vector3(_glGlyphRenderer.MaxX, _glGlyphRenderer.MaxY, 0);
            vertices[3].Position = new Vector3(_glGlyphRenderer.MinX, _glGlyphRenderer.MaxY, 0);
            return new Quad(vertices);
        }
    }

    private class MultiImageInternalRenderNode : InternalRenderNode<RenderTreeMultiImage>
    {
        private readonly Texture _texture;
        private Quad _quad;

        public MultiImageInternalRenderNode(RenderTreeMultiImage treeNode, Texture texture) : base(treeNode)
        {
            _texture = texture;
        }

        public override void Render(OpenGL gl)
        {
            var otkMat4 = TreeNode.Transform;

            // top left, top right, bottom right, bottom left
            var colors = new Vector4[4];

            colors[0] = TreeNode.BlendedColor;
            colors[1] = TreeNode.BlendedColor;
            colors[2] = TreeNode.BlendedColor;
            colors[3] = TreeNode.BlendedColor;

            _quad = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                TreeNode.UpperLeft,
                TreeNode.LowerRight,
                colors);
            _quad.Render(gl, _texture);
        }

        public override Quad GetBoundingQuad()
        {
            return _quad;
        }
    }

    private class MovieInternalRenderNode : InternalRenderNode<RenderTreeMovie>
    {
        public MovieInternalRenderNode(RenderTreeMovie treeNode) : base(treeNode)
        {
        }

        public override void Render(OpenGL gl)
        {
            //
        }

        public override Quad GetBoundingQuad()
        {
            throw new NotImplementedException();
        }
    }

    private class NodeSorter : IEnumerable<InternalRenderNode>
    {
        // 256 buckets for Z levels 0-255
        // Z < 0 || Z > 255 gets clamped
        private readonly List<InternalRenderNode>[] _zBuckets = new List<InternalRenderNode>[256];

        public NodeSorter()
        {
            Reset();
        }

        public IEnumerator<InternalRenderNode> GetEnumerator()
        {
            return _zBuckets.Reverse().SelectMany(bucket => bucket).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Reset()
        {
            for (var i = 0; i < _zBuckets.Length; i++) _zBuckets[i] = new List<InternalRenderNode>(16);
        }

        public void Add(InternalRenderNode node, float z)
        {
            if (node.GetTreeNode().IsHidden())
                return;

            var zBucketIndex = (int)Math.Max(0, Math.Min(_zBuckets.Length - 1, z));

            _zBuckets[zBucketIndex].Add(node);
        }

        public void Finish()
        {
            foreach (var bucket in _zBuckets)
                if (bucket.Count > 0)
                    bucket.Sort((rn1, rn2) =>
                        rn2.GetTreeNode().GetZ().CompareTo(rn1.GetTreeNode().GetZ()));
        }
    }
}