using System.IO;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib;

public abstract class FrontendObjectChunk
{
    protected IObject<ObjectData> FrontendObject { get; }

    protected FrontendObjectChunk(IObject<ObjectData> frontendObject)
    {
        FrontendObject = frontendObject;
    }

    public abstract IObject<ObjectData> Read(Package package, ObjectReaderState readerState, BinaryReader reader);
    public abstract FrontendChunkType GetChunkType();
}