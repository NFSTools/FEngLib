using System.Numerics;
using System.Runtime.InteropServices;
using SharpGL;

namespace FEngRender.GL;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VertexDeclaration
{
    public Vector3 Position;
    public Vector4 Color;
    public Vector2 TexCoords;

    /// <summary>
    /// Sets up OpenGL for this vertex declaration.
    /// Currently unused (not needed with immediate mode).
    /// </summary>
    public static void Declare(OpenGL gl)
    {
        void DeclAttrib(uint index, int sz, string name)
        {
            gl.VertexAttribPointer(index, sz, OpenGL.GL_FLOAT, false,
                Marshal.SizeOf<VertexDeclaration>(),
                Marshal.OffsetOf<VertexDeclaration>(name));
            gl.EnableVertexAttribArray(index);
        }

        DeclAttrib(0, 3, "Position");
        DeclAttrib(1, 4, "Color");
        DeclAttrib(2, 2, "TexCoords");
    }
}