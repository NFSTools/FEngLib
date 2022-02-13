using System.IO;
using FEngLib.Objects;

namespace FEngLib.Scripts.Tags;

public class ScriptTrackOffsetTag : ScriptTag
{
    public ScriptTrackOffsetTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) :
        base(frontendObject,
            scriptProcessingContext)
    {
    }

    public uint Offset { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        Offset = br.ReadUInt32();
    }
}