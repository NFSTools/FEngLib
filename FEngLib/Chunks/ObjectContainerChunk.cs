using System.IO;
using FEngLib.Packages;

namespace FEngLib.Chunks;

public class ObjectContainerChunk : FrontendChunk
{
    public override void Read(Package package, FrontendChunkBlock chunkBlock, FrontendChunkReader chunkReader,
        BinaryReader reader)
    {
        chunkReader.ReadObjects(chunkBlock.Size);
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.ObjectContainer;
    }
}