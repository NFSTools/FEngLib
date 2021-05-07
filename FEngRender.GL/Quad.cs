using OpenTK.Mathematics;
using SharpGL;
using SharpGL.Enumerations;

namespace FEngRender.GL
{
    public class Quad
    {
        private VertexDeclaration[] _vertices = new VertexDeclaration[4];

        private static readonly int[] Indices =
        {
            0, 1, 2, 3
        };

        private readonly Matrix4 _transform;

        private const bool DoZ = false;

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

            _transform = transform;
            _vertices[0].Position = MultIgnoringCol4(transform, _vertices[0].Position);
            _vertices[1].Position = MultIgnoringCol4(transform, _vertices[1].Position);
            _vertices[2].Position = MultIgnoringCol4(transform, _vertices[2].Position);
            _vertices[3].Position = MultIgnoringCol4(transform, _vertices[3].Position);

            if (!DoZ)
            {
                _vertices[0].Position.Z = 0f;
                _vertices[1].Position.Z = 0f;
                _vertices[2].Position.Z = 0f;
                _vertices[3].Position.Z = 0f;
            }

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

        public const float XScale = 1.0f / 320.0f;
        private const float YScale = 1.0f / 240.0f;

        public void Render(OpenGL gl, Texture tex)
        {
            tex.GLTexture.Bind(gl);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            
            gl.Begin(BeginMode.TriangleFan);

            for (var j = 0; j < _vertices.Length; j++)
            {
                _vertices[j].Position.X = _vertices[j].Position.X * XScale - 1.0f;
                _vertices[j].Position.Y = -(_vertices[j].Position.Y * YScale - 1.0f);

                _vertices[j].TexCoords.X += 0.5f / tex.Width;
                _vertices[j].TexCoords.Y += 0.5f / tex.Height;
            }

            foreach (var vertex in _vertices)
            {
                gl.Color(vertex.Color.R, vertex.Color.G, vertex.Color.B, vertex.Color.A);
                gl.TexCoord(vertex.TexCoords.X, vertex.TexCoords.Y);
                gl.Vertex(vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
            }

            gl.End();
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