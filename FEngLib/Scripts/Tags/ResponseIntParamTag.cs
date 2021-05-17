using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Scripts.Tags
{
    public class ResponseIntParamTag : Tag
    {
        public ResponseIntParamTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public uint Param { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package, ushort id,
            ushort length)
        {
            Param = br.ReadUInt32();
        }
    }
}