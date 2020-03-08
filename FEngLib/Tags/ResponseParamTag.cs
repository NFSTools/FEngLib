using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class ResponseParamTag : FrontendTag
    {
        public uint Param { get; set; }

        public ResponseParamTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            Param = br.ReadUInt32();
            //Debug.WriteLine("ResponseParam: {0:X8}", Param);
        }
    }
}