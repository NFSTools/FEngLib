using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class MessageResponseInfoTag : FrontendTag
    {
        public uint Hash { get; set; }

        public MessageResponseInfoTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Hash = br.ReadUInt32();
        }
    }
}