using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ScriptHeaderTag : FrontendScriptTag
    {
        public uint Id { get; set; }
        public uint Length { get; set; }
        public uint Flags { get; set; }
        public uint TrackCount { get; set; }

        public ScriptHeaderTag(IObject<ObjectData> frontendObject, FrontendScript frontendScript) : base(frontendObject, frontendScript)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Id = br.ReadUInt32();
            Length = br.ReadUInt32();
            Flags = br.ReadUInt32();
            TrackCount = br.ReadUInt32();
        }
    }
}