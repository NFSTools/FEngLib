using System.Diagnostics;
using System.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ScriptKeyNodeTag : FrontendScriptTag
    {
        public ScriptKeyNodeTag(FrontendObject frontendObject, FrontendScript frontendScript) : base(frontendObject, frontendScript)
        {
        }

        public override void Read(BinaryReader br, ushort length)
        {
            FEKeyTrack track = FrontendScript.Tracks[^1];
            uint keyDataSize = track.ParamSize + 4u;

            if (length % keyDataSize != 0)
            {
                throw new ChunkReadingException($"{length} % {keyDataSize} = {length % keyDataSize}");
            }

            uint numKeys = length / keyDataSize;

            for (int i = 0; i < numKeys; i++)
            {
                FEKeyNode keyNode = new FEKeyNode
                {
                    Time = br.ReadInt32(),
                    Val =
                    {
                        [0] = br.ReadUInt32(),   
                        [1] = br.ReadUInt32(),   
                        [2] = br.ReadUInt32(),   
                        [3] = br.ReadUInt32(),   
                    }
                };

                if (i == 0)
                {
                    track.BaseKey = keyNode;
                }
                else
                {
                    track.DeltaKeys.Add(keyNode);
                }
            }
        }
    }
}