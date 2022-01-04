using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Scripts.Tags;

public class ScriptHeaderTag : ScriptTag
{
    public uint Id { get; set; }
    public uint Length { get; set; }
    public uint Flags { get; set; }
    public uint TrackCount { get; set; }

    public ScriptHeaderTag(IObject<ObjectData> frontendObject, Script script) : base(frontendObject, script)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        Id = br.ReadUInt32();
        Length = br.ReadUInt32();
        Flags = br.ReadUInt32();
        TrackCount = br.ReadUInt32();
    }
}