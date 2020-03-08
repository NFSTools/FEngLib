using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class StringBufferLeadingTag : FrontendTag
    {
        public int Leading { get; set; }
        public StringBufferLeadingTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Leading = br.ReadInt32();
            //Debug.WriteLine(Leading);
        }
    }
}