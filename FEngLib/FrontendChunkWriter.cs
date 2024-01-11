using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FEngLib.Messaging;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Utils;
using static FEngLib.FrontendChunkType;
using static FEngLib.FrontendTagType;
using ObjectType = FEngLib.Objects.ObjectType;

// to prevent writing to the wrong stream in the nested WriteChunk/etc calls, we intentionally hide any outer writers.
// ReSharper disable VariableHidesOuterVariable

namespace FEngLib;

public class FrontendChunkWriter
{
    public FrontendChunkWriter(Package package)
    {
        Package = package;
    }

    public Package Package { get; }

    public void Write(BinaryWriter writer)
    {
        writer.WriteChunk(PackageStart, writer =>
        {
            WritePkHd(writer);
            WriteTypS(writer);
            WriteResCc(writer);
            WriteObjCc(writer);
            WriteMsgChunks(writer);
        });
    }

    private void WritePkHd(BinaryWriter writer)
    {
        writer.WriteChunk(PackageHeader, bw =>
        {
            bw.Write(0x20000);
            bw.Write(0);
            bw.Write(Package.ResourceRequests.Count);
            var rootObjCount = Package.Objects.Count(o => o.Parent == null);
            bw.Write(rootObjCount);
            bw.Write(Package.Name.Length + 1);
            bw.Write(Package.Filename.Length + 1);
            bw.Write(Package.Name.ToCharArray());
            bw.Write('\0');
            bw.Write(Package.Filename.ToCharArray());
            bw.Write('\0');
            bw.AlignWriter(4);
        });
    }

    private void WriteTypS(BinaryWriter writer)
    {
        writer.WriteChunk(TypeList, bw =>
        {
            var allSizes = new Dictionary<ObjectType, int>
            {
                { ObjectType.Image, 0x54 },
                { ObjectType.String, 0x44 },
                { ObjectType.Group, 0x44 },
                { ObjectType.Movie, 0x44 },
                { ObjectType.ColoredImage, 0x94 },
                { ObjectType.SimpleImage, 0x44 },
                { ObjectType.MultiImage, 0x90 },
            };

            var usedTypes = new List<ObjectType>();
            Package.Objects.ForEach(o =>
            {
                if (!usedTypes.Contains(o.GetObjectType()))
                    usedTypes.Add(o.GetObjectType());
            });

            foreach (var type in usedTypes)
            {
                bw.Write((int)type);
                bw.Write(allSizes[type]);
            }
        });
    }

    private void WriteResCc(BinaryWriter writer)
    {
        writer.WriteChunk(ResourcesContainer, bw =>
        {
            var nameOffsets = new uint[Package.ResourceRequests.Count];

            bw.WriteChunk(ResourceNames, bw =>
            {
                for (var index = 0; index < Package.ResourceRequests.Count; index++)
                {
                    var resourceRequest = Package.ResourceRequests[index];
                    nameOffsets[index] = (uint)bw.BaseStream.Position;
                    bw.WriteCString(resourceRequest.Name);
                }

                bw.AlignWriter(4);
            });

            bw.WriteChunk(ResourceRequests, bw =>
            {
                bw.Write(Package.ResourceRequests.Count);

                foreach (var (resourceRequest, nameOffset) in Package.ResourceRequests.Zip(nameOffsets))
                {
                    bw.Write(resourceRequest.ID);
                    bw.Write(nameOffset);
                    bw.WriteEnum(resourceRequest.Type);
                    bw.Write(0u);
                    bw.Write(0u);
                    bw.Write(0u);
                }

                bw.AlignWriter(4);
            });
        });
    }

