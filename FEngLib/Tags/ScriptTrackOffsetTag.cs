using System.IO;

namespace FEngLib.Tags
{
    public class ScriptTrackOffsetTag : FrontendScriptTag
    {
        public ScriptTrackOffsetTag(FrontendObject frontendObject, FrontendScript frontendScript) : base(frontendObject,
            frontendScript)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            FrontendScript.Tracks[^1].Offset = br.ReadUInt32();
        }
    }
}