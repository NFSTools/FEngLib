using SixLabors.Fonts;

namespace FEngRender
{
    /// <summary>
    /// Utilities for text rendering
    /// </summary>
    public static class TextRendering
    {
        public static readonly Font DefaultFont = SystemFonts.CreateFont("Segoe UI", 12);

        public static FontRectangle MeasureText(string text) =>
            TextMeasurer.Measure(text, new RendererOptions(DefaultFont));

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
}