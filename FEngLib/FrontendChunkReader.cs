using System;
using System.Collections.Generic;
using System.IO;
using FEngLib.Chunks;

namespace FEngLib
{
    /// <summary>
    /// Reads binary data as <see cref="FrontendChunk"/> instances
    /// </summary>
    public class FrontendChunkReader
    {
        public FrontendChunkReader(FrontendPackage package, BinaryReader reader)
        {
            Package = package;
            Reader = reader;
        }

        public FrontendPackage Package { get; }
        public BinaryReader Reader { get; }

        /// <summary>
        /// Reads chunks
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FrontendChunk> ReadMainChunks()
        {
            return ReadMainChunks(Reader.BaseStream.Length);
        }

        public IEnumerable<FrontendChunk> ReadMainChunks(long length)
        {
            var endPos = Reader.BaseStream.Position + length;

            while (Reader.BaseStream.Position < endPos)
            {
                FrontendChunkBlock block = new FrontendChunkBlock
                {
                    Offset = Reader.BaseStream.Position,
                    ChunkType = (FrontendChunkType)Reader.ReadUInt32(),
                    Size = Reader.ReadInt32()
                };

                FrontendChunk chunk = block.ChunkType switch
                {
                    FrontendChunkType.PackageHeader => new PackageHeaderChunk(),
                    FrontendChunkType.TypeList => new TypeListChunk(),
                    FrontendChunkType.ResourcesContainer => new ResourcesContainerChunk(),
                    FrontendChunkType.ResourceNames => new ResourceNamesChunk(),
                    FrontendChunkType.ResourceRequests => new ResourceRequestsChunk(),
                    FrontendChunkType.ObjectContainer => new ObjectContainerChunk(),
                    _ => throw new ChunkReadingException($"Unknown chunk type: 0x{((int)block.ChunkType):X8}")
                };

                chunk.Read(Package, block, this, Reader);

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                {
                    throw new ChunkReadingException($"ERROR: Expected '{chunk.GetType()}' to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");
                }

                yield return chunk;
            }
        }

        public IEnumerable<FrontendObject> ReadObjects(long length)
        {
            var endPos = Reader.BaseStream.Position + length;

            while (Reader.BaseStream.Position < endPos)
            {
                FrontendChunkBlock block = new FrontendChunkBlock
                {
                    Offset = Reader.BaseStream.Position,
                    ChunkType = (FrontendChunkType)Reader.ReadUInt32(),
                    Size = Reader.ReadInt32()
                };

                FrontendObject frontendObject = new FrontendObject();
                FrontendObjectChunk chunk = block.ChunkType switch
                {
                    FrontendChunkType.ButtonMapCount => new ButtonMapCountChunk(frontendObject),
                    FrontendChunkType.FrontendObjectContainer => new FrontendObjectContainerChunk(frontendObject),
                    _ => throw new ChunkReadingException($"Unknown chunk type: 0x{((int)block.ChunkType):X8}")
                };

                chunk.Read(Package, block, this, Reader);

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                {
                    throw new ChunkReadingException($"ERROR: Expected '{chunk.GetType()}' to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");
                }

                yield return frontendObject;
            }
        }

        public FrontendObject ReadFrontendObjectChunks(FrontendObject frontendObject, long length)
        {
            var endPos = Reader.BaseStream.Position + length;

            while (Reader.BaseStream.Position < endPos)
            {
                FrontendChunkBlock block = new FrontendChunkBlock
                {
                    Offset = Reader.BaseStream.Position,
                    ChunkType = (FrontendChunkType)Reader.ReadUInt32(),
                    Size = Reader.ReadInt32()
                };

                FrontendObjectChunk chunk = block.ChunkType switch
                {
                    FrontendChunkType.ObjectData => new ObjectDataChunk(frontendObject),
                    FrontendChunkType.ScriptData => new ScriptDataChunk(frontendObject),
                    _ => throw new ChunkReadingException($"Unknown chunk type: 0x{((int)block.ChunkType):X8}")
                };

                frontendObject = chunk.Read(Package, block, this, Reader);

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                {
                    throw new ChunkReadingException($"ERROR: Expected '{chunk.GetType()}' to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");
                }
            }

            return frontendObject;
        }
    }
}