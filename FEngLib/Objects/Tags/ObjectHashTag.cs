using System.IO;

namespace FEngLib.Objects.Tags;

public class ObjectHashTag : ObjectTag
{
    public ObjectHashTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public uint Hash { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        Hash = br.ReadUInt32();
    }
}