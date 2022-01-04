using System;
using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Utils;

namespace FEngLib.Scripts.Tags;

public class ScriptKeyNodeTag : ScriptTag
{
    public ScriptKeyNodeTag(IObject<ObjectData> frontendObject, Script script) : base(frontendObject,
        script)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        var track = Script.Tracks[^1];
        var keyDataSize = track.ParamSize + 4u;

        if (length % keyDataSize != 0)
            throw new ChunkReadingException($"{length} % {keyDataSize} = {length % keyDataSize}");

        var numKeys = length / keyDataSize;

        for (var i = 0; i < numKeys; i++)
        {
            var keyNode = new TrackNode
            {
                Time = br.ReadInt32()
            };

            object nodeValue = track.ParamType switch
            {
                TrackParamType.Color => br.ReadColor(),
                TrackParamType.Vector2 => br.ReadVector2(),
                TrackParamType.Vector3 => br.ReadVector3(),
                TrackParamType.Quaternion => br.ReadQuaternion(),
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