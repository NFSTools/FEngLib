using System.IO;

namespace FEngLib.Tags
{
    public class ImageInfoTag : FrontendTag
    {
        public ImageInfoTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public uint ImageFlags { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            ImageFlags = br.ReadUInt32();
        }
    }
}