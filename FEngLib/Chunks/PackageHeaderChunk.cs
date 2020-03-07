using System.IO;

namespace FEngLib.Chunks
{
    public class PackageHeaderChunk : FrontendChunk
    {
        public override void Read(FrontendPackage package, BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.PackageHeader;
        }
    }
}