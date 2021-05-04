﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib;
using FEngLib.Data;
using FEngLib.Objects;
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

            if (_boundingBox.width > 0)
            {
                img.Mutate(m => m.Draw(
                    Color.Red, 
                    1, 
                    new RectangleF(_boundingBox.x, _boundingBox.y, _boundingBox.width, _boundingBox.height)));
            }

            return img;
        }

        private void ComputeObjectMatrices(IEnumerable<RenderTreeNode> nodes,
            Matrix4x4 viewMatrix, RenderTreeNode parent = null)
        {
            var nodeList = nodes.ToList();

            nodeList.ForEach(r => r.ApplyContext(viewMatrix, parent));
            foreach (var renderTreeGroup in nodeList.OfType<RenderTreeGroup>())
            {
                ComputeObjectMatrices(renderTreeGroup, renderTreeGroup.ObjectMatrix, renderTreeGroup);
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
                    RenderImage(surface, node, image);
                    break;
                case FrontendString str:
                    RenderString(surface, node, str);
                    break;
            }
        }

        private void RenderString(Image<Rgba32> surface, RenderTreeNode node, FrontendString str)
        {
            var strMatrix = node.ObjectMatrix;
            float posX = strMatrix.M41 + Width / 2f;
            float posY = strMatrix.M42 + Height / 2f;
            surface.Mutate(m =>
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
            });
        }

        private void RenderImage(Image<Rgba32> surface, RenderTreeNode node, FrontendImage image)
        {
            var imgMatrix = node.ObjectMatrix;
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

                    //c.SetGraphicsOptions(new GraphicsOptions {Antialias = false});
                    c.Resize((int)sizeX, (int)sizeY);
                    var rotationQuaternion = node.ObjectRotation;
                    var eulerAngles = MathHelpers.QuaternionToEuler(rotationQuaternion);
                    var rotateX = eulerAngles.Roll * (180 / Math.PI);
                    var rotateY = eulerAngles.Pitch * (180 / Math.PI);
                    var rotateZ = eulerAngles.Yaw * (180 / Math.PI);

                    // TODO: ELIMINATE THESE UGLY HACKS
                    if (Math.Abs(Math.Abs(rotateX) - 180) < 0.1) c.Flip(FlipMode.Vertical);
                    if (Math.Abs(Math.Abs(rotateY) - 180) < 0.1) c.Flip(FlipMode.Horizontal);

                    c.Rotate((float)rotateZ);

                    var colorScaleVector = ColorHelpers.GetLevels(node.ObjectColor);
                    
                    c.ProcessPixelRowsAsVector4(span =>
                    {
                        for (var i = 0; i < span.Length; i++)
                        {
                            span[i] *= colorScaleVector;
                        }
                    });
                });
                m.DrawImage(
                    clone, new Point((int)posX, (int)posY), node.ObjectColor.Alpha / 255f);

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
    }
}