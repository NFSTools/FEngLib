using System.Collections.Generic;

namespace FEngLib.Scripts
{
    public class Script
    {
        public Script()
        {
            Tracks = new List<Track>();
            Events = new List<Event>();
        }

        public string Name { get; set; }
        public uint Id { get; set; }
        public uint ChainedId { get; set; } = 0xFFFFFFFF;
        public uint Length { get; set; }
        public uint Flags { get; set; }
        public List<Track> Tracks { get; }
        public List<Event> Events { get; }
    }
}