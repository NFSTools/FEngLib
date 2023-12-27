using System;

namespace FEngLib.Objects;

[Flags]
public enum TextFormat : uint
{
    JustifyHorizontalCenter = 0x0,
    JustifyHorizontalRight = 0x1,
    JustifyVerticalCenter = 0x2,
    JustifyVerticalBottom = 0x3,
    WordWrap = 0x4,
}

public class Text : BaseObject
{
    public Text(ObjectData data) : base(data)
    {
    }

    public uint BufferLength { get; set; }
    public string Value { get; set; }
    public string Label { get; set; }
    public uint Hash { get; set; }
    public TextFormat Formatting { get; set; }
    public int Leading { get; set; }
    public uint MaxWidth { get; set; }

    public override ObjectType GetObjectType()
    {
        return ObjectType.String;
    }

    public override void InitializeData()
    {
        Data = new ObjectData();
    }
}