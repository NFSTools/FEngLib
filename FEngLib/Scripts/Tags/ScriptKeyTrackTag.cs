using System.Diagnostics;
using System.IO;
using FEngLib.Objects;

namespace FEngLib.Scripts.Tags;

public class ScriptKeyTrackTag : ScriptTag
{
    public ScriptKeyTrackTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) :
        base(frontendObject,
            scriptProcessingContext)
    {
    }

    public byte ParamType { get; set; }
    public byte ParamSize { get; set; }
    public byte InterpType { get; set; }
    public byte InterpAction { get; set; }
    public uint Length { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        ParamType = br.ReadByte();
        ParamSize = br.ReadByte();
        InterpType = br.ReadByte();
        InterpAction = br.ReadByte();
        var propBits = br.ReadUInt32();
        Length = propBits & 0xffffff;
        var trackOffset = (propBits >> 24) & 0xff;

        Debug.Assert(trackOffset == 0, "trackOffset == 0");
    }
}