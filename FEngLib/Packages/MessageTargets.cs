using System.Collections.Generic;

namespace FEngLib.Packages;

public class MessageTargets
{
    public MessageTargets()
    {
        Targets = new List<uint>();
    }

    public uint MsgId { get; set; }
    public List<uint> Targets { get; }
}