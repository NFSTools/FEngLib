using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class ResponseTargetTag : FrontendTag
    {
        public uint Target { get; set; }

        public ResponseTargetTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort length)
        {
            Target = br.ReadUInt32();
            //Debug.WriteLine("ResponseTarget: {0:X8}", Target);
        }
    }
}