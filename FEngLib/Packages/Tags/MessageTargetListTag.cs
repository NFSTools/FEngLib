using System.Collections.Generic;
using System.IO;
using FEngLib.Chunks;
using FEngLib.Tags;

namespace FEngLib.Packages.Tags;

public class MessageTargetListTag : Tag
{
    public MessageTargets Targets { get; private set; }

    public override void Read(BinaryReader br, ushort id,
        ushort length)
    {
        if (length % 4 != 0)
        {
            throw new ChunkReadingException("Length not divisible by 4");
        }

        var msgId = br.ReadUInt32();
        length -= 4;
        var targetList = new List<uint>(length / 4);

        for (int i = 0; i < targetList.Capacity; i++)
        {
            targetList.Add(br.ReadUInt32());
        }

        Targets = new MessageTargets(msgId, targetList);
    }
}