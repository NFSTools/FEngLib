﻿using System.Diagnostics;
using System.Drawing;
using SharpGL;

namespace FEngRender.GL;

// A helper class, much like Shader, meant to simplify loading textures.
public class Texture
{
    public readonly SharpGL.SceneGraph.Assets.Texture GLTexture;
    public readonly int Width;
    public readonly int Height;

    public Texture(OpenGL gl, Bitmap image)
    {
        GLTexture = new SharpGL.SceneGraph.Assets.Texture();
        Width = image.Width;
        Height = image.Height;
        GLTexture.Create(gl, (Bitmap) image.Clone());

        Debug.Assert((Width & (Width - 1)) == 0);
        Debug.Assert((Height & (Height - 1)) == 0);
    }
}