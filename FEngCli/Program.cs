using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using CommandLine;
using FEngLib;
using FEngLib.Objects;
using JetBrains.Annotations;

namespace FEngCli
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(RunApplication, _ => 1);
        }

        private static int RunApplication(Options options)
        {
            if (!File.Exists(options.InputFile))
            {
                Console.Error.WriteLine("File not found: {0}", options.InputFile);
                return 1;
            }

            var package = LoadPackageFromChunk(options.InputFile);
            DumpPackageInfo(package);
            var renderer = new PackageRenderer(package);
            var imagePath = $"{package.Name}.png";
            renderer.RenderToPng(imagePath);
            Process.Start(new ProcessStartInfo(imagePath) {UseShellExecute = true});
            return 0;
        }

        private static void DumpPackageInfo(FrontendPackage package)
        {
            foreach (var frontendObject in package.Objects) DumpObjectInfo(frontendObject);
        }

        private static void DumpObjectInfo(FrontendObject frontendObject)
        {
            Console.WriteLine(frontendObject);
            Console.WriteLine("\tPosition : {0}", frontendObject.Position);
            Console.WriteLine("\tSize     : {0}", frontendObject.Size);
            Console.WriteLine("\tExtent   : {0}",
                new Vector3(frontendObject.Position.X + frontendObject.Size.X,
                    frontendObject.Position.Y + frontendObject.Size.Y,
                    frontendObject.Position.Z + frontendObject.Size.Z));
            Console.WriteLine("\tPivot    : {0}", frontendObject.Pivot);
            Console.WriteLine("\tRotation : {0}", frontendObject.Rotation);
            Console.WriteLine("\tColor    : {0}", frontendObject.Color);
            Console.WriteLine("\tType     : {0}", frontendObject.Type);

            switch (frontendObject)
            {
                case FrontendString frontendString:
                    Console.WriteLine("\t\tText          : {0}", frontendString.Value);
                    Console.WriteLine("\t\tString hash   : {0:X8}", frontendString.Hash);
                    Console.WriteLine("\t\tMaximum width : {0}", frontendString.MaxWidth);
                    Console.WriteLine("\t\tText leading  : {0}", frontendString.Leading);
                    Console.WriteLine("\t\tText format   : {0}", frontendString.Formatting);
                    break;
            }
        }

        private static FrontendPackage LoadPackageFromChunk(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            using var fr = new BinaryReader(fs);

            if (fr.ReadUInt32() != 197123) throw new InvalidDataException($"Invalid FEng chunk file: {path}");

            fs.Seek(0x10, SeekOrigin.Begin);

            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;

            using var mr = new BinaryReader(ms);
            return new FrontendPackageLoader().Load(mr);
        }

        [UsedImplicitly]
        private class Options
        {
            [Option('i', "input", Required = true)]
            public string InputFile { get; set; }
        }
    }
}