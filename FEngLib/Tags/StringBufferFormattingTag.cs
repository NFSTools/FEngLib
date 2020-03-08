using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class StringBufferFormattingTag : FrontendTag
    {
        public FEStringFormatting Formatting { get; set; }
        public StringBufferFormattingTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Formatting = br.ReadEnum<FEStringFormatting>();
            //Debug.WriteLine(Formatting.ToString());
        }
    }
}