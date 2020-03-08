using System.Collections.Generic;

namespace FEngLib.Data
{
    public class FEKeyTrack
    {
        public byte ParamType { get; set; }
        public byte ParamSize { get; set; }
        public byte InterpType { get; set; }
        public byte InterpAction { get; set; }
        public uint Length { get; set; }
        public uint Offset { get; set; }
        public FEKeyNode BaseKey { get; set; }
        public List<FEKeyNode> DeltaKeys { get; set; }

        public FEKeyTrack()
        {
            DeltaKeys = new List<FEKeyNode>();
        }
    }
}