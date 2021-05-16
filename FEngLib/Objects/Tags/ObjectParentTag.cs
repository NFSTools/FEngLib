using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Object.Tags
{
    public class ObjectParentTag : Tag
    {
        public ObjectParentTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint ParentId { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            ParentId = br.ReadUInt32();
        }
    }
}