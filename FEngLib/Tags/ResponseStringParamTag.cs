using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ResponseStringParamTag : FrontendTag
    {
        public ResponseStringParamTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public string Param { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            Param = new string(br.ReadChars(length)).Trim('\x00');
        }
    }
}