using System.Diagnostics;
using System.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ScriptKeyTrackTag : FrontendScriptTag
    {
        public byte ParamType { get; set; }
        public byte ParamSize { get; set; }
        public byte InterpType { get; set; }
        public byte InterpAction { get; set; }
        public ScriptKeyTrackTag(FrontendObject frontendObject, FrontendScript frontendScript) : base(frontendObject, frontendScript)
        {
        }

        public override void Read(BinaryReader br, ushort length)
        {
            ParamType = br.ReadByte();
            ParamSize = br.ReadByte();
            InterpType = br.ReadByte();
            InterpAction = br.ReadByte();
            uint value = br.ReadUInt32();
            uint trackLength = value & 0xffffff;
            uint trackOffset = (value >> 24) & 0xff;
            Debug.WriteLine("PT {0} PS {1} IT {2} IA {3} LEN {4} OFF {5}", ParamType, ParamSize, InterpType,
                InterpAction, trackLength, trackOffset);

            FEKeyTrack keyTrack = new FEKeyTrack
            {
                ParamSize = ParamSize,
                ParamType = ParamType,
                InterpType = InterpType,
                InterpAction = InterpAction,
                Length = trackLength,
                Offset = trackOffset
            };

            FrontendScript.Tracks.Add(keyTrack);
        }
    }
}