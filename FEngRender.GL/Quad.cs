using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using SharpGL;
using SharpGL.Enumerations;

namespace FEngRender.GL;

public class Quad
{
    public const float XScale = 1.0f / 320.0f;
    private const float YScale = 1.0f / 240.0f;
    private readonly VertexDeclaration[] _vertices = new VertexDeclaration[4];

    public Quad(VertexDeclaration[] vertices)
    {
        _vertices = vertices;
    }

    public Quad(
        float left, float up,
        float right, float down,
        float z,
        Matrix4x4 transform,
        Vector2 texTopLeft, Vector2 texBottomRight,
        Vector4[] colors)
    {
        _vertices[0].Position = Vector3.Transform(new Vector3(left, up, z), transform);
        _vertices[1].Position = Vector3.Transform(new Vector3(right, up, z), transform);
        _vertices[2].Position = Vector3.Transform(new Vector3(right, down, z), transform);
        _vertices[3].Position = Vector3.Transform(new Vector3(left, down, z), transform);

        _vertices[0].TexCoords.X = texTopLeft.X;
        _vertices[0].TexCoords.Y = texTopLeft.Y;

        _vertices[1].TexCoords.X = texBottomRight.X;
        _vertices[1].TexCoords.Y = texTopLeft.Y;

        _vertices[2].TexCoords.X = texBottomRight.X;
        _vertices[2].TexCoords.Y = texBottomRight.Y;

        _vertices[3].TexCoords.X = texTopLeft.X;
        _vertices[3].TexCoords.Y = texBottomRight.Y;

        _vertices[0].Color = colors[0];
        _vertices[1].Color = colors[1];
        _vertices[2].Color = colors[2];
        _vertices[3].Color = colors[3];
    }

    public void Render(OpenGL gl, Texture tex = null)
    {
        gl.Enable(OpenGL.GL_TEXTURE_2D);
        gl.Enable(OpenGL.GL_BLEND);

        tex?.GLTexture.Push(gl);
        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, (int)OpenGL.GL_NEAREST);
        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, (int)OpenGL.GL_NEAREST);

        gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

        gl.Begin(BeginMode.Quads);

        foreach (var vertex in _vertices)
        {
            gl.Color(vertex.Color.X, vertex.Color.Y, vertex.Color.Z, vertex.Color.W);
            gl.TexCoord(vertex.TexCoords.X, vertex.TexCoords.Y);
            Vertex(gl, vertex.Position.X, vertex.Position.Y, 0);
        }

        gl.End();
        tex?.GLTexture.Pop(gl);

        gl.Disable(OpenGL.GL_BLEND);
        gl.Disable(OpenGL.GL_TEXTURE_2D);
    }

    public void DrawBoundingBox(OpenGL gl)
    {
        gl.Disable(OpenGL.GL_BLEND);
        gl.Begin(BeginMode.LineLoop);
        gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
        gl.Color(1.0f, 0, 0, 1.0f);
        foreach (var vertex in _vertices)
        {
            Vertex(gl, vertex.Position.X, vertex.Position.Y, 0);
        }

        gl.End();
    }

    private static void Vertex(OpenGL gl, float x, float y, float z)
    {
        gl.Vertex(x * XScale, -y * YScale, z);
    }

    public static Quad MaxBox(IEnumerable<Quad> quads)
    {
        Quad best = null;

        foreach (var quad in quads)
        {
            Debug.Assert(quad != null, "quad!=null");
            best = best == null ? quad : MaxBox(best, quad);
        }

        return best;
    }

    public static Quad MaxBox(Quad q1, Quad q2)
    {
        var vertices = new VertexDeclaration[4];

        // MINIMIZE top left X/Y
        var (tl1, tl2) = (q1._vertices[0].Position, q2._vertices[0].Position);
        vertices[0].Position = new Vector3(Math.Min(tl1.X, tl2.X), Math.Min(tl1.Y, tl2.Y), 0);

        // MAXIMIZE top right X, MINIMIZE top right Y
        var (tr1, tr2) = (q1._vertices[1].Position, q2._vertices[1].Position);
        vertices[1].Position = new Vector3(Math.Max(tr1.X, tr2.X), Math.Min(tr1.Y, tr2.Y), 0);

        // MAXIMIZE bottom right X/Y
        var (br1, br2) = (q1._vertices[2].Position, q2._vertices[2].Position);
        vertices[2].Position = new Vector3(Math.Max(br1.X, br2.X), Math.Max(br1.Y, br2.Y), 0);

        // MINIMIZE bottom left X, MAXIMIZE bottom left Y
        var (bl1, bl2) = (q1._vertices[3].Position, q2._vertices[3].Position);
        vertices[3].Position = new Vector3(Math.Min(bl1.X, bl2.X), Math.Max(bl1.Y, bl2.Y), 0);

        return new Quad(vertices);
    }
}