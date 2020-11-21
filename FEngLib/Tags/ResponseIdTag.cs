using System.IO;

namespace FEngLib.Tags
{
    public class ResponseIdTag : FrontendTag
    {
        public ResponseIdTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public uint Id { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            Id = br.ReadUInt32();
        }
    }
}