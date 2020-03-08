using System.IO;

namespace FEngLib.Tags
{
    public class StringBufferLengthTag : FrontendTag
    {
        public StringBufferLengthTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, ushort length)
        {
            br.ReadUInt32();
        }
    }
}