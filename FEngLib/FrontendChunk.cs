using System.IO;
using FEngLib.Packages;

namespace FEngLib;

public abstract class FrontendChunk
{
    public abstract void Read(Package package, FrontendChunkBlock chunkBlock,
        FrontendChunkReader chunkReader, BinaryReader reader);

    public abstract FrontendChunkType GetChunkType();
}