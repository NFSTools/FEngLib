using System.IO;

namespace FEngLib.Objects.Tags;

public class ObjectParentTag : ObjectTag
{
    public ObjectParentTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint ParentId { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        ParentId = br.ReadUInt32();
    }
}