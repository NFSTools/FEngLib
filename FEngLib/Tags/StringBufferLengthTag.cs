using System.IO;

namespace FEngLib.Tags
{
    public class StringBufferLengthTag : FrontendTag
    {
        public StringBufferLengthTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort length)
        {
            br.ReadUInt32();
        }
    }
}