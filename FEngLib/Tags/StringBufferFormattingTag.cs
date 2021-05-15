using System.IO;
using FEngLib.Data;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class StringBufferFormattingTag : FrontendTag
    {
        public StringBufferFormattingTag(IObject<ObjectData> frontendObject) : base(frontendObject)
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