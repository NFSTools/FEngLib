using System;
using System.IO;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Objects.Tags;

public class MultiImageTextureFlagsTag : Tag
{
    public MultiImageTextureFlagsTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package, ushort id,
        ushort length)
    {
        MultiImage multiImage = (MultiImage)FrontendObject;
        int index = (id >> 8) - 0x61;

        switch (index)
        {
            case 0:
                multiImage.TextureFlags1 = br.ReadUInt32();
                break;
            case 1:
                multiImage.TextureFlags2 = br.ReadUInt32();
                break;
            case 2:
                multiImage.TextureFlags3 = br.ReadUInt32();
                break;
            default:
                throw new IndexOutOfRangeException($"Invalid MultiImageTextureFlags index: {index}");
        }
    }
}