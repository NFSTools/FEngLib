using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using FEngLib.Packages;
using FEngRender.GL;
using FEngViewer.Properties;

namespace FEngViewer;

public class TextureProvider : ITextureProvider
{
    private Dictionary<string, Bitmap> _loadedBitmaps = new();
    private Bitmap _defaultTexture;

    public TextureProvider()
    {
        _defaultTexture = Resources.DefaultTexture;
    }

    public Bitmap GetTexture(ResourceRequest resourceRequest)
    {
        // if the resource isn't in the cache, we might still have it, but try not to waste time
        if (resourceRequest is not { Type: ResourceType.Image } || string.IsNullOrWhiteSpace(resourceRequest.Name))
        {
            // TODO(2024/01/07): this should really just throw. there's no reason for us to be calling GetTexture on font resources, for example.
            // "there's no reason" but we're doing it anyway... oh well
            return _defaultTexture;
        }

        var cleanedName = Path.GetFileNameWithoutExtension(resourceRequest.Name).ToUpperInvariant();

        return _loadedBitmaps.GetValueOrDefault(cleanedName, _defaultTexture);
    }

    public void LoadTextures(string directory)
    {
        CleanBitmaps();
        foreach (var pngFile in Directory.GetFiles(directory, "*.png"))
        {
            var filename = Path.GetFileNameWithoutExtension(pngFile);
            _loadedBitmaps.Add(filename.ToUpperInvariant(), new Bitmap(pngFile));
        }
    }

    private void CleanBitmaps()
    {
        // should we dispose bitmaps? yeah, probably.
        // does it actually matter? right now, no.
        _loadedBitmaps.Clear();
    }
}