using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Object.Tags
{
    public class StringBufferMaxWidthTag : Tag
    {
        public StringBufferMaxWidthTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint MaxWidth { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            MaxWidth = br.ReadUInt32();
        }
    }
}