using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class MessageTargetCountTag : FrontendTag
    {
        public MessageTargetCountTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            br.ReadUInt32();
        }
    }
}