using System.IO;
using FEngLib.Packages;

namespace FEngLib.Chunks;

public class ResourcesContainerChunk : FrontendChunk
{
    public override void Read(Package package, FrontendChunkBlock chunkBlock,
        FrontendChunkReader chunkReader, BinaryReader reader)
    {
        ResourceNamesChunk resourceNamesChunk = null;

        foreach (var chunk in chunkReader.ReadMainChunks(chunkBlock.Size))
        {
            switch (chunk)
            {
                case ResourceNamesChunk rnc:
                    resourceNamesChunk = rnc;
                    break;
                case ResourceRequestsChunk _ when resourceNamesChunk == null:
                    throw new ChunkReadingException("RsRq came before RsNm?!");
                case ResourceRequestsChunk rrc:
                {
                    for (var index = 0; index < rrc.ResourceRequests.Count; index++)
                    {
                        var resourceRequest = rrc.ResourceRequests[index];
                        var nameOffset = rrc.GetNameOffset(index);
                        resourceRequest.Name = resourceNamesChunk.Names[nameOffset];
                    }

                    package.ResourceRequests.AddRange(rrc.ResourceRequests);

                    break;
                }
            }
        }
    }

    public override FrontendChunkType GetChunkType()
    {
        return FrontendChunkType.ResourcesContainer;
    }
}