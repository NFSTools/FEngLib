using System.IO;
using FEngLib.Tags;

namespace FEngLib.Messaging.Tags;

public class ResponseIdTag : Tag
{
    public uint Id { get; set; }

    public override void Read(BinaryReader br, ushort id,
        ushort length)
    {
        Id = br.ReadUInt32();
    }
}