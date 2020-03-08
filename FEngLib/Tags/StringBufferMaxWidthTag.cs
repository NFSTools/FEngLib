using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class StringBufferMaxWidthTag : FrontendTag
    {
        public uint MaxWidth { get; set; }
        public StringBufferMaxWidthTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort length)
        {
            MaxWidth = br.ReadUInt32();
            Debug.WriteLine(MaxWidth);
        }
    }
}