using System.IO;

namespace FEngLib.Structures
{
    public struct Color4
    {
        public int Blue;
        public int Green;
        public int Red;
        public int Alpha;

        public Color4(int blue, int green, int red, int alpha)
        {
            Blue = blue;
            Green = green;
            Red = red;
            Alpha = alpha;
        }

        public override string ToString()
        {
            return $"R: {Red} G: {Green} B: {Blue} A: {Alpha}";
        }
    }
}