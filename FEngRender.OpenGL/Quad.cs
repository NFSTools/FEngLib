using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FEngRender.OpenGL
{
    public class Quad
    {
        private VertexDeclaration[] _vertices = new VertexDeclaration[4];

        private static readonly int[] Indices =
        {
            0, 1, 2, 3
        };

        public Quad(
            float left, float up,
            float right, float down,
            float z,
            Matrix4 transform,
            Vector2 texTopLeft, Vector2 texBottomRight,
            Color4[] colors)
        {
            _vertices[0].Position.X = left;
            _vertices[0].Position.Y = up;
            _vertices[0].Position.Z = z;

            _vertices[1].Position.X = right;
            _vertices[1].Position.Y = up;
            _vertices[1].Position.Z = z;

            _vertices[2].Position.X = right;
            _vertices[2].Position.Y = down;
            _vertices[2].Position.Z = z;

            _vertices[3].Position.X = left;
            _vertices[3].Position.Y = down;
            _vertices[3].Position.Z = z;

            _vertices[0].Position = MultIgnoringCol4(transform, _vertices[0].Position);
            _vertices[1].Position = MultIgnoringCol4(transform, _vertices[1].Position);
            _vertices[2].Position = MultIgnoringCol4(transform, _vertices[2].Position);
            _vertices[3].Position = MultIgnoringCol4(transform, _vertices[3].Position);

            _vertices[0].TexCoords.X = texTopLeft.X;
            _vertices[0].TexCoords.Y = texTopLeft.Y;
            
            _vertices[1].TexCoords.X = texBottomRight.X;
            _vertices[1].TexCoords.Y = texTopLeft.X;
            
            _vertices[2].TexCoords.X = texBottomRight.X;
            _vertices[2].TexCoords.Y = texBottomRight.Y;
            
            _vertices[3].TexCoords.X = texTopLeft.X;
            _vertices[3].TexCoords.Y = texBottomRight.Y;

            _vertices[0].Color = colors[0];
            _vertices[1].Color = colors[1];
            _vertices[2].Color = colors[2];
            _vertices[3].Color = colors[3];
        }

        public void Render(Texture tex)
        {
            // TODO maybe? apply those weird float scaling things? probably
            // TODO apply texture adjustment
            // TODO lots of init in renderer
            // TODO probably pass some handles down in here after that
            tex.Use(TextureUnit.Texture0);

            GL.DrawElements(PrimitiveType.TriangleFan, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }
        
        // we need Vector3 for the vertex format
        private static Vector3 MultIgnoringCol4(Matrix4 mat, Vector3 vec)
        {
            var (x, y, z) = vec;
            return new Vector3
            {
                X = mat.M11 * x + mat.M21 * y + mat.M31 * z + mat.M41,
                Y = mat.M12 * x + mat.M22 * y + mat.M32 * z + mat.M42,
                Z = mat.M13 * x + mat.M23 * y + mat.M33 * z + mat.M43
            };
        }
    }
}