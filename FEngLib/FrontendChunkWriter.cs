using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FEngLib.Packages;
using FEngLib.Utils;

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
    }

    private void WritePkHd(BinaryWriter writer)
    {
        WriteChunk(FrontendChunkType.PackageHeader,bw =>
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
        }, writer);
    }

    private void WriteTypS(BinaryWriter writer)
    {
        WriteChunk(FrontendChunkType.TypeList, bw =>
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
        }, writer);
    }

    private void WriteResCc(BinaryWriter writer)
    {
        WriteChunk(FrontendChunkType.ResourcesContainer, resbw =>
        {
            WriteChunk(FrontendChunkType.ResourceNames, nmbw =>
            {
                foreach (var resrq in Package.ResourceRequests)
                {
                    nmbw.Write(resrq.Name.ToCharArray());
                    nmbw.Write('\0');
                }
                nmbw.AlignWriter(4);
            }, resbw);
            
            WriteChunk(FrontendChunkType.ResourceRequests, rqbw =>
            {
                rqbw.Write(Package.ResourceRequests.Count);
                foreach (var resrq in Package.ResourceRequests)
                {
                    rqbw.Write(resrq.ID);
                    rqbw.Write(resrq.NameOffset);
                    rqbw.WriteEnum(resrq.Type);
                    rqbw.Write(resrq.Flags);
                    rqbw.Write(resrq.Handle);
                    rqbw.Write(resrq.UserParam);
                }
                rqbw.AlignWriter(4);
            }, resbw);
        }, writer);
    }

    private void WriteObjCc(BinaryWriter writer)
    {
        WriteChunk(FrontendChunkType.ObjectContainer, bw =>
        {
            var buttonCount = Package.ButtonCount;
            if (buttonCount != 0)
            {
                WriteChunk(FrontendChunkType.ButtonMapCount, butnBw =>
                {
                    butnBw.Write(buttonCount);
                }, bw);
            }
        }, writer);
    }

    private static void WriteChunk(FrontendChunkType id, Action<BinaryWriter> chunkWriter, BinaryWriter target)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        
        chunkWriter(bw);

        target.WriteEnum(id);
        target.Write((int) ms.Length);
        ms.WriteTo(target.BaseStream);
    }
}