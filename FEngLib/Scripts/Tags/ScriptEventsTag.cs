using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Scripts.Tags;

public class ScriptEventsTag : ScriptTag
{
    public ScriptEventsTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) : base(
        frontendObject, scriptProcessingContext)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        if (length % 0xC != 0)
        {
            throw new ChunkReadingException($"Tag length ({length}) should be divisible by 12.");
        }

        for (int i = 0; i < length / 0xC; i++)
        {
            Event @event = new Event
            {
                EventId = br.ReadUInt32(),
                Target = br.ReadUInt32(),
                Time = br.ReadUInt32()
            };

            Script.Events.Add(@event);
        }
    }
}