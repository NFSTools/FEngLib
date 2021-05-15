using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ImageInfoTag : FrontendTag
    {
        public ImageInfoTag(IObject<ObjectData> frontendObject) : base(frontendObject)
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