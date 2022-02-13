using System.Collections.Generic;
using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;

namespace FEngLib.Scripts.Tags;

public class ScriptEventsTag : ScriptTag
{
    public ScriptEventsTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) : base(
        frontendObject, scriptProcessingContext)
    {
    }

    public List<Event> Events { get; set; }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        if (length % 0xC != 0)
        {
            throw new ChunkReadingException($"Tag length ({length}) should be divisible by 12.");
        }

        var numEvents = length / 0xC;
        Events = new List<Event>(numEvents);

        for (var i = 0; i < numEvents; i++)
        {
            Event @event = new Event
            {
                EventId = br.ReadUInt32(),
                Target = br.ReadUInt32(),
                Time = br.ReadUInt32()
            };

            Events.Add(@event);
        }
    }
}