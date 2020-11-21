using System.IO;

namespace FEngLib.Tags
{
    public class ScriptUnknownTag : FrontendScriptTag
    {
        public ScriptUnknownTag(FrontendObject frontendObject, FrontendScript frontendScript) : base(frontendObject,
            frontendScript)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            var val = br.ReadUInt32();
        }
    }
}