using System.IO;

namespace FEngLib.Tags
{
    public class MessageResponseInfoTag : FrontendTag
    {
        public uint Hash { get; set; }

        public MessageResponseInfoTag(FrontendObject frontendObject) : base(frontendObject)
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