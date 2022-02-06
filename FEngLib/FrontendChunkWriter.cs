using System.IO;
using FEngLib.Chunks;
using FEngLib.Packages;

namespace FEngLib;

public class FrontendChunkWriter
{
    public FrontendChunkWriter(Package package, BinaryWriter writer)
    {
        Package = package;
        Writer = writer;
    }
    
    public Package Package { get; }
    public BinaryWriter Writer { get; }

    public void Write()
    {
        Package.Write(Writer);
    }

    private void WritePackageHeader()
    {
        
    }
}