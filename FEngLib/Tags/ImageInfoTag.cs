using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class ImageInfoTag : FrontendTag
    {
        public uint Value { get; set; }

        public override void Read(BinaryReader br)
        {
            Value = br.ReadUInt32();

            if (Value != 0)
                Debugger.Break();
        }
    }
}