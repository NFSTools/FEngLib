using System.Diagnostics;
using System.IO;
using FEngLib.Data;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ScriptEventsTag : FrontendScriptTag
    {
        public ScriptEventsTag(IObject<ObjectData> frontendObject, FrontendScript frontendScript) : base(frontendObject, frontendScript)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            if (length % 0xC != 0)
            {
                throw new ChunkReadingException($"Tag length ({length}) should be divisible by 12.");
            }

            for (int i = 0; i < length / 0xC; i++)
            {
                FEEvent scriptEvent = new FEEvent
                {
                    EventId = br.ReadUInt32(),
                    Target = br.ReadUInt32(),
                    Time = br.ReadUInt32()
                };

                FrontendScript.Events.Add(scriptEvent);
            }
        }
    }
}