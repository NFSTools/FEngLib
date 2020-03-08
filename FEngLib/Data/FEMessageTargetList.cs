using System.Collections.Generic;

namespace FEngLib.Data
{
    public class FEMessageTargetList
    {
        public uint MsgId { get; set; }
        public List<uint> Targets { get; set; }

        public FEMessageTargetList()
        {
            Targets = new List<uint>();
        }
    }
}