using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class StringBufferLeadingTag : FrontendTag
    {
        public StringBufferLeadingTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public int Leading { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Leading = br.ReadInt32();
        }
    }
}