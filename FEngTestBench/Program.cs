using System;
using System.Diagnostics;
using System.IO;
using FEngLib.Packages;

namespace FEngTestBench;

internal static class Program
{
    private static void Main(string[] args)
    {
        var stopwatch = Stopwatch.StartNew();
        Console.WriteLine("Running test bench on directory {0}", args[0]);
        foreach (var fngPath in Directory.GetFiles(args[0], "*.fng", SearchOption.AllDirectories))
        {
            stopwatch.Restart();
            var package = LoadPackageFromChunk(fngPath);
            stopwatch.Stop();
            Console.WriteLine("Loaded {2} (file: {0}) in {1}ms", fngPath, stopwatch.ElapsedMilliseconds, package.Name);
        }
    }

    private static Package LoadPackageFromChunk(string path)
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
}