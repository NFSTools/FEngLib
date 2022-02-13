using System.IO;

namespace FEngLib.Objects.Tags;

public class StringBufferLeadingTag : ObjectTag
{
    public StringBufferLeadingTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public int Leading { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        Leading = br.ReadInt32();
    }
}