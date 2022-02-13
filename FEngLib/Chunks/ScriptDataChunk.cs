using System;
using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Scripts.Tags;
using FEngLib.Tags;

namespace FEngLib.Chunks;

public class ScriptDataChunk : FrontendObjectChunk
{
    public ScriptDataChunk(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader)
    {
        var ctx = new ScriptProcessingContext(FrontendObject.CreateScript());
        var tagStream = new ScriptTagStream(reader,
            readerState.CurrentChunkBlock.Size, FrontendObject, ctx);

        while (tagStream.HasTag())
        {
            var tag = tagStream.NextTag();
            ProcessTag(ctx, tag);
        }

        return FrontendObject;
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.ScriptData;
    }

    private void ProcessTag(ScriptProcessingContext ctx, Tag tag)
    {
        switch (tag)
        {
            case ScriptHeaderTag scriptHeaderTag:
                ProcessScriptHeaderTag(ctx, scriptHeaderTag);
                break;
            case ScriptNameTag scriptNameTag:
                ctx.Script.Name = scriptNameTag.Name;
                ctx.Script.Id = scriptNameTag.NameHash;
                break;
            case ScriptChainTag scriptChainTag:
                ctx.Script.ChainedId = scriptChainTag.Id;
                break;
            case ScriptKeyTrackTag scriptKeyTrackTag:
                ProcessScriptKeyTrackTag(ctx, scriptKeyTrackTag);
                break;
            case ScriptTrackOffsetTag scriptTrackOffsetTag:
                ProcessScriptTrackOffsetTag(ctx, scriptTrackOffsetTag);
                break;
            case ScriptEventsTag scriptEventsTag:
                ctx.Script.Events.AddRange(scriptEventsTag.Events);
                break;
            case ScriptKeyNodeTag: // Side effects are OK for this one, it's just a delegate
                break;
            default:
                throw new NotImplementedException($"Unsupported tag type: {tag.GetType()}");
        }
    }

    private void ProcessScriptTrackOffsetTag(ScriptProcessingContext ctx, ScriptTrackOffsetTag scriptTrackOffsetTag)
    {
        var offset = scriptTrackOffsetTag.Offset;
        var currentTrack = ctx.CurrentTrack;
        var genericTracks = ctx.Script.GetTracks();

        if (offset <= 14)
            switch (offset)
            {
                case 0:
                    genericTracks.Color = (ColorTrack)currentTrack;
                    break;
                case 4:
                    genericTracks.Pivot = (Vector3Track)currentTrack;
                    break;
                case 7:
                    genericTracks.Position = (Vector3Track)currentTrack;
                    break;
                case 10:
                    genericTracks.Rotation = (QuaternionTrack)currentTrack;
                    break;
                case 14:
                    genericTracks.Size = (Vector3Track)currentTrack;
                    break;
                default:
                    throw new IndexOutOfRangeException($"Unsupported general track offset: {offset}");
            }
        else if (genericTracks is ImageScriptTracks imageScriptTracks)
            switch (offset)
            {
                case 17:
                    imageScriptTracks.UpperLeft = (Vector2Track)currentTrack;
                    break;
                case 19:
                    imageScriptTracks.LowerRight = (Vector2Track)currentTrack;
                    break;
                default:
                    switch (genericTracks)
                    {
                        case MultiImageScriptTracks multiImageScriptTracks:
                            switch (offset)
                            {
                                case 21:
                                    multiImageScriptTracks.TopLeft1 = (Vector2Track)currentTrack;
                                    break;
                                case 23:
                                    multiImageScriptTracks.TopLeft2 = (Vector2Track)currentTrack;
                                    break;
                                case 25:
                                    multiImageScriptTracks.TopLeft3 = (Vector2Track)currentTrack;
                                    break;
                                case 27:
                                    multiImageScriptTracks.BottomRight1 = (Vector2Track)currentTrack;
                                    break;
                                case 29:
                                    multiImageScriptTracks.BottomRight2 = (Vector2Track)currentTrack;
                                    break;
                                case 31:
                                    multiImageScriptTracks.BottomRight3 = (Vector2Track)currentTrack;
                                    break;
                                case 33:
                                    multiImageScriptTracks.PivotRotation = (Vector3Track)currentTrack;
                                    break;
                                default:
                                    throw new IndexOutOfRangeException(
                                        $"Unsupported MultiImage track offset: {offset}");
                            }

                            break;
                        case ColoredImageScriptTracks coloredImageScriptTracks:
                            switch (offset)
                            {
                                case 21:
                                    coloredImageScriptTracks.TopLeft = (ColorTrack)currentTrack;
                                    break;
                                case 25:
                                    coloredImageScriptTracks.TopRight = (ColorTrack)currentTrack;
                                    break;
                                case 29:
                                    coloredImageScriptTracks.BottomRight = (ColorTrack)currentTrack;
                                    break;
                                case 33:
                                    coloredImageScriptTracks.BottomLeft = (ColorTrack)currentTrack;
                                    break;
                                default:
                                    throw new IndexOutOfRangeException(
                                        $"Unsupported ColoredImage track offset: {offset}");
                            }

                            break;
                        default:
                            throw new IndexOutOfRangeException($"Unsupported Image track offset: {offset}");
                    }

                    break;
            }
        else
            throw new NotImplementedException(
                $"Track offset > 14 with an unexpected object type ({FrontendObject.Type}) ...");
    }

    private void ProcessScriptKeyTrackTag(ScriptProcessingContext ctx, ScriptKeyTrackTag scriptKeyTrackTag)
    {
        var paramType = (TrackParamType)scriptKeyTrackTag.ParamType;
        Track newTrack = paramType switch
        {
            TrackParamType.Vector2 => new Vector2Track(),
            TrackParamType.Vector3 => new Vector3Track(),
            TrackParamType.Quaternion => new QuaternionTrack(),
            TrackParamType.Color => new ColorTrack(),
            _ => throw new ArgumentOutOfRangeException($"Unsupported parameter type: {paramType}")
        };

        newTrack.Length = scriptKeyTrackTag.Length;
        newTrack.InterpType = (TrackInterpolationMethod)scriptKeyTrackTag.InterpType;
        newTrack.InterpAction = scriptKeyTrackTag.InterpAction;

        ctx.CurrentTrack = newTrack;
    }

    private void ProcessScriptHeaderTag(ScriptProcessingContext ctx,
        ScriptHeaderTag scriptHeaderTag)
    {
        ctx.Script.Id = scriptHeaderTag.Id;
        ctx.Script.Flags = scriptHeaderTag.Flags;
        ctx.Script.Length = scriptHeaderTag.Length;
    }
}