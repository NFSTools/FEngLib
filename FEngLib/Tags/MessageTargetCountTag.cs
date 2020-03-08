using System.IO;

namespace FEngLib.Tags
{
    public class MessageTargetCountTag : FrontendTag
    {
        public MessageTargetCountTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            br.ReadUInt32();
        }
    }
}