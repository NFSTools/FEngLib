using System.IO;
using FEngLib.Object;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Scripts.Tags
{
    public class ScriptKeyTrackTag : ScriptTag
    {
        public ScriptKeyTrackTag(IObject<ObjectData> frontendObject, Script script) : base(frontendObject,
            script)
        {
        }

        public byte ParamType { get; set; }
        public byte ParamSize { get; set; }
        public byte InterpType { get; set; }
        public byte InterpAction { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            ParamType = br.ReadByte();
            ParamSize = br.ReadByte();
            InterpType = br.ReadByte();
            InterpAction = br.ReadByte();
            var value = br.ReadUInt32();
            var trackLength = value & 0xffffff;
            var trackOffset = (value >> 24) & 0xff;

            var keyTrack = new Track
            {
                ParamSize = ParamSize,
                ParamType = (TrackParamType) ParamType,
                InterpType = (TrackInterpolationMethod) InterpType,
                InterpAction = InterpAction,
                Length = trackLength,
                Offset = trackOffset
            };

            Script.Tracks.Add(keyTrack);
        }
    }
}