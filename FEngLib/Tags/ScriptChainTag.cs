using System.IO;

namespace FEngLib.Tags
{
    public class ScriptChainTag : FrontendScriptTag
    {
        public uint Id { get; set; }

        public ScriptChainTag(FrontendObject frontendObject, FrontendScript frontendScript) : base(frontendObject, frontendScript)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort length)
        {
            Id = br.ReadUInt32();
        }
    }
}