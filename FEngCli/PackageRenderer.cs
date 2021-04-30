using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using FEngLib;
using FEngLib.Data;
using JetBrains.Annotations;
using SixLabors.ImageSharp;
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
        ///     Renders the package to a PNG image file.
        /// </summary>
        /// <param name="imagePath">The destination path of the image.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="imagePath" /> is <c>null</c></exception>
        public void RenderToPng([NotNull] string imagePath)
        {
            if (imagePath == null) throw new ArgumentNullException(nameof(imagePath));
            using var img = new Image<Rgba32>(640, 480, /*Rgba32.ParseHex("#000000ff")*/ Color.Black);

            var renderOrderItems = new List<RenderOrderItem>();
            var goodGuids = new HashSet<uint>
            {
                0x10EFB0, // scroll bar GRAY
                0x10EFAF, // scroll bar WHITE

                0x10EFB4, // light gray background
                0xF271C // dark gray background
            };

            foreach (var frontendObject in _package.Objects)
            {
                if (frontendObject.Type != FEObjType.FE_Image /* || !goodGuids.Contains(frontendObject.Guid)*/
                ) continue;
                if (frontendObject.Color.Alpha == 0) continue;

                var computedMatrix = ComputeObjectMatrix(frontendObject);
                renderOrderItems.Add(new RenderOrderItem(computedMatrix, frontendObject));
            }

            foreach (var renderOrderItem in renderOrderItems.OrderByDescending(o => o.GetZ()))
            {
                var frontendObject = renderOrderItem.FrontendObject;
                var x = renderOrderItem.GetX();
                var y = renderOrderItem.GetY();

                if (x < 0 || x > 640 || y < 0 || y > 480)
                {
                    Console.WriteLine("off-screen, not rendering");
                    continue;
                }

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
                        var scaleX = frontendObject.Size.X;
                        var scaleY = frontendObject.Size.Y;
                        var objectRotation = ComputeObjectRotation(frontendObject);
                        var rotateDeg = ExtractZRotation(objectRotation) * (180f / Math.PI);

                        if (scaleX < 0)
                        {
                            c.Flip(FlipMode.Horizontal);
                            scaleX = -scaleX;
                        }

                        if (scaleY < 0)
                        {
                            c.Flip(FlipMode.Vertical);
                            scaleY = -scaleY;
                        }

                        c.Resize((int) scaleX, (int) scaleY);
                        c.Rotate((float) rotateDeg);

                        Console.WriteLine(
                            "Object 0x{0:X}: OriginalPos={1}, Size={4}, NewPos=({2}, {3}), Color={5}, Rotation={6} [{7} deg]",
                            frontendObject.Guid, frontendObject.Position, x, y, frontendObject.Size,
                            frontendObject.Color, objectRotation, rotateDeg);
                        Console.WriteLine(renderOrderItem.ObjectMatrix);
                        // Handle rotation
                        // c.Resize((int) frontendObject.Size.X, (int) frontendObject.Size.Y);
                        // c.Fill(Color.FromRgb(
                        //     (byte) (frontendObject.Color.Red & 0xff),
                        //     (byte) (frontendObject.Color.Green & 0xff),
                        //     (byte) (frontendObject.Color.Blue & 0xff)));
                    });
                    m.DrawImage(
                        image, new Point((int) x, (int) y), frontendObject.Color.Alpha / 255f);
                });
            }

            // img.Mutate(m =>
            // {
            //     m.DrawText("Bruh", new Font(SystemFonts.Find("Segoe UI"), 12), Color.Aqua,
            //         new PointF(54.524994f, 113.525f));
            // });

            using var fs = File.OpenWrite(imagePath);
            img.SaveAsPng(fs);

            var obj = _package.FindObjectByGuid(0x10EFAD);
            var obj2 = _package.FindObjectByGuid(0x10EFAC);
            var ro = new RenderOrderItem(ComputeObjectMatrix(obj), obj);
            Console.WriteLine("{0}/{1}/{2}", ro.GetX(), ro.GetY(), ro.GetZ());

            // Console.WriteLine(Quaternion.Multiply(
            //     new Quaternion(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.Rotation.W), 
            //     new Quaternion(obj2.Rotation.X, obj2.Rotation.Y, obj2.Rotation.Z, obj2.Rotation.W)));
            // Console.WriteLine(Quaternion.Multiply(
            //     new Quaternion(obj2.Rotation.X, obj2.Rotation.Y, obj2.Rotation.Z, obj2.Rotation.W), 
            //     new Quaternion(obj.Rotation.X, obj.Rotation.Y, obj.Rotation.Z, obj.Rotation.W)));
            // Console.WriteLine(
            //     Matrix4x4.Multiply(
            //         Matrix4x4.CreateFromQuaternion(new Quaternion(0, 0, 0.7071068f, 0.7071067f)),
            //         Matrix4x4.CreateFromQuaternion(new Quaternion(0, 0, 0, 1))));
            // Console.WriteLine(
            //     Matrix4x4.Multiply(
            //         Matrix4x4.CreateFromQuaternion(new Quaternion(0, 0, 0, 1)),
            //         Matrix4x4.CreateFromQuaternion(new Quaternion(0, 0, 0.7071068f, 0.7071067f))));
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

        private static string MatrixToString(Matrix4x4 matrix)
        {
            return
                $"[({matrix.M11}, {matrix.M12}, {matrix.M13}, {matrix.M14}), ({matrix.M21}, {matrix.M22}, {matrix.M23}, {matrix.M24}), ({matrix.M31}, {matrix.M32}, {matrix.M33}, {matrix.M34}), ({matrix.M41}, {matrix.M42}, {matrix.M43}, {matrix.M44})]";
        }

        private static double ExtractZRotation(Quaternion q)
        {
            /*
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
            return Math.Atan2(2 * (q.W * q.Z + q.X * q.Y), 1 - 2 * (q.Y * q.Y + q.Z * q.Z));
        }

        private class RenderOrderItem
        {
            public RenderOrderItem(Matrix4x4 objectMatrix, FrontendObject frontendObject)
            {
                ObjectMatrix = objectMatrix;
                FrontendObject = frontendObject;
            }

            public Matrix4x4 ObjectMatrix { get; }
            public FrontendObject FrontendObject { get; }

            public float GetX()
            {
                return ObjectMatrix.M41 - FrontendObject.Size.X * 0.5f + 320f;
            }

            public float GetY()
            {
                return ObjectMatrix.M42 - FrontendObject.Size.Y * 0.5f + 240f;
            }

            public float GetZ()
            {
                return ObjectMatrix.M43;
            }
        }
    }
}