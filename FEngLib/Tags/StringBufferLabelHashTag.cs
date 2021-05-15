using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class StringBufferLabelHashTag : FrontendTag
    {
        public StringBufferLabelHashTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint Hash { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Hash = br.ReadUInt32();
        }
    }
}