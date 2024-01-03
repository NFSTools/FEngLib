using System.Collections.Generic;

namespace FEngLib.Packages;

public class MessageTargets
{
    public MessageTargets()
    {
        Targets = new List<uint>();
    }

    public MessageTargets(uint msgId, List<uint> targets)
    {
        MsgId = msgId;
        Targets = targets;
    }

    public uint MsgId { get; set; }
    public List<uint> Targets { get; }
}