using System.Numerics;
using SharpGL;
using SharpGL.Enumerations;

namespace FEngRender.GL
{
    public class Quad
    {
        private VertexDeclaration[] _vertices = new VertexDeclaration[4];

        public Quad(
            float left, float up,
            float right, float down,
            float z,
            Matrix4x4 transform,
            Vector2 texTopLeft, Vector2 texBottomRight,
            OpenTK.Mathematics.Color4[] colors)
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

            _vertices[0].Position = Vector3.Transform(_vertices[0].Position, transform);
            _vertices[1].Position = Vector3.Transform(_vertices[1].Position, transform);
            _vertices[2].Position = Vector3.Transform(_vertices[2].Position, transform);
            _vertices[3].Position = Vector3.Transform(_vertices[3].Position, transform);

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

            foreach (var vertex in _vertices)
            {
                gl.Color(vertex.Color.R, vertex.Color.G, vertex.Color.B, vertex.Color.A);
                gl.TexCoord(vertex.TexCoords.X + 0.5f / tex.Width, vertex.TexCoords.Y + 0.5f / tex.Height);
                gl.Vertex(vertex.Position.X * XScale - 1.0f, -(vertex.Position.Y * YScale - 1.0f), 0);
            }

            gl.End();
        }

        public void DrawBoundingBox(OpenGL gl)
        {
            gl.Begin(BeginMode.LineLoop);
            
            gl.Color(1.0f, 0, 0, 1.0f);
            foreach (var vertex in _vertices)
            {
                gl.Vertex(vertex.Position.X * XScale - 1.0f, -(vertex.Position.Y * YScale - 1.0f), 0);
            }

            gl.End();
        }
    }
}