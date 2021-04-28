using System.Collections.Generic;
using FEngLib.Data;

namespace FEngLib
{
    public class FrontendScript
    {
        public FrontendScript()
        {
            Tracks = new List<FEKeyTrack>();
            Events = new List<FEEvent>();
        }

        public string Name { get; set; }
        public uint Id { get; set; }
        public uint Length { get; set; }
        public uint Flags { get; set; }
        public List<FEKeyTrack> Tracks { get; set; }
        public List<FEEvent> Events { get; set; }
    }
}