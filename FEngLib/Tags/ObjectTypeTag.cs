using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ObjectTypeTag : FrontendTag
    {
        public FEObjType Type { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Type = br.ReadEnum<FEObjType>();
        }

        public ObjectTypeTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}