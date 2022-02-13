using System.IO;
using FEngLib.Tags;

namespace FEngLib.Messaging.Tags;

public class MessageResponseCountTag : Tag
{
    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        br.ReadUInt32();
    }
}