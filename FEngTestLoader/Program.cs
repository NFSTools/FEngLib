using System;
using System.IO;
using FEngLib;

namespace FEngTestLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            FrontendPackage package = LoadDumpedChunk(@"test-data\mw\InGameRivalBio.fng");
        }

        private static FrontendPackage LoadDumpedChunk(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read) { Position = 0x10 };
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;
            ms.SetLength(fs.Length - 0x10);
            using var br = new BinaryReader(ms);
            return new FrontendPackageLoader().Load(br);
        }
    }
}