    private void WriteObjCc(BinaryWriter writer)
    {
        writer.WriteChunk(ObjectContainer, bw =>
        {
            bw.WriteChunk(ButtonMapCount,
                bw => { bw.Write(Package.Objects.Count(o => o.Flags.HasFlag(ObjectFlags.IsButton))); });

            foreach (var obj in Package.Objects)
            {
                bw.WriteChunk(FrontendObjectContainer, bw =>
                {
                    bw.WriteChunk(FrontendChunkType.ObjectData, bw =>
                    {
                        bw.WriteTag(FrontendTagType.ObjectType, bw => bw.WriteEnum(obj.GetObjectType()));
                        if (obj.Name != null)
                        {
                            bw.WriteTag(ObjectName, bw =>
                            {
                                bw.WriteCString(obj.Name);
                                bw.AlignWriter(4);
                            });
                        }
                        else
                        {
                            bw.WriteTag(ObjectHash, bw => bw.Write(obj.NameHash));
                        }

                        bw.WriteTag(ObjectReference, bw =>
                        {
                            bw.Write(obj.Guid);
                            bw.Write(obj.NameHash);
                            bw.WriteEnum(obj.Flags);
                            if (obj.ResourceRequest != null)
                            {
                                bw.Write(Package.ResourceRequests.IndexOf(obj.ResourceRequest));
                            }
                            else
                            {
                                bw.Write(-1);
                            }
                        });

                        if (obj.Parent != null)
                        {
                            bw.WriteTag(ObjectParent, bw => bw.Write(obj.Parent.Guid));
                        }

                        switch (obj.GetObjectType())
                        {
                            case ObjectType.Image:
                                var img = (IImage<ImageData>)obj;
                                bw.WriteTag(ImageInfo, bw => bw.Write(img.ImageFlags));
                                break;
                            case ObjectType.String:
                                var str = (Text)obj;
                                // TODO we don't actually read/use this value yet (:
                                //  either find out if we can deduce this automatically, or make it editable...
                                bw.WriteTag(StringBufferLength, bw => bw.Write(str.BufferLength));
                                bw.WriteTag(StringBufferText, bw =>
                                {
                                    bw.Write(Encoding.Unicode.GetBytes(str.Value));
                                    bw.Write((short)0);
                                    bw.AlignWriter(4);
                                });
                                // TODO do we always write these tags? or are they left out for default values?
                                bw.WriteTag(StringBufferFormatting, bw => bw.WriteEnum(str.Formatting));
                                bw.WriteTag(StringBufferLeading, bw => bw.Write(str.Leading));
                                bw.WriteTag(StringBufferMaxWidth, bw => bw.Write(str.MaxWidth));
                                bw.WriteTag(StringBufferLabelHash, bw => bw.Write(str.Hash));
                                break;
                            case ObjectType.Group:
                            case ObjectType.Movie:
                            case ObjectType.SimpleImage:
                                // nothing special
                                break;
                            case ObjectType.ColoredImage:
                                var cImg = (ColoredImage)obj;
                                bw.WriteTag(ImageInfo, bw => bw.Write(cImg.ImageFlags));
                                break;
                            case ObjectType.MultiImage:
                                var mImg = (MultiImage)obj;
                                bw.WriteTag(ImageInfo, bw => bw.Write(mImg.ImageFlags));
                                bw.WriteTag(MultiImageTexture1, bw => bw.Write(mImg.Texture1));
                                bw.WriteTag(MultiImageTexture2, bw => bw.Write(mImg.Texture2));
                                bw.WriteTag(MultiImageTexture3, bw => bw.Write(mImg.Texture3));
                                bw.WriteTag(MultiImageTextureFlags1, bw => bw.Write(mImg.TextureFlags1));
                                bw.WriteTag(MultiImageTextureFlags2, bw => bw.Write(mImg.TextureFlags2));
                                bw.WriteTag(MultiImageTextureFlags3, bw => bw.Write(mImg.TextureFlags3));
                                break;
                            default:
                                Console.WriteLine(
                                    $"!!!!!! idk whether/how to write a {obj.GetObjectType()}'s specific tags!");
                                throw new NotImplementedException();
                        }

                        if (obj.Data != null)
                            bw.WriteTag(FrontendTagType.ObjectData, bw => obj.Data.Write(bw));
                    });

                    foreach (var script in obj.GetScripts())
                    {
                        bw.WriteChunk(ScriptData, bw =>
                        {
                            if (script.Name != null)
                            {
                                bw.WriteTag(ScriptName, bw =>
                                {
                                    bw.WriteCString(script.Name);
                                    bw.AlignWriter(4);
                                });
                            }

                            var tracks = GetScriptTracks(script);

                            bw.WriteTag(ScriptHeader, bw =>
                            {
                                bw.Write(script.Id);
                                bw.Write(script.Length);
                                bw.Write(script.Flags);
                                bw.Write(tracks.Count);
                            });

                            if (script.ChainedId is { } chainedId) bw.WriteTag(ScriptChain, bw => bw.Write(chainedId));

                            foreach (var (track, offset) in tracks)
                            {
                                bw.WriteTag(ScriptKeyTrack, bw =>
                                {
                                    bw.WriteEnum(track.GetParamType());
                                    bw.Write(track.GetParamSize());
                                    bw.WriteEnum(track.InterpType);
                                    bw.Write(track.InterpAction);
                                    // TODO inconsistent-ish. TrackOffset is actually different from this value.
                                    // UG2 at least does not use this offset thing at all...?
                                    bw.Write((track.Length & 0xffffff) /*| (track.Offset << 24)*/);
                                });

                                bw.WriteTag(ScriptTrackOffset, bw => bw.Write(offset));
                                bw.WriteTag(ScriptKeyNode, track.WriteKeys);
                            }

                            if (script.Events.Count != 0)
                            {
                                bw.WriteTag(ScriptEvents, bw =>
                                {
                                    foreach (var @event in script.Events)
                                    {
                                        bw.Write(@event.EventId);
                                        bw.Write(@event.Target);
                                        bw.Write(@event.Time);
                                    }
                                });
                            }
                        });
                    }

                    if (obj.MessageResponses.Count != 0)
                    {
                        bw.WriteChunk(MessageResponses, bw =>
                        {
                            foreach (var msgr in obj.MessageResponses)
                            {
                                WriteMessageResponse(msgr, bw);
                            }
                        });
                    }
                });
            }
        });
    }

