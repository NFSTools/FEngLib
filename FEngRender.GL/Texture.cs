using System.Drawing;
using SharpGL;

namespace FEngRender.GL;

// A helper class, much like Shader, meant to simplify loading textures.
public class Texture
{
    public readonly SharpGL.SceneGraph.Assets.Texture GLTexture;
    public readonly int Width;
    public readonly int Height;

    public Texture(OpenGL gl, string path)
    {
        GLTexture = new SharpGL.SceneGraph.Assets.Texture();
        using var image = new Bitmap(path);

        Width = image.Width;
        Height = image.Height;
        GLTexture.Create(gl, image);
    }
}