using System.Drawing;
using FEngLib.Packages;

namespace FEngRender.GL;

public interface ITextureProvider
{
    Bitmap GetTexture(ResourceRequest resourceRequest);
}