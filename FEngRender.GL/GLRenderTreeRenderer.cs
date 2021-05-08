using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib;
using FEngLib.Data;
using FEngLib.Objects;
using FEngRender.Data;
using FEngRender.Utils;
using OpenTK.Mathematics;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.VertexBuffers;
using Quaternion = System.Numerics.Quaternion;
using Vector2 = OpenTK.Mathematics.Vector2;
using Texture = SharpGL.SceneGraph.Assets.Texture;

namespace FEngRender.GL
{
    /// <summary>
    /// Make sure to initialize an OpenGL context before using this.
    /// </summary>
    public class GLRenderTreeRenderer
    {
        private const int Width = 640;
        private const int Height = 480;

        public RenderTreeNode SelectedNode { get; set; }
        private (float width, float height, float x, float y) _boundingBox;

        private readonly Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

        private readonly OpenGL _gl;

        private Stopwatch _stopwatch;

        public GLRenderTreeRenderer(OpenGL gl)
        {
            _gl = gl;
            _stopwatch = Stopwatch.StartNew();
        }

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

            _boundingBox = (0, 0, 0, 0);
            _gl.LoadIdentity();

            // disable depth
            _gl.DepthMask(0);

            PrepareNodes(tree, Matrix4x4.Identity, (int) _stopwatch.ElapsedMilliseconds);
            RenderTree(tree);

            _gl.MatrixMode(MatrixMode.Projection);
            _gl.Ortho(0, 640, 480, 0, -1, 1);
            _gl.Flush();
            _stopwatch.Restart();
        }

        private void PrepareNodes(IEnumerable<RenderTreeNode> nodes,
            Matrix4x4 viewMatrix, int deltaTime, RenderTreeNode parent = null)
        {
            var nodeList = nodes.ToList();

            nodeList.ForEach(r => r.PrepareForRender(viewMatrix, parent, deltaTime));
            foreach (var renderTreeGroup in nodeList.OfType<RenderTreeGroup>())
            {
                PrepareNodes(renderTreeGroup, renderTreeGroup.ObjectMatrix, deltaTime, renderTreeGroup);
            }
        }

        private IEnumerable<RenderTreeNode> GetAllTreeNodes(IEnumerable<RenderTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                yield return node;

                if (!(node is RenderTreeGroup grp)) continue;

                foreach (var rtn in GetAllTreeNodes(grp))
                {
                    yield return rtn;
                }
            }
        }

        private void RenderTree(IEnumerable<RenderTreeNode> nodes)
        {
            foreach (var renderTreeNode in GetAllTreeNodes(nodes).OrderByDescending(n => n.GetZ()))
            {
                RenderNode(renderTreeNode);
            }
        }

        private void RenderNode(RenderTreeNode node)
        {
            switch (node.FrontendObject)
            {
                case FrontendImage image:
                    RenderImage(node, image);
                    break;
                case FrontendString str:
                    RenderString(node, str);
                    break;
            }
        }

        private void RenderString(RenderTreeNode node, FrontendString str)
        {
            var strMatrix = node.ObjectMatrix;
            float posX = strMatrix.M41 + Width / 2f;
            float posY = strMatrix.M42 + Height / 2f;
            // TODO
            /*surface.Mutate(m =>
            {
                var rect = TextRendering.MeasureText(str.Value, str.MaxWidth);
                var xOffset = TextRendering.CalculateXOffset((uint)str.Formatting,
                    rect.Width);
                var yOffset = TextRendering.CalculateYOffset((uint)str.Formatting,
                    rect.Height);

                posX += xOffset;
                posY += yOffset;

                m.DrawText(new TextGraphicsOptions(new GraphicsOptions(), new TextOptions
                    {
                        WrapTextWidth = str.MaxWidth
                    }),  str.Value, TextRendering.DefaultFont,
                    Color.FromRgba((byte)(node.ObjectColor.Red & 0xff),
                        (byte)(node.ObjectColor.Green & 0xff), (byte)(node.ObjectColor.Blue & 0xff),
                        (byte)(node.ObjectColor.Alpha & 0xff)),
                    new PointF(posX, posY));
                if (SelectedNode?.FrontendObject?.Guid == str.Guid)
                {
                    _boundingBox = (rect.Width, rect.Height, posX, posY);
                }
            });*/
        }

        private void RenderImage(RenderTreeNode node, FrontendImage image)
        {
            var texture = GetTexture(image.ResourceRequest);

            if (texture == null)
                return;

            var otkMat4 = new Matrix4(
                node.ObjectMatrix.M11, node.ObjectMatrix.M12, node.ObjectMatrix.M13, node.ObjectMatrix.M14,
                node.ObjectMatrix.M21, node.ObjectMatrix.M22, node.ObjectMatrix.M23, node.ObjectMatrix.M24,
                node.ObjectMatrix.M31, node.ObjectMatrix.M32, node.ObjectMatrix.M33, node.ObjectMatrix.M34,
                node.ObjectMatrix.M41, node.ObjectMatrix.M42, node.ObjectMatrix.M43, node.ObjectMatrix.M44);

            var feTranslateMat = Matrix4.Identity;
            feTranslateMat.M41 = 320.0f;
            feTranslateMat.M42 = 240.0f;
            feTranslateMat.M43 = 0.0f;

            otkMat4 *= feTranslateMat;

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
                (texture.Width / widthDivide) * image.UpperLeft.X,
                (texture.Height / heightDivide) * image.UpperLeft.Y
            );

            var texLowRight = new Vector2(
                (texture.Width / widthDivide) * image.LowerRight.X,
                (texture.Height / heightDivide) * image.LowerRight.Y
            );

            var color = new Color4(
                node.ObjectColor.Red / 255f, node.ObjectColor.Green / 255f, node.ObjectColor.Blue / 255f,
                node.ObjectColor.Alpha / 255f
            );

            Color4[] colors =
            {
                color, color, color, color
            };

            var q = new Quad(-0.5f, -0.5f, 0.5f, 0.5f,
                1.0f,
                otkMat4,
                texUpLeft,
                texLowRight,
                colors);

            q.Render(_gl, texture);
        }

        private Texture GetTexture(FEResourceRequest resource)
        {
            if (resource.Type != FEResourceType.RT_Image)
            {
                return null;
            }

            var key = CleanResourcePath(resource.Name);
            if (_textures.TryGetValue(key, out var tex))
                return tex;
            Debug.WriteLine("Texture not found: {0}", new object[] { key });
            return null;
        }

        private static string CleanResourcePath(string path)
        {
            return path.Split('\\')[^1].Split('.')[0].ToUpperInvariant();
        }

        private static Quaternion ComputeObjectRotation(FrontendObject frontendObject)
        {
            var q = new Quaternion(frontendObject.Rotation.X, frontendObject.Rotation.Y, frontendObject.Rotation.Z,
                frontendObject.Rotation.W);

            if (frontendObject.Parent is { } parent) return ComputeObjectRotation(parent) * q;

            return q;
        }
    }
}