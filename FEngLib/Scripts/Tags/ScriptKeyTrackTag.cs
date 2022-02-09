using System;
using System.Diagnostics;
using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Scripts.Tags;

public class ScriptKeyTrackTag : ScriptTag
{
    public ScriptKeyTrackTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) :
        base(frontendObject,
            scriptProcessingContext)
    {
    }

    public byte ParamType { get; set; }
    public byte ParamSize { get; set; }
    public byte InterpType { get; set; }
    public byte InterpAction { get; set; }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        ParamType = br.ReadByte();
        ParamSize = br.ReadByte();
        InterpType = br.ReadByte();
        InterpAction = br.ReadByte();
        var propBits = br.ReadUInt32();
        var trackLength = propBits & 0xffffff;
        var trackOffset = (propBits >> 24) & 0xff;

        Debug.Assert(trackOffset == 0, "trackOffset == 0");

        Track newTrack = (TrackParamType)ParamType switch
        {
            TrackParamType.Vector2 => new Vector2Track(),
            TrackParamType.Vector3 => new Vector3Track(),
            TrackParamType.Quaternion => new QuaternionTrack(),
            TrackParamType.Color => new ColorTrack(),
            _ => throw new ArgumentOutOfRangeException($"Unsupported parameter type: {ParamType}")
        };

        newTrack.Length = trackLength;
        newTrack.InterpType = (TrackInterpolationMethod)InterpType;
        newTrack.InterpAction = InterpAction;

        ScriptProcessingContext.CurrentTrack = newTrack;

        // var keyTrack = new Track
        // {
        //     ParamSize = ParamSize,
        //     ParamType = (TrackParamType) ParamType,
        //     InterpType = (TrackInterpolationMethod) InterpType,
        //     InterpAction = InterpAction,
        //     Length = trackLength,
        //     Offset = trackOffset
        // };
        //
        // Script.Tracks.Add(keyTrack);
    }
}