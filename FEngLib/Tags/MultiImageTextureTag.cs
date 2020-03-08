using System.IO;
using FEngLib.Objects;

namespace FEngLib.Tags
{
    public class MultiImageTextureTag : FrontendTag
    {
        public MultiImageTextureTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            FrontendMultiImage multiImage = (FrontendMultiImage) FrontendObject;
            int index = (id >> 8) - 0x31;

            multiImage.Texture[index] = br.ReadUInt32();
        }
    }
}