using System.Diagnostics;
using System.IO;
using CommandLine;
using FEngRender;
using FEngRender.Data;
using JetBrains.Annotations;
using SixLabors.ImageSharp;

namespace FEngCli;

[Verb("render")]
public class RenderCommand : BaseCommand
{
    [Option('i', "input", Required = true)]
    [UsedImplicitly]
    public string InputFile { get; set; }

    [UsedImplicitly]
    [Option('o', "output")]
    public string OutputFile { get; set; }

    [UsedImplicitly]
    [Option('t', "textures", Required = true)]
    public string TextureDir { get; set; }

    [UsedImplicitly]
    [Option('q', "no-open")]
    public bool NoOpen { get; set; }

    public override int Execute()
    {
        var package = PackageLoader.Load(InputFile);
        PackageDumper.DumpPackage(package);

        var outputFile = OutputFile;
        if (!string.IsNullOrWhiteSpace(outputFile))
        {
            var renderer = new ImageRenderTreeRenderer();
            renderer.LoadTextures(TextureDir);
            var img = renderer.Render(RenderTree.Create(package));
            using var fs = File.OpenWrite(outputFile);
            img.SaveAsPng(fs);

            if (!NoOpen) Process.Start(new ProcessStartInfo(outputFile) { UseShellExecute = true });
        }

        return 0;
    }
}