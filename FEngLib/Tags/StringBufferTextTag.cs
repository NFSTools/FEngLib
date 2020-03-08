using System.Diagnostics;
using System.IO;
using System.Text;

namespace FEngLib.Tags
{
    public class StringBufferTextTag : FrontendTag
    {
        public string Value { get; set; }

        public StringBufferTextTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, ushort length)
        {
            Value = Encoding.Unicode.GetString(br.ReadBytes(length));
            Debug.WriteLine(Value);
        }
    }
}