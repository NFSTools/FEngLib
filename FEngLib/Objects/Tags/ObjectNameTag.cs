using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Tags;
using FEngLib.Utils;

namespace FEngLib.Objects.Tags
{
    public class ObjectNameTag : Tag
    {
        public ObjectNameTag(IObject<ObjectData> frontendObject) : base(frontendObject)
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