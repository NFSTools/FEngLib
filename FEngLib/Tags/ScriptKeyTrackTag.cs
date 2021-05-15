using System.IO;
using FEngLib.Data;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ScriptKeyTrackTag : FrontendScriptTag
    {
        public ScriptKeyTrackTag(IObject<ObjectData> frontendObject, FrontendScript frontendScript) : base(frontendObject,
            frontendScript)
        {
        }

        public byte ParamType { get; set; }
        public byte ParamSize { get; set; }
        public byte InterpType { get; set; }
        public byte InterpAction { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
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

            var keyTrack = new FEKeyTrack
            {
                ParamSize = ParamSize,
                ParamType = (FEParamType) ParamType,
                InterpType = (FEInterpMethod) InterpType,
                InterpAction = InterpAction,
                Length = trackLength,
                Offset = trackOffset
            };

            FrontendScript.Tracks.Add(keyTrack);
        }
    }
}