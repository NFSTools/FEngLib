using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib;
using FEngLib.Data;
using FEngLib.Objects;
using FEngLib.Tags;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FEngRender
{
    /// <summary>
    /// Implements render-tree rendering APIs.
    /// </summary>
    public class RenderTreeRenderer
    {
        private const int Width = 640;
        private const int Height = 480;

        public RenderTreeNode SelectedNode { get; set; }
        private (float width, float height, float x, float y) _boundingBox;

        private readonly Dictionary<string, Image> _textures = new Dictionary<string, Image>();

        public void LoadTextures(string directory)
        {
            _textures.Clear();
            foreach (var pngFile in Directory.GetFiles(directory, "*.png"))
            {
                var filename = Path.GetFileNameWithoutExtension(pngFile) ?? "";
                _textures.Add(filename.ToUpperInvariant(), Image.Load(pngFile));
            }
        }

        /// <summary>
        /// Renders nodes from a <see cref="RenderTree"/> to a surface.
        /// </summary>
        /// <param name="tree">The <see cref="RenderTree"/> to render.</param>
        public Image<Rgba32> Render(RenderTree tree)
        {
            var img = new Image<Rgba32>(Width, Height, Color.Black);

            _boundingBox = (0, 0, 0, 0);
            ComputeObjectMatrices(tree, Matrix4x4.Identity);
            RenderTree(img, tree);

            // make sure we have a box to draw
            if (_boundingBox.width != 0)
            {
                img.Mutate(m => m.Draw(Color.Red, 1,
                    new RectangleF(new PointF(_boundingBox.x, _boundingBox.y), new SizeF(_boundingBox.width, _boundingBox.height))));
            }

            return img;
        }

        private void ComputeObjectMatrices(IEnumerable<RenderTreeNode> nodes,
            Matrix4x4 viewMatrix)
        {
            var nodeList = nodes.ToList();

            nodeList.ForEach(r => r.ComputeObjectMatrix(viewMatrix));
            foreach (var renderTreeGroup in nodeList.OfType<RenderTreeGroup>())
            {
                ComputeObjectMatrices(renderTreeGroup, renderTreeGroup.ObjectMatrix);
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

        private void RenderTree(Image<Rgba32> surface, IEnumerable<RenderTreeNode> nodes)
        {
            foreach (var renderTreeNode in GetAllTreeNodes(nodes).OrderByDescending(n => n.GetZ()))
            {
                RenderNode(surface, renderTreeNode);
            }
        }

        private void RenderNode(Image<Rgba32> surface, RenderTreeNode node)
        {
            switch (node.FrontendObject)
            {
                case FrontendImage image:
                    RenderImage(surface, node.ObjectMatrix, image);
                    break;
                case FrontendString str:
                    RenderString(surface, node.ObjectMatrix, str);
                    break;
            }
        }

        private void RenderString(Image<Rgba32> surface, Matrix4x4 imgMatrix, FrontendString str)
        {
            float posX = imgMatrix.M41 + Width / 2f;
            float posY = imgMatrix.M42 + Height / 2f;
            surface.Mutate(m =>
            {
                var rect = TextRendering.MeasureText(str.Value);
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
                    Color.FromRgba((byte)(str.Color.Red & 0xff),
                        (byte)(str.Color.Green & 0xff), (byte)(str.Color.Blue & 0xff),
                        (byte)(str.Color.Alpha & 0xff)),
                    new PointF(posX, posY));
                if (SelectedNode?.FrontendObject?.Guid == str.Guid)
                {
                    _boundingBox = (rect.Width, rect.Height, posX, posY);
                }
            });
        }

        private void RenderImage(Image<Rgba32> surface, Matrix4x4 imgMatrix, FrontendImage image)
        {
            float sizeX = imgMatrix.M11;
            float sizeY = imgMatrix.M22;
            float posX = imgMatrix.M41 + Width / 2f - sizeX * 0.5f;
            float posY = imgMatrix.M42 + Height / 2f - sizeY * 0.5f;

            // Bounds checking
            if (posX < 0 || posY < 0 || posX > Width || posY > Height)
                return;

            var texture = GetTexture(image.ResourceRequest);

            if (texture == null)
                return;

            surface.Mutate(m =>
            {
                var clone = texture.Clone(c =>
                {
                    if (sizeX < 0)
                    {
                        c.Flip(FlipMode.Horizontal);
                        sizeX = -sizeX;
                        posX -= sizeX;
                    }

                    if (sizeY < 0)
                    {
                        c.Flip(FlipMode.Vertical);
                        sizeY = -sizeY;
                        posY -= sizeY;
                    }

                    if ((int)sizeX == 0 || (int)sizeY == 0)
                    {
                        return;
                    }

                    c.Resize((int)sizeX, (int)sizeY);
                    var rotationQuaternion = ComputeObjectRotation(image);
                    var eulerAngles = QuaternionToEuler(rotationQuaternion);
                    var rotateX = eulerAngles.Roll * (180 / Math.PI);
                    var rotateY = eulerAngles.Pitch * (180 / Math.PI);
                    var rotateZ = eulerAngles.Yaw * (180 / Math.PI);

                    // TODO: ELIMINATE THESE UGLY HACKS
                    if (Math.Abs(Math.Abs(rotateX) - 180) < 0.1) c.Flip(FlipMode.Vertical);
                    if (Math.Abs(Math.Abs(rotateY) - 180) < 0.1) c.Flip(FlipMode.Horizontal);

                    c.Rotate((float)rotateZ);

                    var redScale = image.Color.Red / 255f;
                    var greenScale = image.Color.Green / 255f;
                    var blueScale = image.Color.Blue / 255f;
                    var alphaScale = image.Color.Alpha / 255f;
                    var scaleVector = new Vector4(redScale, greenScale, blueScale, alphaScale);

                    c.ProcessPixelRowsAsVector4(span =>
                    {
                        for (var i = 0; i < span.Length; i++)
                        {
                            span[i] *= scaleVector;
                        }
                    });
                });
                m.DrawImage(
                    clone, new Point((int)posX, (int)posY), image.Color.Alpha / 255f);

                /*
                 *                             m.Draw(Color.Red, 1,
                                new RectangleF(new PointF(x, y), new SizeF(image.Width, image.Height)));
                 */
                if (SelectedNode?.FrontendObject?.Guid == image.Guid)
                {
                    _boundingBox = (clone.Width, clone.Height, posX, posY);
                }
            });

        }

        private Image GetTexture(FEResourceRequest resource)
        {
            if (resource.Type != FEResourceType.RT_Image)
            {
                return null;
            }

            var key = CleanResourcePath(resource.Name);
            return _textures.TryGetValue(key, out var img) ? img : null;
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

        private static EulerAngles QuaternionToEuler(Quaternion q)
        {
            var sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            var cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            var roll = Math.Atan2(sinr_cosp, cosr_cosp);

            var sinp = 2 * (q.W * q.Y - q.Z * q.X);
            var pitch = Math.Abs(sinp) >= 1 ? Math.CopySign(Math.PI / 2, sinp) : Math.Asin(sinp);

            var siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            var cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            var yaw = Math.Atan2(siny_cosp, cosy_cosp);

            return new EulerAngles(roll, pitch, yaw);
        }

        private struct EulerAngles
        {
            public readonly double Roll;
            public readonly double Pitch;
            public readonly double Yaw;

            public EulerAngles(double roll, double pitch, double yaw)
            {
                Roll = roll;
                Pitch = pitch;
                Yaw = yaw;
            }
        }

    }
}