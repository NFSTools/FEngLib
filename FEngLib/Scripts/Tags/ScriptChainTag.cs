using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Scripts.Tags
{
    public class ScriptChainTag : ScriptTag
    {
        public uint Id { get; set; }

        public ScriptChainTag(IObject<ObjectData> frontendObject, Script script) : base(frontendObject, script)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            Id = br.ReadUInt32();
        }
    }
}