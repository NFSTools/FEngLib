using System;
using System.IO;
using FEngLib;
using JetBrains.Annotations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FEngCli
{
    /// <summary>
    ///     Renders objects from a <see cref="FrontendPackage" /> to an image.
    /// </summary>
    public class PackageRenderer
    {
        private readonly FrontendPackage _package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PackageRenderer" /> class.
        /// </summary>
        /// <param name="package">The package to render.</param>
        public PackageRenderer(FrontendPackage package)
        {
            _package = package;
        }

        /// <summary>
        ///     Renders the package to a PNG image file.
        /// </summary>
        /// <param name="imagePath">The destination path of the image.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="imagePath" /> is <c>null</c></exception>
        public void RenderToPng([NotNull] string imagePath)
        {
            if (imagePath == null) throw new ArgumentNullException(nameof(imagePath));
            using var img = new Image<Rgba32>(640, 480, Rgba32.ParseHex("#000000ff"));

            // img.Mutate(x => x.DrawText("Player One Press START", SystemFonts.CreateFont("Segoe UI", 18), Color.FromRgb(204, 244, 138), new PointF(320, 240)));
            // img.Mutate(x => x.DrawText("Paint Appropreate Layer", SystemFonts.CreateFont("Segoe UI", 12), Color.FromRgb(202, 244, 138), new PointF(224.854164f, 86.5f)));
            // img.Mutate(x => 
            //     x.Draw(
            //             new ShapeGraphicsOptions(), 
            //             Pens.Solid(Color.Aqua, 1), 
            //             new RectangularPolygon(
            //                 113, 344, 16, 16)));
            //

            img.Mutate(mutator =>
            {
                float width = 16;
                float height = 16;

                float x = 0;
                float y = 0;

                // float x = 113;
                // float y = (480 / 2) + 130;
                // float x = (640 - width) / 2;
                // float y = (480 - height) / 2;
                mutator.Fill(Color.FromRgb(0, 128, 192),
                    new RectangleF(new PointF(x, y), new SizeF(width, height)));
                // x.Fill().Draw(new ShapeGraphicsOptions(), Pens.Solid(Color.White, 1),
                //     new RectangularPolygon((640 - width) / 2f, (480 - height) / 2f, width, height));
            });

            using var fs = File.OpenWrite(imagePath);
            img.SaveAsPng(fs);
        }
    }
}