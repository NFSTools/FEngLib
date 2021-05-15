using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ObjectParentTag : FrontendTag
    {
        public ObjectParentTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint ParentId { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            ParentId = br.ReadUInt32();
        }
    }
}