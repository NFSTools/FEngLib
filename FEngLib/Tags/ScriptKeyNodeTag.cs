using System.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ScriptKeyNodeTag : FrontendScriptTag
    {
        public ScriptKeyNodeTag(FrontendObject frontendObject, FrontendScript frontendScript) : base(frontendObject,
            frontendScript)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort id,
            ushort length)
        {
            var track = FrontendScript.Tracks[^1];
            var keyDataSize = track.ParamSize + 4u;

            if (length % keyDataSize != 0)
                throw new ChunkReadingException($"{length} % {keyDataSize} = {length % keyDataSize}");

            var numKeys = length / keyDataSize;

            for (var i = 0; i < numKeys; i++)
            {
                var keyNode = new FEKeyNode
                {
                    Time = br.ReadInt32(),
                    Val = new uint[4]
                };

                if (keyDataSize >> 2 != 1)
                    for (var j = 0; j < (keyDataSize >> 2) - 1; j++)
                        keyNode.Val[j] = br.ReadUInt32();

                if (i == 0)
                    track.BaseKey = keyNode;
                else
                    track.DeltaKeys.Add(keyNode);
            }
        }
    }
}