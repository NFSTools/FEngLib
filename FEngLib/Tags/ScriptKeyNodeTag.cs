using System;
using System.IO;
using System.Numerics;
using FEngLib.Data;
using FEngLib.Object;
using FEngLib.Structures;

namespace FEngLib.Tags
{
    public class ScriptKeyNodeTag : FrontendScriptTag
    {
        public ScriptKeyNodeTag(IObject<ObjectData> frontendObject, FrontendScript frontendScript) : base(frontendObject,
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

                object nodeValue = track.ParamType switch
                {
                    FEParamType.PT_Color => br.ReadColor(),
                    FEParamType.PT_Vector2 => br.ReadVector2(),
                    FEParamType.PT_Vector3 => br.ReadVector3(),
                    FEParamType.PT_Quaternion => br.ReadQuaternion(),
                    _ => throw new Exception("unhandled ParamType: " + track.ParamType)
                };

                keyNode.Val = nodeValue;

                if (i == 0)
                    track.BaseKey = keyNode;
                else
                    track.DeltaKeys.AddLast(keyNode);
            }
        }
    }
}