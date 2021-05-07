using System;
using System.IO;
using FEngLib.Data;
using FEngLib.Structures;

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
                    Time = br.ReadInt32()
                };

                object nodeValue;

                if (track.ParamType == FEParamType.PT_Color)
                {
                    var feColor = new FEColor();
                    feColor.Read(br);
                    nodeValue = feColor;
                }
                else if (track.ParamType == FEParamType.PT_Vector2)
                {
                    var feVec2 = new FEVector2();
                    feVec2.Read(br);
                    nodeValue = feVec2;
                }
                else if (track.ParamType == FEParamType.PT_Vector3)
                {
                    var feVec3 = new FEVector3();
                    feVec3.Read(br);
                    nodeValue = feVec3;
                }
                else if (track.ParamType == FEParamType.PT_Quaternion)
                {
                    var feQuaternion = new FEQuaternion();
                    feQuaternion.Read(br);
                    nodeValue = feQuaternion;
                }
                else
                {
                    throw new Exception("unhandled ParamType: " + track.ParamType);
                }

                keyNode.Val = nodeValue;

                if (i == 0)
                    track.BaseKey = keyNode;
                else
                    track.DeltaKeys.AddLast(keyNode);
            }
        }
    }
}