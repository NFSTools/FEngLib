using System.IO;
using System.Text;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Object.Tags
{
    public class StringBufferTextTag : Tag
    {
        public StringBufferTextTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public string Value { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            Value = Encoding.Unicode.GetString(br.ReadBytes(length)).Trim('\0');
        }
    }
}