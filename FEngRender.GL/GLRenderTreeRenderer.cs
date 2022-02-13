using System;
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

namespace FEngRender.GL;

/// <summary>
/// Make sure to initialize an OpenGL context before using this.
/// </summary>
public class GLRenderTreeRenderer
{
    private readonly OpenGL _gl;
    private readonly GLGlyphRenderer _glyphRenderer;

    private readonly Dictionary<string, Texture> _loadedTextures = new Dictionary<string, Texture>();
    private readonly Dictionary<uint, Texture> _resourceRequestToTexture = new Dictionary<uint, Texture>();
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
        _loadedTextures.Clear();
        foreach (var pngFile in Directory.GetFiles(directory, "*.png"))
        {
            var filename = Path.GetFileNameWithoutExtension(pngFile) ?? "";
            _loadedTextures.Add(filename.ToUpperInvariant(), new Texture(_gl, pngFile));
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

        var dt = (int)(_stopwatch.ElapsedMilliseconds - _lastRenderTime);
        _gl.DrawText(10, 10, 1, 0, 0, "Consolas", 16, $"dt: {dt}ms");
        PrepareNodes(tree, Matrix4x4.Identity, dt);
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
        var renderContext = new RenderContext(viewMatrix, parent);
        var nodeList = nodes.ToList();

        foreach (var node in nodeList)
        {
            node.Update(renderContext, timeDelta);
        }

        foreach (var renderTreeGroup in nodeList.OfType<RenderTreeGroup>())
        {
            PrepareNodes(renderTreeGroup, renderTreeGroup.Transform, timeDelta, renderTreeGroup);
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
        switch (node)
        {
            case RenderTreeGroup:
                // TODO: render group bounding box
                break;
            case RenderTreeMovie:
                break;
            case RenderTreeSimpleImage si:
                RenderSimpleImage(si, doBoundingBox);
                break;
            case RenderTreeColoredImage ci:
                RenderColoredImage(ci, doBoundingBox);
                break;
            case RenderTreeImage img:
                RenderRegularImage(img, doBoundingBox);
                break;
            case RenderTreeMultiImage mi:
                RenderMultiImage(mi, doBoundingBox);
                break;
            case RenderTreeText text:
                RenderString(text);
                break;
            default:
                Debug.Assert(false, "Unsupported node", "Type: {0}", node.GetType());
                break;
        }
        //switch (node.FrontendObject)
        //{
        //    // there are some things we just don't need to handle
        //    case Group _:
        //        RenderGroupBB(node, doBoundingBox);
        //        break;
        //    case Movie _:
        //        break;
        //    case SimpleImage _:
        //        RenderSimpleImage(node, doBoundingBox);
        //        break;
        //    case IImage<ImageData> image:
        //        RenderImage(node, image, doBoundingBox);
        //        break;
        //    case Text str:
        //        RenderString(node, str);
        //        break;
        //    default:
        //        Debug.Assert(false, "Unsupported object", "Type: {0}", node.FrontendObject.GetType());
        //        break;
        //}
    }

    private void RenderString(RenderTreeText node)
    {
        var font = TextHelpers.GetFont(18);
        var str = node.FrontendObject;
        var (_, _, width, height) = TextHelpers.MeasureText(str.Value, new RendererOptions(font)
        {
            WrappingWidth = str.MaxWidth
        });
        var xOffset = TextHelpers.CalculateXOffset((uint)str.Formatting,
            width);
        var yOffset = TextHelpers.CalculateYOffset((uint)str.Formatting,
            height);

        _glyphRenderer.Transform = node.Transform * Matrix4x4.CreateTranslation(320, 240, 0);
        _glyphRenderer.Color = node.BlendedColor;
        _glyphRenderer.Formatting = str.Formatting;

        TextRenderer.RenderTextTo(_glyphRenderer, str.Value,
            new RendererOptions(font, new Vector2(xOffset, yOffset))
            {
                WrappingWidth = str.MaxWidth,
            }
        );
    }

    private void RenderSimpleImage(RenderTreeSimpleImage node, bool doBoundingBox = false)
    {
        // top left, top right, bottom right, bottom left
        Vector4[] colors = new Vector4[4];
        colors[0] = colors[1] = colors[2] = colors[3] = node.BlendedColor;

        var otkMat4 = node.Transform * Matrix4x4.CreateTranslation(320, 240, 0);
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

    private void RenderColoredImage(RenderTreeColoredImage coloredImage, bool doBoundingBox = false)
    {
        var texture = GetTexture(coloredImage.FrontendObject.ResourceRequest);

        if (texture == null)
            return;

        var otkMat4 = coloredImage.Transform * Matrix4x4.CreateTranslation(320, 240, 0);

        // top left, top right, bottom right, bottom left
        Vector4[] colors = new Vector4[4];

        colors[0] = coloredImage.TopLeft;
        colors[1] = coloredImage.TopRight;
        colors[2] = coloredImage.BottomRight;
        colors[3] = coloredImage.BottomLeft;

        var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
            1.0f,
            otkMat4,
            coloredImage.UpperLeft,
            coloredImage.LowerRight,
            colors);

        if (doBoundingBox)
            q.DrawBoundingBox(_gl);
        else
            q.Render(_gl, texture);
    }

    private void RenderRegularImage(RenderTreeImage image, bool doBoundingBox = false)
    {
        var texture = GetTexture(image.FrontendObject.ResourceRequest);

        if (texture == null)
            return;

        var otkMat4 = image.Transform * Matrix4x4.CreateTranslation(320, 240, 0);

        // top left, top right, bottom right, bottom left
        Vector4[] colors = new Vector4[4];

        colors[0] = image.BlendedColor;
        colors[1] = image.BlendedColor;
        colors[2] = image.BlendedColor;
        colors[3] = image.BlendedColor;

        var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
            1.0f,
            otkMat4,
            image.UpperLeft,
            image.LowerRight,
            colors);

        if (doBoundingBox)
            q.DrawBoundingBox(_gl);
        else
            q.Render(_gl, texture);
    }

