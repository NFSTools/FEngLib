using System;
using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib.Scripts.Tags;

public class ScriptTrackOffsetTag : ScriptTag
{
    public ScriptTrackOffsetTag(IObject<ObjectData> frontendObject, ScriptProcessingContext scriptProcessingContext) :
        base(frontendObject,
            scriptProcessingContext)
    {
    }

    public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
        ushort id,
        ushort length)
    {
        var offset = br.ReadUInt32();
        var currentTrack = ScriptProcessingContext.CurrentTrack;
        var genericTracks = Script.GetTracks();

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
}