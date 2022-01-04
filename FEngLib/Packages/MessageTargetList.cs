using System.Collections.Generic;

namespace FEngLib.Packages;

public class MessageTargetList
{
    public uint MsgId { get; set; }
    public List<uint> Targets { get; }

    public MessageTargetList()
    {
        Targets = new List<uint>();
    }
}