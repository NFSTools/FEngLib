using System.Collections.Generic;
using FEngLib.Objects;
using SixLabors.Fonts;

namespace FEngRender.Utils
{
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

        public static float CalculateXOffset(TextFormat format, float lineWidth)
        {
            if ((format & TextFormat.JustifyHorizontalCenter) == TextFormat.JustifyHorizontalCenter)
                return lineWidth * -0.5f;
            if ((format & TextFormat.JustifyHorizontalRight) == TextFormat.JustifyHorizontalRight)
                return -lineWidth;

            return 0;
        }

        public static float CalculateYOffset(TextFormat format, float textHeight)
        {
            if ((format & TextFormat.JustifyVerticalCenter) == TextFormat.JustifyVerticalCenter)
                return textHeight * -0.5f;

            if ((format & TextFormat.JustifyVerticalBottom) == TextFormat.JustifyVerticalBottom)
                return -textHeight;

            return 0;
        }
    }
}