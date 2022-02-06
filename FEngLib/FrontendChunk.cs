using System;
using System.IO;
using FEngLib.Packages;

namespace FEngLib;

public abstract class FrontendChunk
{
    public abstract void Read(Package package, FrontendChunkBlock chunkBlock,
        FrontendChunkReader chunkReader, BinaryReader reader);

    public virtual void Write(Package package,
        FrontendChunkWriter chunkWriter, BinaryWriter writer)
    {
        throw new Exception();
    }

    public abstract FrontendChunkType GetChunkType();
}