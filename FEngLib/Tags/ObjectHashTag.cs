using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ObjectHashTag : FrontendTag
    {
        public uint Hash { get; set; }

        public override void Read(BinaryReader br, ushort length)
        {
            Hash = br.ReadUInt32();
            Debug.WriteLine("Object hash: {0:X8}", Hash);
        }

        public ObjectHashTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}