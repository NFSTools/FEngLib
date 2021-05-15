using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ScriptNameTag : FrontendScriptTag
    {
        public ScriptNameTag(IObject<ObjectData> frontendObject, FrontendScript frontendScript) : base(frontendObject,
            frontendScript)
        {
        }

        public string Name { get; set; }
        public uint NameHash { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Name = new string(br.ReadChars(length)).Trim('\x00');
            NameHash = Hashing.BinHash(Name.ToUpper());
        }
    }
}