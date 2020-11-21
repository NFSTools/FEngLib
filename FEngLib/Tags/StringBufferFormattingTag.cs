using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class StringBufferFormattingTag : FrontendTag
    {
        public StringBufferFormattingTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public FEStringFormatting Formatting { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Formatting = br.ReadEnum<FEStringFormatting>();
        }
    }
}