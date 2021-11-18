using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Structures;
using FEngRender.Data;
using FEngRender.Utils;
using SharpGL;
using SharpGL.Enumerations;
using SixLabors.Fonts;

namespace FEngRender.GL
{
    /// <summary>
    /// Make sure to initialize an OpenGL context before using this.
    /// </summary>
    public class GLRenderTreeRenderer
    {
        private readonly OpenGL _gl;
        private readonly GLGlyphRenderer _glyphRenderer;

        private readonly Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
        private long _lastRenderTime;

        private Stopwatch _stopwatch;

        public GLRenderTreeRenderer(OpenGL gl)
        {
            _gl = gl;
            _glyphRenderer = new GLGlyphRenderer(_gl);
            _stopwatch = Stopwatch.StartNew();
        }

        public RenderTreeNode SelectedNode { get; set; }

        public void PrepareRender()
        {
            _gl.ClearColor(0.0f, 0f, 0.0f, 0f);

            _gl.Enable(OpenGL.GL_TEXTURE_2D);
        }

        public void LoadTextures(string directory)
        {
            _textures.Clear();
            foreach (var pngFile in Directory.GetFiles(directory, "*.png"))
            {
                var filename = Path.GetFileNameWithoutExtension(pngFile) ?? "";
                _textures.Add(filename.ToUpperInvariant(), new Texture(_gl, pngFile));
            }
        }

        /// <summary>
        /// Renders nodes from a <see cref="RenderTree"/> to a surface.
        /// </summary>
        /// <param name="tree">The <see cref="RenderTree"/> to render.</param>
        public void Render(RenderTree tree)
        {
            _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();

            // disable depth
            _gl.DepthMask(0);

            PrepareNodes(tree, Matrix4x4.Identity, (int)(_stopwatch.ElapsedMilliseconds - _lastRenderTime));
            RenderTree(tree);
            // _stopwatch.Restart();

            if (SelectedNode != null)
            {
                RenderNode(SelectedNode, true);
            }

            _gl.MatrixMode(MatrixMode.Projection);
            _gl.Ortho(0, 640, 480, 0, -1, 1);
            _gl.Flush();
            _lastRenderTime = _stopwatch.ElapsedMilliseconds;
        }

        public void SetBackgroundColor(Color4 color)
        {
            Vector4 colorV = color;
            _gl.ClearColor(colorV.X, colorV.Y, colorV.Z, colorV.W);
        }

        private void PrepareNodes(IEnumerable<RenderTreeNode> nodes,
            Matrix4x4 viewMatrix, int timeDelta, RenderTreeNode parent = null)
        {
            var nodeList = nodes.ToList();

            nodeList.ForEach(r => r.PrepareForRender(viewMatrix, parent, timeDelta, true));
            foreach (var renderTreeGroup in nodeList.OfType<RenderTreeGroup>())
            {
                PrepareNodes(renderTreeGroup, renderTreeGroup.ObjectMatrix, timeDelta, renderTreeGroup);
            }
        }

        private void RenderTree(IEnumerable<RenderTreeNode> nodes)
        {
            foreach (var renderTreeNode in Data.RenderTree.GetAllTreeNodesForRendering(nodes)
                .OrderByDescending(n => n.GetZ()))
            {
                RenderNode(renderTreeNode);
            }
        }

        private void RenderNode(RenderTreeNode node, bool doBoundingBox = false)
        {
            switch (node.FrontendObject)
            {
                // there are some things we just don't need to handle
                case Group _:
                    RenderGroupBB(node, doBoundingBox);
                    break;
                case Movie _:
                    break;
                case SimpleImage _:
                    RenderSimpleImage(node, doBoundingBox);
                    break;
                case IImage<ImageData> image:
                    RenderImage(node, image, doBoundingBox);
                    break;
                case Text str:
                    RenderString(node, str);
                    break;
                default:
                    Debug.Assert(false, "Unsupported object", "Type: {0}", node.FrontendObject.GetType());
                    break;
            }
        }

