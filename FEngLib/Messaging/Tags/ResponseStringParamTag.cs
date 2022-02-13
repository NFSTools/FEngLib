using System.IO;
using FEngLib.Tags;

namespace FEngLib.Messaging.Tags;

public class ResponseStringParamTag : Tag
{
    public string Param { get; set; }

    public override void Read(BinaryReader br, ushort id,
        ushort length)
    {
        Param = new string(br.ReadChars(length)).Trim('\x00');
    }
}