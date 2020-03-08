using System.Collections.Generic;

namespace FEngLib.Data
{
    public class FEMessageTargetList
    {
        public uint MsgId { get; set; }
        public List<FrontendObject> Targets { get; set; }
    }
}