        private void RenderString(RenderTreeNode node, Text str)
        {
            var font = TextHelpers.GetFont(18);
            var (_, _, width, height) = TextHelpers.MeasureText(str.Value, new RendererOptions(font)
            {
                WrappingWidth = str.MaxWidth
            });
            var xOffset = TextHelpers.CalculateXOffset((uint)str.Formatting,
                width);
            var yOffset = TextHelpers.CalculateYOffset((uint)str.Formatting,
                height);

            _glyphRenderer.Transform = node.ObjectMatrix * Matrix4x4.CreateTranslation(320, 240, 0);
            _glyphRenderer.Color = node.ObjectColor;
            _glyphRenderer.Formatting = str.Formatting;

            TextRenderer.RenderTextTo(_glyphRenderer, str.Value,
                new RendererOptions(font, new Vector2(xOffset, yOffset))
                {
                    WrappingWidth = str.MaxWidth,
                }
            );
        }

        private void RenderSimpleImage(RenderTreeNode node, bool doBoundingBox = false)
        {
            // top left, top right, bottom right, bottom left
            Vector4[] colors = new Vector4[4];
            colors[0] = colors[1] = colors[2] = colors[3] = node.ObjectColor;

            var otkMat4 = node.ObjectMatrix * Matrix4x4.CreateTranslation(320, 240, 0);
            var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                Vector2.Zero,
                Vector2.Zero,
                colors);

            if (doBoundingBox)
                q.DrawBoundingBox(_gl);
            else
                q.Render(_gl);
        }

        private void RenderImage(RenderTreeNode node, IObject<ImageData> image, bool doBoundingBox = false)
        {
            var texture = GetTexture(image.ResourceRequest);

            if (texture == null)
                return;

            var otkMat4 = node.ObjectMatrix * Matrix4x4.CreateTranslation(320, 240, 0);

            // this is done by the game, basically a noop in most cases but sometimes relevant?
            // i think it's to do with square-ness and powers of two. or something
            uint CalculateDivisor(int value)
            {
                if (texture.Width == 0) return 0;

                var v10 = (uint)(value - 1) >> 1;
                uint divisor;
                for (divisor = 2; v10 != 0; divisor *= 2)
                {
                    v10 >>= 1;
                }

                return divisor;
            }

            var widthDivide = (float)CalculateDivisor(texture.Width);
            var heightDivide = (float)CalculateDivisor(texture.Height);

            var texUpLeft = new Vector2(
                texture.Width / widthDivide * node.UpperLeft.X,
                texture.Height / heightDivide * node.UpperLeft.Y
            );

            var texLowRight = new Vector2(
                texture.Width / widthDivide * node.LowerRight.X,
                texture.Height / heightDivide * node.LowerRight.Y
            );

            // top left, top right, bottom right, bottom left
            Vector4[] colors = new Vector4[4];

            if (image is ColoredImage ci)
            {
                colors[0] = ci.Data.TopLeft;
                colors[1] = ci.Data.TopRight;
                colors[2] = ci.Data.BottomRight;
                colors[3] = ci.Data.BottomLeft;
            }
            else
            {
                colors[0] = colors[1] = colors[2] = colors[3] = node.ObjectColor;
            }

            var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                texUpLeft,
                texLowRight,
                colors);

            if (doBoundingBox)
                q.DrawBoundingBox(_gl);
            else
                q.Render(_gl, texture);
        }

        private void RenderGroupBB(RenderTreeNode node, bool doBoundingBox = false)
        {
            var otkMat4 = node.ObjectMatrix * Matrix4x4.CreateTranslation(320, 240, 0);

            var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                Vector2.Zero,
                Vector2.One,
                new Vector4[4]);

            if (doBoundingBox)
                q.DrawBoundingBox(_gl);
        }

        private Texture GetTexture(ResourceRequest resource)
        {
            if (resource.Type != ResourceType.Image)
            {
                return null;
            }

            var key = CleanResourcePath(resource.Name);
            if (_textures.TryGetValue(key, out var tex))
                return tex;
            //Debug.WriteLine("Texture not found: {0}", new object[] { key });
            return null;
        }

        private static string CleanResourcePath(string path)
        {
            return path.Split('\\')[^1].Split('.')[0].ToUpperInvariant();
        }
    }
}