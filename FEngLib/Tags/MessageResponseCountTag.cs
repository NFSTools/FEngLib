using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class MessageResponseCountTag : FrontendTag
    {
        public MessageResponseCountTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            var value = br.ReadUInt32();
        }
    }
}