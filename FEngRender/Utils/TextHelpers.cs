using System.Collections.Generic;
using SixLabors.Fonts;

namespace FEngRender.Utils;

/// <summary>
/// Utilities for text rendering
/// </summary>
public static class TextHelpers
{
    private static Dictionary<float, Font> _fontCache = new Dictionary<float, Font>();

    //public static readonly Font DefaultFont = SystemFonts.CreateFont("Segoe UI", 12);

    public static Font GetFont(float size)
    {
        if (_fontCache.TryGetValue(size, out var font))
            return font;
        return _fontCache[size] = SystemFonts.CreateFont("Segoe UI", size);
    }

    public static FontRectangle MeasureText(string text, RendererOptions rendererOptions) =>
        TextMeasurer.Measure(text, rendererOptions);

    public static float CalculateXOffset(uint justification, float lineWidth)
    {
        if ((justification & 1) == 1) return lineWidth * -0.5f;

        if ((justification & 2) == 2) return -lineWidth;

        return 0;
    }

    public static float CalculateYOffset(uint justification, float textHeight)
    {
        if ((justification & 4) == 4) return textHeight * -0.5f;

        if ((justification & 8) == 8) return -textHeight;

        return 0;
    }
}