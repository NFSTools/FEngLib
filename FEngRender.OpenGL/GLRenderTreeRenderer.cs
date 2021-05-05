using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib;
using FEngLib.Data;
using FEngLib.Objects;
using FEngRender.Data;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Quaternion = System.Numerics.Quaternion;


namespace FEngRender.OpenGL
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
        
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;

        public GLRenderTreeRenderer()
        {
            PrepareRender();
        }

        private void PrepareRender()
        {
            GL.ClearColor(Color4.Green);

            /*_vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            // BufferData??? todo in quad
            
            _shader = new Shader();
            _shader.Use();
            
            VertexDeclaration.Declare();*/
            
            
        }

        public void LoadTextures(string directory)
        {
            _textures.Clear();
            foreach (var pngFile in Directory.GetFiles(directory, "*.png"))
            {
                var filename = Path.GetFileNameWithoutExtension(pngFile) ?? "";
                _textures.Add(filename.ToUpperInvariant(), Texture.LoadFromFile(pngFile));
            }
        }

        /// <summary>
        /// Renders nodes from a <see cref="RenderTree"/> to a surface.
        /// </summary>
        /// <param name="tree">The <see cref="RenderTree"/> to render.</param>
        public void Render(RenderTree tree)
        {
            GL.ClearColor(Color4.MidnightBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (!CanRender(tree))
                return;
            
            _boundingBox = (0, 0, 0, 0);
            ComputeObjectMatrices(tree, Matrix4x4.Identity);
            RenderTree(tree);

            // make sure we have a box to draw
            /*if (_boundingBox.width > 0)
            {
                img.Mutate(m => m.Draw(
                    Color.Red, 
                    1, 
                    new RectangleF(_boundingBox.x, _boundingBox.y, _boundingBox.width, _boundingBox.height)));
            }*/
        }

        private bool CanRender(RenderTree tree)
        {
            return tree != null && _textures.Count > 0;
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
                    //RenderString(surface, node, str);
                    break;
            }
        }

        /*private void RenderString(Image<Rgba32> surface, RenderTreeNode node, FrontendString str)
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
        }*/

        private void RenderImage(RenderTreeNode node, FrontendImage image)
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

            /*surface.Mutate(m =>
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
                /*if (SelectedNode?.FrontendObject?.Guid == image.Guid)
                {
                    _boundingBox = (clone.Width, clone.Height, posX, posY);
                }
            });*/

        }

        private Texture GetTexture(FEResourceRequest resource)
        {
            if (resource.Type != FEResourceType.RT_Image)
            {
                return null;
            }

            var key = CleanResourcePath(resource.Name);
            return _textures.TryGetValue(key, out var tex) ? tex : null;
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