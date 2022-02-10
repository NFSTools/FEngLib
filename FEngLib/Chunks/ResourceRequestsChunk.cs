using System;
using System.Collections.Generic;
using System.IO;
using FEngLib.Packages;

namespace FEngLib.Chunks;

public class ResourceRequestsChunk : FrontendChunk
{
    private uint[] _nameOffsets;

    public List<ResourceRequest> ResourceRequests { get; set; }

    public override void Read(Package package, FrontendChunkBlock chunkBlock,
        FrontendChunkReader chunkReader, BinaryReader reader)
    {
        if ((chunkBlock.Size - 4) % 0x18 != 0) throw new ChunkReadingException("Malformed RsRq chunk");

        var numRequests = reader.ReadInt32();
        ResourceRequests = new List<ResourceRequest>(numRequests);

        _nameOffsets = new uint[numRequests];

        for (var i = 0; i < numRequests; i++)
        {
            var resourceRequest = new ResourceRequest();
            resourceRequest.ID = reader.ReadUInt32();
            _nameOffsets[i] = reader.ReadUInt32();
            resourceRequest.Type = (ResourceType)reader.ReadUInt32();
            //resourceRequest.Flags = reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            //resourceRequest.UserParam = reader.ReadUInt32();

            ResourceRequests.Add(resourceRequest);
        }
    }

    public override FrontendChunkType GetChunkType()
    {
        throw new NotImplementedException();
    }

    public uint GetNameOffset(int i)
    {
        return _nameOffsets[i];
    }
}