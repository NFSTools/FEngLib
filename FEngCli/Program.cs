﻿using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using FEngLib.Packages;
using FEngRender;
using FEngRender.Data;
using JetBrains.Annotations;
using SixLabors.ImageSharp;

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

            var outputFile = options.OutputFile;
            if (!string.IsNullOrWhiteSpace(outputFile))
            {
                var renderer = new ImageRenderTreeRenderer();
                renderer.LoadTextures(options.TextureDir);
                var img = renderer.Render(RenderTree.Create(package));
                using var fs = File.OpenWrite(outputFile);
                img.SaveAsPng(fs);

                if (!options.NoOpen) Process.Start(new ProcessStartInfo(outputFile) { UseShellExecute = true });
            }

            //Console.WriteLine(JsonConvert.SerializeObject(package, Formatting.Indented, new JsonSerializerSettings()
            //{
            //    TypeNameHandling = TypeNameHandling.Auto,
            //    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            //    Converters =
            //    {
            //        new StringEnumConverter(new DefaultNamingStrategy())
            //    }
            //}));

            return 0;
        }

        private static Package LoadPackageFromChunk(string path)
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
            [UsedImplicitly]
            public string InputFile { get; set; }

            [UsedImplicitly]
            [Option('o', "output")] public string OutputFile { get; set; }

            [UsedImplicitly]
            [Option('t', "textures", Required = true)]
            public string TextureDir { get; set; }

            [UsedImplicitly]
            [Option('q', "no-open")] public bool NoOpen { get; set; }
        }
    }
}