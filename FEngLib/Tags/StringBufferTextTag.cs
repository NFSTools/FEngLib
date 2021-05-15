using System.IO;
using System.Text;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class StringBufferTextTag : FrontendTag
    {
        public StringBufferTextTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public string Value { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Value = Encoding.Unicode.GetString(br.ReadBytes(length)).Trim('\0');
        }
    }
}