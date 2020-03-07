using System;
using System.IO;
using FEngLib;

namespace FEngTestLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            FrontendPackage package = LoadDumpedChunk(@"test-data\mw\UI_OL_Disconnect_BG.fng");
        }

        private static FrontendPackage LoadDumpedChunk(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read) { Position = 0x10 };
            using var br = new BinaryReader(fs);
            return new FrontendPackageLoader().Load(br);
        }
    }
}
