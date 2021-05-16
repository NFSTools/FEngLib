using System.Diagnostics;
using System.Numerics;
using FEngLib.Objects;
using FEngLib.Structures;
using SharpGL;
using SharpGL.Enumerations;
using SixLabors.Fonts;

namespace FEngRender.GL
{
    public class GLGlyphRenderer : IGlyphRenderer
    {
        public Matrix4x4 Transform;
        public Color4 Color;
        public TextFormat Formatting;

        private readonly OpenGL _gl;

        private Vector2 _currentPoint;

        public GLGlyphRenderer(OpenGL gl)
        {
            _gl = gl;
        }

        private const float XScale = 1.0f / 320.0f;
        private const float YScale = 1.0f / 240.0f;
        private const float Z = 0.0f;
        
        /// <summary>
        /// Called before any glyphs have been rendered.
        /// </summary>
        /// <param name="bounds">The bounds the text will be rendered at and at whats size.</param>
        void IGlyphRenderer.BeginText(FontRectangle bounds)
        {
            // called before any thing else to provide access to the total required size to redner the text

            _gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            _gl.Enable(OpenGL.GL_BLEND);

            // TODO why does the color stay black no matter what?
            Vector4 colorV = Color;
            _gl.Color(colorV.X, colorV.Y, colorV.Z, colorV.W);
        }

        /// <summary>
        /// Begins the glyph.
        /// </summary>
        /// <param name="bounds">The bounds the glyph will be rendered at and at what size.</param>
        /// <param name="paramaters">The set of paramaters that uniquely represents a version of a glyph in at particular font size, font family, font style and DPI.</param>
        /// <returns>Returns true if the glyph should be rendered othersie it returns false.</returns>
        bool IGlyphRenderer.BeginGlyph(FontRectangle bounds, GlyphRendererParameters paramaters)
        {
            // called before each glyph/glyph layer is rendered.
            // The paramaters can be used to detect the exact details
            // of the glyph so that duplicate glyphs could optionally 
            // be cached to reduce processing.

            // You can return false to skip all the figures within the glyph (if you return false EndGlyph will still be called)
            
            return true;
        }

        /// <summary>
        /// Begins the figure.
        /// </summary>
        void IGlyphRenderer.BeginFigure()
        {
            // called at the start of the figure within the single glyph/layer
            // glyphs are rendered as a serise of arcs, lines and movements 
            // which together describe a complex shape.
        }

        /// <summary>
        /// Sets a new start point to draw lines from
        /// </summary>
        /// <param name="point">The point.</param>
        void IGlyphRenderer.MoveTo(Vector2 point)
        {
            // move current point to location marked by point without describing a line;
            
            _currentPoint = Vector2.Transform(point, Transform);
        }

        /// <summary>
        /// Draw a quadratic bezier curve connecting the previous point to <paramref name="point"/>.
        /// </summary>
        /// <param name="secondControlPoint">The second control point.</param>
        /// <param name="point">The point.</param>
        void IGlyphRenderer.QuadraticBezierTo(Vector2 secondControlPoint, Vector2 point)
        {
            // describes Quadratic Bezier curve from the 'current point' using the 
            // 'second control point' and final 'point' leaving the 'current point'
            // at 'point'
            
            // TODO actually implement proper bezier with Map1
            
            var secondCpTransformed = Vector2.Transform(secondControlPoint, Transform);
            var nextPointTransformed = Vector2.Transform(point, Transform);

            _gl.Begin(BeginMode.LineStrip);
            {
                Vertex(_currentPoint);
                Vertex(secondCpTransformed);
                _currentPoint = nextPointTransformed;
                Vertex(_currentPoint);
            }
            _gl.End();
        }

        /// <summary>
        /// Draw a Cubics bezier curve connecting the previous point to <paramref name="point"/>.
        /// </summary>
        /// <param name="secondControlPoint">The second control point.</param>
        /// <param name="thirdControlPoint">The third control point.</param>
        /// <param name="point">The point.</param>
        void IGlyphRenderer.CubicBezierTo(Vector2 secondControlPoint, Vector2 thirdControlPoint, Vector2 point)
        {
            // describes Cubic Bezier curve from the 'current point' using the 
            // 'second control point', 'third control point' and final 'point' 
            // leaving the 'current point' at 'point'
            
            // TODO actually implement proper bezier with Map1
            
            var secondCpTransformed = Vector2.Transform(secondControlPoint, Transform);
            var thirdCpTransformed = Vector2.Transform(thirdControlPoint, Transform);
            var nextPointTransformed = Vector2.Transform(point, Transform);
            
            _gl.Begin(BeginMode.LineStrip);
            {
                Vertex(_currentPoint);
                Vertex(secondCpTransformed);
                Vertex(thirdCpTransformed);
                _currentPoint = nextPointTransformed;
                Vertex(_currentPoint);
            }
            _gl.End();
        }

        /// <summary>
        /// Draw a straight line connecting the previous point to <paramref name="point"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        void IGlyphRenderer.LineTo(Vector2 point)
        {
            // describes straight line from the 'current point' to the final 'point' 
            // leaving the 'current point' at 'point'

            var nextPointTransformed = Vector2.Transform(point, Transform);
            
            _gl.Begin(BeginMode.Lines);
            {
                Vertex(_currentPoint);
                _currentPoint = nextPointTransformed;
                Vertex(_currentPoint);
            }
            _gl.End();
        }

        /// <summary>
        /// Ends the figure.
        /// </summary>
        void IGlyphRenderer.EndFigure()
        {
            // Called after the figure has completed denoting a straight line should 
            // be drawn from the current point to the first point
        }

        /// <summary>
        /// Ends the glyph.
        /// </summary>
        void IGlyphRenderer.EndGlyph()
        {
            Debug.Write("\n");
            // says the all figures have completed for the current glyph/layer.
            // NOTE this will be called even if BeginGlyph return false.
        }


        /// <summary>
        /// Called once all glyphs have completed rendering
        /// </summary>
        void IGlyphRenderer.EndText()
        {
            //once all glyphs/layers have been drawn this is called.
        }
        
        private void Vertex(float x, float y)
        {
            _gl.Vertex(x * XScale - 1.0f, -(y * YScale - 1.0f), Z);
        }
        
        private void Vertex(Vector2 v)
        {
            Vertex(v.X, v.Y);
        }
    }
}