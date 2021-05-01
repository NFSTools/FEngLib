using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using FEngLib;
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
            PackageDumper.DumpPackage(package);
            var renderer = new PackageRenderer(package, options.TextureDir);
            var outputFile = options.OutputFile ?? $"{package.Name}.png";
            renderer.RenderToPng(outputFile);
            Process.Start(new ProcessStartInfo(outputFile) {UseShellExecute = true});
            return 0;
        }

        private static FrontendPackage LoadPackageFromChunk(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            using var fr = new BinaryReader(fs);
            var marker = fr.ReadUInt32();
            switch (marker)
            {
                case 0x30203:
                    fs.Seek(0x10, SeekOrigin.Begin);
                    break;
                case 0xE76E4546:
                    fs.Seek(0x8, SeekOrigin.Begin);
                    break;
                default:
                    throw new InvalidDataException($"Invalid FEng chunk file: {path}");
            }

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

            [Option('o', "output")] public string OutputFile { get; set; }

            [Option('t', "textures", Required = true)]
            public string TextureDir { get; set; }
        }
    }
}