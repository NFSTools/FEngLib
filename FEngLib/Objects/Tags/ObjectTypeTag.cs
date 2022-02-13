using System.IO;
using FEngLib.Utils;

namespace FEngLib.Objects.Tags;

public class ObjectTypeTag : ObjectTag
{
    public ObjectTypeTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public ObjectType Type { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        Type = br.ReadEnum<ObjectType>();
    }
}