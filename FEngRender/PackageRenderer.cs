using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib;
using FEngLib.Data;
using FEngLib.Objects;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FEngRender
{
    /// <summary>
    ///     Renders objects from a <see cref="FrontendPackage" /> to an image.
    /// </summary>
    public class PackageRenderer
    {
        private readonly FrontendPackage _package;
        private readonly string _textureDir;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PackageRenderer" /> class.
        /// </summary>
        /// <param name="package">The package to render.</param>
        /// <param name="textureDir"></param>
        public PackageRenderer(FrontendPackage package, string textureDir)
        {
            _package = package;
            _textureDir = textureDir;
        }

        /// <summary>
        ///     Renders the package to an <see cref="Image" /> object.
        /// </summary>
        /// <returns>The rendered image.</returns>
        public Image<Rgba32> Render()
        {
            const int width = /*1280*/ 640;
            const int height = /*960*/ 480;

            var img = new Image<Rgba32>(width, height, /*Rgba32.ParseHex("#000000ff")*/ Color.Black);

            var renderOrderItems = new List<RenderOrderItem>();
            var goodGuids = new List<uint>
            {
                0xE6D9B
            };

            foreach (var frontendObject in _package.Objects)
            {
                if (frontendObject.Type != FEObjType.FE_Image && frontendObject.Type != FEObjType.FE_String) continue;
                if ( /*frontendObject.Color.Alpha == 0 || */IsInvisible(frontendObject)) continue;
                // if (!goodGuids.Contains(frontendObject.Guid)) continue;
                // if (frontendObject.ResourceIndex != 7) continue;

                var computedMatrix = ComputeObjectMatrix(frontendObject);
                renderOrderItems.Add(new RenderOrderItem(computedMatrix, frontendObject, width / 2, height / 2));
            }

            foreach (var renderOrderItem in renderOrderItems.OrderByDescending(o => o.GetZ()))
            {
                var frontendObject = renderOrderItem.FrontendObject;
                var x = renderOrderItem.GetX();
                var y = renderOrderItem.GetY();
                var sizeX = renderOrderItem.GetSizeX();
                var sizeY = renderOrderItem.GetSizeY();

                Console.WriteLine(
                    "Object 0x{0:X}: position=({1}, {2}), size=({3}, {4}), color={5}, type={6}, resource={7}",
                    frontendObject.Guid, x, y, sizeX, sizeY, frontendObject.Color, frontendObject.Type,
                    _package.ResourceRequests[frontendObject.ResourceIndex].Name);

                if (x < 0 || x > width || y < 0 || y > height)
                {
                    Console.WriteLine("Object out of bounds. Skipping.");
                    continue;
                }

                Console.WriteLine("Computed matrix: {0}", renderOrderItem.ObjectMatrix);

                if (frontendObject.Type == FEObjType.FE_Image)
                {
                    var resource = _package.ResourceRequests[frontendObject.ResourceIndex];

                    if (resource.Type != FEResourceType.RT_Image)
                    {
                        Console.WriteLine($"Expected resource type to be RT_Image, but it was {resource.Type}");
                        continue;
                    }

                    var resourceFile = Path.Combine(_textureDir, $"{resource.Name.Split('.')[0]}.png");

                    if (!File.Exists(resourceFile))
                    {
                        Console.WriteLine($"Cannot find resource file: {resourceFile}");
                        continue;
                    }

                    img.Mutate(m =>
                    {
                        var image = Image.Load(resourceFile);
                        image.Mutate(c =>
                        {
                            if (sizeX < 0)
                            {
                                c.Flip(FlipMode.Horizontal);
                                sizeX = -sizeX;
                            }

                            if (sizeY < 0)
                            {
                                c.Flip(FlipMode.Vertical);
                                sizeY = -sizeY;
                            }

                            var objectRotation = ComputeObjectRotation(frontendObject);
                            var atb = new AffineTransformBuilder();
                            atb.AppendScale(new SizeF(sizeX / image.Width, sizeY / image.Height));

                            c.Transform(atb);

                            // var eulerAngles = QuaternionToEuler(objectRotation);
                            // var xRotateDeg = eulerAngles.Roll * (180f / Math.PI);
                            // var yRotateDeg = eulerAngles.Pitch * (180f / Math.PI);
                            // var zRotateDeg = eulerAngles.Yaw * (180f / Math.PI);

                            // Console.WriteLine("Rotation quaternion to degrees: {0} {1} {2}", xRotateDeg, yRotateDeg, zRotateDeg);
                            // var rotateDeg = ExtractZRotation(objectRotation) * (180f / Math.PI);

                            // if (sizeX < 0)
                            // {
                            //     c.Flip(FlipMode.Horizontal);
                            //     sizeX = -sizeX;
                            // }
                            //
                            // if (sizeY < 0)
                            // {
                            //     c.Flip(FlipMode.Vertical);
                            //     sizeY = -sizeY;
                            // }
                            //
                            // c.Resize((int) sizeX, (int) sizeY);
                            //
                            // c.Rotate((float) xRotateDeg);
                            // c.Rotate((float) zRotateDeg);

                            // if (zRotateDeg != 0)
                            // {
                            //     Console.WriteLine("Computed rotation: {1} - rotating object by {0} degrees.", zRotateDeg,
                            //         objectRotation);
                            //     c.Rotate((float) zRotateDeg);
                            // }

                            var redScale = frontendObject.Color.Red / 255f;
                            var greenScale = frontendObject.Color.Green / 255f;
                            var blueScale = frontendObject.Color.Blue / 255f;
                            var alphaScale = frontendObject.Color.Alpha / 255f;

                            c.ProcessPixelRowsAsVector4(span =>
                            {
                                // foreach (var vector4 in span)
                                for (var i = 0; i < span.Length; i++)
                                {
                                    span[i].X *= redScale;
                                    span[i].Y *= greenScale;
                                    span[i].Z *= blueScale;
                                    span[i].W *= alphaScale;
                                }
                            });

                            Console.WriteLine("Applied channel filter");
                        });
                        m.DrawImage(
                            image, new Point((int) x, (int) y), frontendObject.Color.Alpha / 255f);
                    });
                }
                else if (frontendObject is FrontendString frontendString && !string.IsNullOrEmpty(frontendString.Value))
                {
                    Console.WriteLine("\tDrawing text in format {1}: {0}", frontendString.Value,
                        frontendString.Formatting);

                    img.Mutate(m =>
                    {
                        var font = new Font(SystemFonts.Find("Segoe UI"), 12);
                        var rect = TextMeasurer.Measure(frontendString.Value, new RendererOptions(font));

                        float CalculateXOffset(uint justification, float lineWidth)
                        {
                            if ((justification & 1) == 1) return lineWidth * -0.5f;

                            if ((justification & 2) == 2) return -lineWidth;

                            return 0;
                        }

                        float CalculateYOffset(uint justification, float textHeight)
                        {
                            if ((justification & 4) == 4) return textHeight * -0.5f;

                            if ((justification & 8) == 8) return -textHeight;

                            return 0;
                        }

                        var xOffset = CalculateXOffset((uint) frontendString.Formatting,
                            rect.Width);
                        var yOffset = CalculateYOffset((uint) frontendString.Formatting,
                            rect.Height);

                        m.DrawText(frontendString.Value, font,
                            Color.FromRgba((byte) (frontendObject.Color.Red & 0xff),
                                (byte) (frontendObject.Color.Green & 0xff), (byte) (frontendObject.Color.Blue & 0xff),
                                (byte) (frontendObject.Color.Alpha & 0xff)),
                            new PointF(x + xOffset, y + yOffset));
                    });
                }
            }

            return img;
        }

        private static bool IsInvisible(FrontendObject frontendObject)
        {
            if (frontendObject.Parent is { } parent)
                return (frontendObject.Flags & FE_ObjectFlags.FF_Invisible) != 0 || IsInvisible(parent);

            return (frontendObject.Flags & FE_ObjectFlags.FF_Invisible) != 0;
        }

        private static Matrix4x4 ComputeObjectMatrix(FrontendObject frontendObject)
        {
            var matrix = new Matrix4x4(
                frontendObject.Size.X, 0, 0, 0,
                0, frontendObject.Size.Y, 0, 0,
                0, 0, frontendObject.Size.Z, 0,
                frontendObject.Position.X, frontendObject.Position.Y, frontendObject.Position.Z, 1);

            if (frontendObject.Parent is { } parentObject)
                matrix = Matrix4x4.Multiply(matrix, ComputeObjectMatrix(parentObject));

            return matrix;
        }

        private static Quaternion ComputeObjectRotation(FrontendObject frontendObject)
        {
            var q = new Quaternion(frontendObject.Rotation.X, frontendObject.Rotation.Y, frontendObject.Rotation.Z,
                frontendObject.Rotation.W);

            if (frontendObject.Parent is { } parent) return ComputeObjectRotation(parent) * q;

            return q;
        }

        private static EulerAngles QuaternionToEuler(Quaternion q)
        {
/*
    EulerAngles angles;

    // roll (x-axis rotation)
    double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
    double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
    angles.roll = std::atan2(sinr_cosp, cosr_cosp);

    // pitch (y-axis rotation)
    double sinp = 2 * (q.w * q.y - q.z * q.x);
    if (std::abs(sinp) >= 1)
        angles.pitch = std::copysign(M_PI / 2, sinp); // use 90 degrees if out of range
    else
        angles.pitch = std::asin(sinp);

    // yaw (z-axis rotation)
    double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
    double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
    angles.yaw = std::atan2(siny_cosp, cosy_cosp);

    return angles;
 */
            var sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            var cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            var roll = Math.Atan2(sinr_cosp, cosr_cosp);

            var sinp = 2 * (q.W * q.Y - q.Z * q.X);
            var pitch = Math.Abs(sinp) >= 1 ? Math.CopySign(Math.PI / 2, sinp) : Math.Asin(sinp);

            var siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            var cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            var yaw = Math.Atan2(siny_cosp, cosy_cosp);

            return new EulerAngles(roll, pitch, yaw);
        }

        private struct EulerAngles
        {
            public double Roll, Pitch, Yaw;

            public EulerAngles(double roll, double pitch, double yaw)
            {
                Roll = roll;
                Pitch = pitch;
                Yaw = yaw;
            }
        }

        private class RenderOrderItem
        {
            private readonly int _halfHeight;
            private readonly int _halfWidth;

            public RenderOrderItem(Matrix4x4 objectMatrix, FrontendObject frontendObject, int halfWidth, int halfHeight)
            {
                _halfWidth = halfWidth;
                _halfHeight = halfHeight;
                ObjectMatrix = objectMatrix;
                FrontendObject = frontendObject;
            }

            public Matrix4x4 ObjectMatrix { get; }
            public FrontendObject FrontendObject { get; }

            public float GetX()
            {
                var x = ObjectMatrix.M41 + _halfWidth;

                switch (FrontendObject.Type)
                {
                    case FEObjType.FE_Image:
                        x -= GetSizeX() * 0.5f;
                        break;
                }

                return x;
            }

            public float GetY()
            {
                var y = ObjectMatrix.M42 + _halfHeight;

                switch (FrontendObject.Type)
                {
                    case FEObjType.FE_Image:
                        y -= GetSizeY() * 0.5f;
                        break;
                }

                return y;
            }

            public float GetZ()
            {
                return ObjectMatrix.M43;
            }

            public float GetSizeX()
            {
                return ObjectMatrix.M11;
            }

            public float GetSizeY()
            {
                return ObjectMatrix.M22;
            }

            public float GetSizeZ()
            {
                return ObjectMatrix.M33;
            }
        }
    }
}