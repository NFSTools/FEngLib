using System.IO;

namespace FEngLib.Tags
{
    public class StringBufferLabelTag : FrontendTag
    {
        public StringBufferLabelTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public string Label { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Label = new string(br.ReadChars(length)).Trim('\x00');
        }
    }
}