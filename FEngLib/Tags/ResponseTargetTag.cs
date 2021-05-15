using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ResponseTargetTag : FrontendTag
    {
        public ResponseTargetTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint Target { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            Target = br.ReadUInt32();
        }
    }
}