using System.IO;
using FEngLib.Objects;

namespace FEngLib.Tags
{
    public class MultiImageTextureFlagsTag : FrontendTag
    {
        public MultiImageTextureFlagsTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            FrontendMultiImage multiImage = (FrontendMultiImage) FrontendObject;
            int index = (id >> 8) - 0x61;

            multiImage.TextureFlags[index] = br.ReadUInt32();
        }
    }
}