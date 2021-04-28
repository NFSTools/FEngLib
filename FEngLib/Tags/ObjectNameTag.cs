using System.IO;

namespace FEngLib.Tags
{
    public class ObjectNameTag : FrontendTag
    {
        public ObjectNameTag(FrontendObject frontendObject) : base(frontendObject)
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