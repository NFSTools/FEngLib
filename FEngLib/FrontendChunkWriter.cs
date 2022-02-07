﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FEngLib.Objects;
using FEngLib.Packages;
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
        // PkgR
        // Targ
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
            // TODO: check if these are always the same in every fng, or if the type sizes present vary.
            var sizes = new Dictionary<int, int>
            {
                { 5, 0x44 },
                { 2, 0x44 },
                { 1, 0x54 }
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
                        bw.WriteTag(ObjectHash, bw => bw.Write(obj.NameHash));
                        bw.WriteTag(ObjectReference, bw =>
                        {
                            bw.Write(obj.Guid);
                            bw.Write(obj.NameHash);
                            bw.WriteEnum(obj.Flags);
                            if (obj.ResourceRequest != null)
                            {
                                // TODO get the index back from the package
                                // or, make them up anew?
                                bw.Write(0);
                            }
                            else
                            {
                                bw.Write(-1);
                            }
                        });

                        if (obj.Parent != null)
                        {
                            bw.WriteTag(ObjectParent, bw => bw.Write(obj.Parent.NameHash));
                        }

                        switch (obj.Type)
                        {
                            case ObjectType.Image:
                                var img = (IImage<ImageData>)obj;
                                bw.WriteTag(ImageInfo, bw => bw.Write(img.ImageFlags));
                                break;
                            case ObjectType.String:
                                // TODO incomplete, all of this
                                var str = (Text)obj;
                                // TODO we don't actually read/use this value yet (:
                                bw.WriteTag(StringBufferLength, bw => bw.Write(0));
                                bw.WriteTag(StringBufferText, bw =>
                                {
                                    bw.Write(Encoding.Unicode.GetBytes(str.Value));
                                    bw.Write((short) 0);
                                    bw.AlignWriter(4);
                                });
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
                        
                        });
                    }

                    foreach (var msgr in obj.MessageResponses)
                    {
                        bw.WriteChunk(MessageResponses, bw =>
                        {
                        
                        });
                    }
                });
            }
        });
    }
}