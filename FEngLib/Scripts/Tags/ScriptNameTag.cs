using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Utils;

namespace FEngLib.Scripts.Tags
{
    public class ScriptNameTag : ScriptTag
    {
        public ScriptNameTag(IObject<ObjectData> frontendObject, Script script) : base(frontendObject,
            script)
        {
        }

        public string Name { get; set; }
        public uint NameHash { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            Name = new string(br.ReadChars(length)).Trim('\x00');
            NameHash = Hashing.BinHash(Name.ToUpper());
        }
    }
}