using System;
using System.IO;

namespace FEngLib.Objects.Tags;

public class MultiImageTextureTag : ObjectTag
{
    public MultiImageTextureTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override void Read(BinaryReader br, ushort id,
        ushort length)
    {
        MultiImage multiImage = (MultiImage)FrontendObject;
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