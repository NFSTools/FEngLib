using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Scripts.Tags;

public class ScriptTrackOffsetTag : ScriptTag
{
    public ScriptTrackOffsetTag(IObject<ObjectData> frontendObject, Script script) : base(frontendObject,
        script)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        Script.Tracks[^1].Offset = br.ReadUInt32();
    }
}