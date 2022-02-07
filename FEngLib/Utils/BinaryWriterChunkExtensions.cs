using System;
using System.IO;

namespace FEngLib.Utils;

public static class BinaryWriterChunkExtensions
{
    internal static void WriteChunk(this BinaryWriter target, FrontendChunkType id, Action<BinaryWriter> chunkWriter)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        
        chunkWriter(bw);

        target.WriteEnum(id);
        target.Write((int) ms.Length);
        ms.WriteTo(target.BaseStream);
    }
    
    internal static void WriteTag(this BinaryWriter target, FrontendTagType id, Action<BinaryWriter> chunkWriter)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        
        chunkWriter(bw);

        target.WriteEnum(id);
        target.Write((short) ms.Length);
        ms.WriteTo(target.BaseStream);
    }
}