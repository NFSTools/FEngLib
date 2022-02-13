using System.IO;

namespace FEngLib.Tags;

public abstract class TagStream
{
    private readonly long _endPosition;
    protected readonly BinaryReader Reader;

    protected TagStream(BinaryReader reader, long length)
    {
        Reader = reader;
        _endPosition = reader.BaseStream.Position + length;
    }

    public bool HasTag()
    {
        return Reader.BaseStream.Position < _endPosition;
    }

    public abstract Tag NextTag();
}