using System.IO;

namespace FEngLib.Tags
{
    public class StringBufferMaxWidthTag : FrontendTag
    {
        public StringBufferMaxWidthTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public uint MaxWidth { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            MaxWidth = br.ReadUInt32();
        }
    }
}