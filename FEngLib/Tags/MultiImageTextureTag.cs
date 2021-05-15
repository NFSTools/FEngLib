using System;
using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class MultiImageTextureTag : FrontendTag
    {
        public MultiImageTextureTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            MultiImage multiImage = (MultiImage) FrontendObject;
            int index = (id >> 8) - 0x31;

            switch (index)
            {
                case 0:
                    multiImage.Texture1 = br.ReadUInt32();
                    break;
                case 1:
                    multiImage.Texture2 = br.ReadUInt32();
                    break;
                case 2:
                    multiImage.Texture3 = br.ReadUInt32();
                    break;
                default:
                    throw new IndexOutOfRangeException($"Invalid MultiImageTexture index: {index}");
            }
        }
    }
}