    private void RenderMultiImage(RenderTreeMultiImage image, bool doBoundingBox = false)
    {
        var texture = GetTexture(image.FrontendObject.ResourceRequest);

        if (texture == null)
            return;

        var otkMat4 = image.Transform * Matrix4x4.CreateTranslation(320, 240, 0);

        // top left, top right, bottom right, bottom left
        Vector4[] colors = new Vector4[4];

        colors[0] = image.BlendedColor;
        colors[1] = image.BlendedColor;
        colors[2] = image.BlendedColor;
        colors[3] = image.BlendedColor;

        var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
            1.0f,
            otkMat4,
            image.UpperLeft,
            image.LowerRight,
            colors);

        if (doBoundingBox)
            q.DrawBoundingBox(_gl);
        else
            q.Render(_gl, texture);
    }

    private void RenderGroupBB(RenderTreeNode node, bool doBoundingBox = false)
    {
        var otkMat4 = node.Transform * Matrix4x4.CreateTranslation(320, 240, 0);

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
        if (resource is not { Type: ResourceType.Image })
        {
            return null;
        }

        if (_resourceRequestToTexture.TryGetValue(resource.ID, out var texture)) return texture;

        var key = CleanResourcePath(resource.Name);
        if (!_loadedTextures.TryGetValue(key, out texture))
        {
            Debug.WriteLine("Texture not found: {0}", new object[] { key });
        }
        else
        {
            _resourceRequestToTexture[resource.ID] = texture;
        }

        //Debug.WriteLine("Texture not found: {0}", new object[] { key });
        return texture;
    }

    private static string CleanResourcePath(string path)
    {
        return path.Split('\\')[^1].Split('.')[0].ToUpperInvariant();
    }
}