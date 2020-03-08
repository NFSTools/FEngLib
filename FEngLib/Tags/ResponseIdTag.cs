using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class ResponseIdTag : FrontendTag
    {
        public uint Id { get; set; }

        public ResponseIdTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort length)
        {
            Id = br.ReadUInt32();
            //Debug.WriteLine("ResponseId: {0:X8}", Id);
        }
    }
}