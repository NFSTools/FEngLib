using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace FEngRender.OpenGL
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexDeclaration
    {
        public Vector3 Position;
        public Color4 Color;
        public Vector2 TexCoords;

        /// <summary>
        /// Sets up OpenGL for this vertex declaration.
        /// </summary>
        public static void Declare()
        {
            static void DeclAttrib(int index, int sz, string name)
            {
                GL.VertexAttribPointer(index, sz, VertexAttribPointerType.Float, false,
                    Marshal.SizeOf<VertexDeclaration>(),
                    Marshal.OffsetOf<VertexDeclaration>(name));
                GL.EnableVertexAttribArray(index);
            }

            DeclAttrib(0, 3, "Position");
            DeclAttrib(1, 4, "Color");
            DeclAttrib(2, 2, "TexCoords");
        }
    }
}