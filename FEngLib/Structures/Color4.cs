using System.Numerics;

namespace FEngLib.Structures;

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

    /// <summary>
    /// Converts a <see cref="Color4"/> to a <see cref="Vector4"/> that represents the levels of each channel.
    /// </summary>
    /// <param name="c">The <see cref="Color4"/> to convert.</param>
    public static implicit operator Vector4(Color4 c) =>
        new Vector4(c.Red / 255f, c.Green / 255f, c.Blue / 255f, c.Alpha / 255f);

    /// <summary>
    /// Blends one <see cref="Color4"/> with another, and returns the result.
    /// </summary>
    /// <param name="first">The first <see cref="Color4"/></param>
    /// <param name="second">The second <see cref="Color4"/></param>
    /// <returns>A new <see cref="Color4"/> representing the result of blending</returns>
    public static Color4 Blend(Color4 first, Color4 second)
    {
        return new Color4
        {
            Blue = (first.Blue * second.Blue + 128) >> 8,
            Green = (first.Green * second.Green + 128) >> 8,
            Red = (first.Red * second.Red + 128) >> 8,
            Alpha = (first.Alpha * second.Alpha + 128) >> 8
        };
    }
}