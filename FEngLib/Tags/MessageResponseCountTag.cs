using System.IO;

namespace FEngLib.Tags
{
    public class MessageResponseCountTag : FrontendTag
    {
        public MessageResponseCountTag(FrontendObject frontendObject) : base(frontendObject)
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