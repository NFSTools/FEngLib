using System.IO;

namespace FEngLib.Tags;

public abstract class Tag
{
    public abstract void Read(BinaryReader br, ushort id, ushort length);
}