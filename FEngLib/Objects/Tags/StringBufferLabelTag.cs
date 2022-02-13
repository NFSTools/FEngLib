using System.IO;

namespace FEngLib.Objects.Tags;

public class StringBufferLabelTag : ObjectTag
{
    public StringBufferLabelTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public string Label { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        Label = new string(br.ReadChars(length)).Trim('\x00');
    }
}