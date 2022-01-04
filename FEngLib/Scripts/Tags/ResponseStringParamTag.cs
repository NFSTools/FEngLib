using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Scripts.Tags;

public class ResponseStringParamTag : Tag
{
    public ResponseStringParamTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public string Param { get; set; }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package, ushort id,
        ushort length)
    {
        Param = new string(br.ReadChars(length)).Trim('\x00');
    }
}