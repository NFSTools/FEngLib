﻿using System.Numerics;
using FEngLib.Structures;

namespace FEngRender.Utils
{
    /// <summary>
    /// Helper functions for working with <see cref="Color4"/> objects
    /// </summary>
    public static class ColorHelpers
    {
        /// <summary>
        /// Blend the values of two <see cref="Color4"/> into a new <see cref="Color4"/>.
        /// </summary>
        /// <param name="c1">The first color</param>
        /// <param name="c2">The second color</param>
        /// <returns>The blended color</returns>
        public static Color4 BlendColors(Color4 c1, Color4 c2)
        {
            return new Color4
            {
                Blue = (c1.Blue * c2.Blue + 128) >> 8,
                Green = (c1.Green * c2.Green + 128) >> 8,
                Red = (c1.Red * c2.Red + 128) >> 8,
                Alpha = (c1.Alpha * c2.Alpha + 128) >> 8
            };
        }

        /// <summary>
        /// Compute channel levels (0-1) from a <see cref="Color4"/>.
        /// </summary>
        /// <param name="color">The color to compute channel levels for.</param>
        /// <returns>The channel levels</returns>
        public static Vector4 ColorToVector(Color4 color)
        {
            return new Vector4(
                color.Red / 255f, 
                color.Green / 255f, 
                color.Blue / 255f, 
                color.Alpha / 255f);
        }
    }
}