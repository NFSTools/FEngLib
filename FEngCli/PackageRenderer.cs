using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FEngLib;
using JetBrains.Annotations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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

            var (x, y) = ComputeCoordinateExtremes();
            var width = (int) (x.max - x.min);
            var height = (int) (y.max - y.min);

            Debug.WriteLine("Computed extremes : X={0}, Y={1}", x, y);
            Debug.WriteLine("Computed canvas   : {0} by {1}", width, height);

            using var img = new Image<Rgba32>(width, height, Rgba32.ParseHex("#ff0000ff"));
            using var fs = File.OpenWrite(imagePath);
            img.SaveAsPng(fs);
        }

        private ((float min, float max) x, (float min, float max) y) ComputeCoordinateExtremes()
        {
            var minX = _package.Objects.Min(o => o.Position.X);
            var maxX = _package.Objects.Max(o => o.Position.X + o.Size.X);
            var minY = _package.Objects.Min(o => o.Position.Y);
            var maxY = _package.Objects.Max(o => o.Position.Y + o.Size.Y);

            return ((minX, maxX), (minY, maxY));
        }
    }
}