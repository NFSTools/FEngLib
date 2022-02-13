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

        Targets = new MessageTargets
        {
            MsgId = br.ReadUInt32()
        };

        for (var i = 0; i < length / 4 - 1; i++)
        {
            Targets.Targets.Add(br.ReadUInt32());
        }
    }
}