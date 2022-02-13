using System.IO;
using FEngLib.Objects;

namespace FEngLib.Scripts.Tags;

public class ScriptChainTag : ScriptTag
{
    public ScriptChainTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) : base(
        frontendObject, scriptProcessingContext)
    {
    }

    public uint Id { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        Id = br.ReadUInt32();
    }
}