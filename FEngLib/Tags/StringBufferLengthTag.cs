using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class StringBufferLengthTag : FrontendTag
    {
        public StringBufferLengthTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            br.ReadUInt32();
        }
    }
}