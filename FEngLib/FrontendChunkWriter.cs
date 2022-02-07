using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using FEngLib.Objects;
using FEngLib.Packages;
using FEngLib.Scripts;
using FEngLib.Structures;
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
        WritePkHd(writer);
        WriteTypS(writer);
        WriteResCc(writer);
        WriteObjCc(writer);
        WriteMsgChunks(writer);
    }

    private void WritePkHd(BinaryWriter writer)
    {
        writer.WriteChunk(PackageHeader,bw =>
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
            // TODO these are from UG2 UI_QRTrackSelect.fng.
            //  they should be the same, at the very least per game.
            // TODO: The type sizes present vary. how do they vary? by objects that are present?
            var sizes = new Dictionary<int, int>
            {
                { 5, 0x44 },
                { 2, 0x44 },
                //{ 1, 0x54 }
            };
            foreach (var (key, value) in sizes)
            {
                bw.Write(key);
                bw.Write(value);
            }
        });
    }

    private void WriteResCc(BinaryWriter writer)
    {
        writer.WriteChunk(ResourcesContainer, bw =>
        {
            bw.WriteChunk(ResourceNames, bw =>
            {
                foreach (var resrq in Package.ResourceRequests)
                {
                    bw.Write(resrq.Name.ToCharArray());
                    bw.Write('\0');
                }
                bw.AlignWriter(4);
            });
            
            bw.WriteChunk(ResourceRequests, bw =>
            {
                bw.Write(Package.ResourceRequests.Count);
                foreach (var resrq in Package.ResourceRequests)
                {
                    bw.Write(resrq.ID);
                    bw.Write(resrq.NameOffset);
                    bw.WriteEnum(resrq.Type);
                    bw.Write(resrq.Flags);
                    bw.Write(resrq.Handle);
                    bw.Write(resrq.UserParam);
                }
                bw.AlignWriter(4);
            });
        });
    }

    private void WriteObjCc(BinaryWriter writer)
    {
        writer.WriteChunk(ObjectContainer, bw =>
        {
            bw.WriteChunk(ButtonMapCount, bw =>
            {
                bw.Write(Package.ButtonCount);
            });

            foreach (var obj in Package.Objects)
            {
                bw.WriteChunk(FrontendObjectContainer, bw =>
                {
                    bw.WriteChunk(FrontendChunkType.ObjectData, bw =>
                    {
                        bw.WriteTag(FrontendTagType.ObjectType, bw => bw.WriteEnum(obj.Type));
                        if (obj.Name != null)
                        {
                            bw.WriteTag(ObjectName, bw =>
                            {
                                bw.Write(obj.Name.ToCharArray());
                                bw.Write('\0');
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

                        switch (obj.Type)
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
                                    bw.Write((short) 0);
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
                            case ObjectType.ColoredImage:
                            case ObjectType.SimpleImage:
                                // nothing special
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
                                Console.WriteLine($"!!!!!! idk whether/how to write a {obj.Type}'s specific tags!");
                                throw new NotImplementedException();
                        }

                        if (obj.Data != null)
                            bw.WriteTag(FrontendTagType.ObjectData, bw => obj.Data.Write(bw));
                    });

                    foreach (var script in obj.Scripts)
                    {
                        bw.WriteChunk(ScriptData, bw =>
                        {
                            if (script.Name != null)
                            {
                                bw.WriteTag(ScriptName, bw =>
                                {
                                    bw.Write(script.Name.ToCharArray());
                                    bw.Write('\0');
                                    bw.AlignWriter(4);
                                });
                            }
                            
                            bw.WriteTag(ScriptHeader, bw =>
                            {
                                bw.Write(script.Id);
                                bw.Write(script.Length);
                                bw.Write(script.Flags);
                                bw.Write(script.Tracks.Count);
                            });

                            if (script.ChainedId != 0xFFFFFFFF)
                            {
                                bw.WriteTag(ScriptChain, bw => bw.Write(script.ChainedId));
                            }

                            foreach (var track in script.Tracks)
                            {
                                bw.WriteTag(ScriptKeyTrack, bw =>
                                {
                                    bw.WriteEnum(track.ParamType);
                                    bw.Write(track.ParamSize);
                                    bw.WriteEnum(track.InterpType);
                                    bw.Write(track.InterpAction);
                                    bw.Write((track.Length & 0xffffff) | (track.Offset << 24));
                                });
                                
                                bw.WriteTag(ScriptTrackOffset, bw => bw.Write(track.Offset));
                                bw.WriteTag(ScriptKeyNode, bw =>
                                {
                                    void WriteNode(TrackNode node, BinaryWriter w)
                                    {
                                        w.Write(node.Time);
                                        switch (track.ParamType)
                                        {
                                            case TrackParamType.Vector2:
                                                w.Write((Vector2)node.Val);
                                                break;
                                            case TrackParamType.Vector3:
                                                w.Write((Vector3)node.Val);
                                                break;
                                            case TrackParamType.Quaternion:
                                                w.Write((Quaternion)node.Val);
                                                break;
                                            case TrackParamType.Color:
                                                w.Write((Color4)node.Val);
                                                break;
                                            default:
                                                throw new NotImplementedException("unhandled ParamType: " + track.ParamType);
                                        }
                                    }

                                    WriteNode(track.BaseKey, bw);
                                    foreach (var deltaKey in track.DeltaKeys)
                                    {
                                        WriteNode(deltaKey, bw);
                                    }
                                });
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
            bw.WriteTag(MessageTargetCount, bw => bw.Write(Package.MessageTargetLists.Count));
            foreach (var targetList in Package.MessageTargetLists)
            {
                bw.WriteTag(FrontendTagType.MessageTargetList, bw =>
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
            bw.WriteTag(ResponseId, bw => bw.Write(resp.Id));
            switch (resp.Param)
            {
                case string str:
                    bw.WriteTag(ResponseStringParam, bw =>
                    {
                        bw.Write(str.ToCharArray());
                        bw.Write('\0');
                        bw.AlignWriter(4);
                    });
                    break;
                case uint ui:
                    bw.WriteTag(ResponseIntParam, bw => bw.Write(ui));
                    break;
            }

            bw.WriteTag(ResponseTarget, bw => bw.Write(resp.Target));
        }
    }
}