using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;

namespace FEngLib.Scripts.Tags;

public class ScriptKeyNodeTag : ScriptTag
{
    public ScriptKeyNodeTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) : base(
        frontendObject, scriptProcessingContext)
    {
    }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        var track = ScriptProcessingContext.CurrentTrack;
        var keyDataSize = track.GetParamSize() + 4u;

        if (length % keyDataSize != 0)
            throw new ChunkReadingException($"{length} % {keyDataSize} = {length % keyDataSize}");

        var numKeys = length / keyDataSize;
        track.ReadKeys(br, numKeys);
    }
}