    private List<(Track track, uint offset)> GetScriptTracks(Script script)
    {
        var scriptTracks = script.GetTracks();
        var tracks = new List<(Track track, uint offset)>();

        if (scriptTracks.Color is { } colorTrack)
            tracks.Add((colorTrack, 0));
        if (scriptTracks.Pivot is { } pivotTrack)
            tracks.Add((pivotTrack, 4));
        if (scriptTracks.Position is { } positionTrack)
            tracks.Add((positionTrack, 7));
        if (scriptTracks.Rotation is { } rotationTrack)
            tracks.Add((rotationTrack, 10));
        if (scriptTracks.Size is { } sizeTrack)
            tracks.Add((sizeTrack, 14));

        if (scriptTracks is ImageScriptTracks imageScriptTracks)
        {
            if (imageScriptTracks.UpperLeft is { } upperLeft)
                tracks.Add((upperLeft, 17));
            if (imageScriptTracks.LowerRight is { } lowerRight)
                tracks.Add((lowerRight, 19));
            if (imageScriptTracks is MultiImageScriptTracks multiImageScriptTracks)
            {
                if (multiImageScriptTracks.TopLeft1 is { } topLeft1)
                    tracks.Add((topLeft1, 21));
                if (multiImageScriptTracks.TopLeft2 is { } topLeft2)
                    tracks.Add((topLeft2, 23));
                if (multiImageScriptTracks.TopLeft3 is { } topLeft3)
                    tracks.Add((topLeft3, 25));
                if (multiImageScriptTracks.BottomRight1 is { } bottomRight1)
                    tracks.Add((bottomRight1, 27));
                if (multiImageScriptTracks.BottomRight2 is { } bottomRight2)
                    tracks.Add((bottomRight2, 29));
                if (multiImageScriptTracks.BottomRight3 is { } bottomRight3)
                    tracks.Add((bottomRight3, 31));
                if (multiImageScriptTracks.PivotRotation is { } pivotRotation)
                    tracks.Add((pivotRotation, 33));
            }
            else if (imageScriptTracks is ColoredImageScriptTracks coloredImageScriptTracks)
            {
                if (coloredImageScriptTracks.TopLeft is { } topLeft)
                    tracks.Add((topLeft, 21));
                if (coloredImageScriptTracks.TopRight is { } topRight)
                    tracks.Add((topRight, 25));
                if (coloredImageScriptTracks.BottomRight is { } bottomRight)
                    tracks.Add((bottomRight, 29));
                if (coloredImageScriptTracks.BottomLeft is { } bottomLeft)
                    tracks.Add((bottomLeft, 33));
            }
        }

        return tracks;
    }

    private void WriteMsgChunks(BinaryWriter writer)
    {
        if (Package.MessageResponses.Count != 0)
        {
            writer.WriteChunk(PackageResponses, bw =>
            {
                foreach (var response in Package.MessageResponses)
                {
                    WriteMessageResponse(response, bw);
                }
            });
        }

        writer.WriteChunk(Targets, bw =>
        {
            var targetLists = Package.Objects.SelectMany(o => o.MessageResponses.Select(mr => (o.Guid, mr.Id)))
                .GroupBy(e => e.Id)
                .Select(g => new MessageTargets(g.Key, g.Select(e => e.Guid).ToList()))
                .ToList();

            bw.WriteTag(MessageTargetCount, bw => bw.Write(targetLists.Count));
            foreach (var targetList in targetLists)
            {
                bw.WriteTag(MessageTargetList, bw =>
                {
                    bw.Write(targetList.MsgId);
                    foreach (var target in targetList.Targets)
                    {
                        bw.Write(target);
                    }
                });
            }
        });
    }

    private void WriteMessageResponse(MessageResponse msgr, BinaryWriter bw)
    {
        bw.WriteTag(MessageResponseInfo, bw => bw.Write(msgr.Id));
        bw.WriteTag(MessageResponseCount, bw => bw.Write(msgr.Responses.Count));
        foreach (var resp in msgr.Responses)
        {
            bw.WriteTag(ResponseId, bw => bw.Write(resp.GetId()));

            switch (resp)
            {
                case IIntegerCommand integerCommand:
                    bw.WriteTag(ResponseIntParam, bw => bw.Write(integerCommand.GetParameter()));
                    break;
                case IStringCommand stringCommand:
                    bw.WriteTag(ResponseStringParam, bw =>
                    {
                        bw.WriteCString(stringCommand.GetParameter());
                        bw.AlignWriter(4);
                    });
                    break;
                case NonParameterizedCommand:
                    bw.WriteTag(ResponseIntParam, bw => bw.Write(0u));
                    break;
            }

            bw.WriteTag(ResponseTarget, bw => bw.Write(resp is ITargetedCommand targetedCommand ? targetedCommand.Target : 0));
        }
    }
}