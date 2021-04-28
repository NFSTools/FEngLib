using System.IO;

namespace FEngLib.Tags
{
    public class ResponseIntParamTag : FrontendTag
    {
        public ResponseIntParamTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public uint Param { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            Param = br.ReadUInt32();
        }
    }
}