using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ObjectHashTag : FrontendTag
    {
        public uint Hash { get; set; }

        public override void Read(BinaryReader br)
        {
            Hash = br.ReadUInt32();
        }

        public ObjectHashTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}