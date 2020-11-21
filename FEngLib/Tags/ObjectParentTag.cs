using System.IO;

namespace FEngLib.Tags
{
    public class ObjectParentTag : FrontendTag
    {
        public ObjectParentTag(FrontendObject frontendObject) : base(frontendObject)
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