using System.IO;
using FEngLib.Tags;

namespace FEngLib.Messaging.Tags;

public class ResponseIntParamTag : Tag
{
    public uint Param { get; set; }

    public override void Read(BinaryReader br, ushort id,
        ushort length)
    {
        Param = br.ReadUInt32();
    }
}