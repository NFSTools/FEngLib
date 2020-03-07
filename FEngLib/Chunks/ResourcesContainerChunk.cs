using System.IO;

namespace FEngLib.Chunks
{
    public class ResourcesContainerChunk : FrontendChunk
    {
        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            ResourceNamesChunk resourceNamesChunk = null;

            foreach (var chunk in chunkReader.ReadMainChunks(chunkBlock.Size))
            {
                if (chunk is ResourceNamesChunk rnc)
                {
                    resourceNamesChunk = rnc;
                } else if (chunk is ResourceRequestsChunk rrc)
                {
                    if (resourceNamesChunk == null)
                    {
                        throw new ChunkReadingException("RsRq came before RsNm?!");
                    }

                    foreach (var resourceRequest in rrc.ResourceRequests)
                    {
                        resourceRequest.Name = resourceNamesChunk.Names[resourceRequest.NameOffset];

                        // TODO: add request to package?
                    }
                }
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            return FrontendChunkType.ResourcesContainer;
        }
    }
}