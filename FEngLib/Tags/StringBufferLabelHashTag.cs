using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class StringBufferLabelHashTag : FrontendTag
    {
        public uint Hash { get; set; }
        public StringBufferLabelHashTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, ushort length)
        {
            Hash = br.ReadUInt32();
            Debug.WriteLine("LabelHash={0:X8}", Hash);
        }
    }
}