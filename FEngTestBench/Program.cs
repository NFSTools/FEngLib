using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using FEngLib;
using FEngLib.Packages;

namespace FEngTestBench;

internal static class Program
{
    private static void Main(string[] args)
    {
        var stopwatch = Stopwatch.StartNew();
        var indir = args[0];
        Console.WriteLine("Running test bench on directory {0}", indir);
        var packages = new List<Package>();
        var paths = Directory.GetFiles(args[0], "*.fng", SearchOption.AllDirectories);
        var relpathsMap = new Dictionary<Package, string>();

        var outdir = indir + "_rewrite";
        foreach (var fngPath in paths)
        {
            stopwatch.Restart();
            var package = LoadPackageFromChunk(fngPath);
            stopwatch.Stop();

            Console.WriteLine("Loaded {2} (file: {0}) in {1}ms", fngPath, stopwatch.ElapsedMilliseconds, package.Name);

            packages.Add(package);
            var relpath = Path.GetRelativePath(indir, fngPath);
            relpathsMap.Add(package, relpath);
        }

        var matching = 0;
        var total = 0;
        foreach (var package in packages)
        {
            var relpath = relpathsMap[package];
            var origPath = indir + Path.DirectorySeparatorChar + relpath;
            var rewrittenPath = outdir + Path.DirectorySeparatorChar + relpath;

            Directory.CreateDirectory(Path.GetDirectoryName(rewrittenPath) ?? throw new InvalidOperationException());

            stopwatch.Restart();
            SavePackageToChunk(package, rewrittenPath);
            stopwatch.Stop();

            Console.WriteLine("Wrote {2} (file: {0}) in {1}ms", rewrittenPath, stopwatch.ElapsedMilliseconds,
                package.Name);

            LoadPackageFromChunk(rewrittenPath);

            var orig = new FileInfo(origPath);
            var rewrite = new FileInfo(rewrittenPath);

            Console.Write($"{relpath,-50} ...");
            if (!FilesAreEqual_Hash(orig, rewrite))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAIL");
            }
            else
            {
                matching++;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("PASS");
            }

            Console.ResetColor();
            Console.WriteLine();

            total++;
        }

        Console.WriteLine("PASSED: {0}/{1} ({2}%)", matching, total, ((float)matching / total) * 100f);
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

    private static void SavePackageToChunk(Package pkg, string path)
    {
        using var fs = new FileStream(path, FileMode.Create);
        using var fw = new BinaryWriter(fs);

        using var ms = new MemoryStream();
        ms.Position = 0;

        using var bw = new BinaryWriter(ms);
        new FrontendChunkWriter(pkg).Write(bw);

        fw.Write(0x30203);
        fw.Write((uint)ms.Length);
        fs.Position = 8; // todo needed?

        bw.Flush();
        ms.Position = 0;
        ms.CopyTo(fs);

        fs.Flush();
    }

    private static bool FilesAreEqual_Hash(FileInfo first, FileInfo second)
    {
        var firstStream = first.OpenRead();
        ValidateAndPrepareFngStream(firstStream);
        var secondStream = second.OpenRead();
        ValidateAndPrepareFngStream(secondStream);
        var firstHash = SHA256.Create().ComputeHash(firstStream);
        var secondHash = SHA256.Create().ComputeHash(secondStream);

        for (var i = 0; i < firstHash.Length; i++)
        {
            if (firstHash[i] != secondHash[i])
                return false;
        }

        return true;
    }

    private static void ValidateAndPrepareFngStream(FileStream fs)
    {
        var fr = new BinaryReader(fs);

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
                throw new InvalidDataException($"Invalid FEng chunk file.");
        }
    }
}