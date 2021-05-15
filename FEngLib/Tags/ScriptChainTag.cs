using System;
using System.IO;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class ScriptChainTag : FrontendScriptTag
    {
        public uint Id { get; set; }

        public ScriptChainTag(IObject<ObjectData> frontendObject, FrontendScript frontendScript) : base(frontendObject, frontendScript)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            Id = br.ReadUInt32();
        }
    }
}