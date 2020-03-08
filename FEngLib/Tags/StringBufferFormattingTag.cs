using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class StringBufferFormattingTag : FrontendTag
    {
        public FEStringFormatting Formatting { get; set; }
        public StringBufferFormattingTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, ushort length)
        {
            Formatting = br.ReadEnum<FEStringFormatting>();
            Debug.WriteLine(Formatting.ToString());
        }
    }
}