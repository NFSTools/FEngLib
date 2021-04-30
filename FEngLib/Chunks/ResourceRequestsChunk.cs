using System;
using System.Collections.Generic;
using System.IO;
using FEngLib.Data;

namespace FEngLib.Chunks
{
    public class ResourceRequestsChunk : FrontendChunk
    {
        public List<FEResourceRequest> ResourceRequests { get; set; }

        public override void Read(FrontendPackage package, FrontendChunkBlock chunkBlock,
            FrontendChunkReader chunkReader, BinaryReader reader)
        {
            if ((chunkBlock.Size - 4) % 0x18 != 0) throw new ChunkReadingException("Malformed RsRq chunk");

            var numRequests = reader.ReadInt32();
            ResourceRequests = new List<FEResourceRequest>(numRequests);

            for (var i = 0; i < numRequests; i++)
            {
                var resourceRequest = new FEResourceRequest();
                resourceRequest.ID = reader.ReadUInt32();
                resourceRequest.NameOffset = reader.ReadUInt32();
                resourceRequest.Type = (FEResourceType) reader.ReadUInt32();
                resourceRequest.Flags = reader.ReadUInt32();
                resourceRequest.Handle = reader.ReadUInt32();
                resourceRequest.UserParam = reader.ReadUInt32();

                ResourceRequests.Add(resourceRequest);
            }
        }

        public override FrontendChunkType GetChunkType()
        {
            throw new NotImplementedException();
        }
    }
}