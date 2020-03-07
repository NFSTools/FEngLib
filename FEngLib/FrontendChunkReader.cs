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
        public IEnumerable<FrontendChunk> ReadChunks()
        {
            return ReadChunks(Reader.BaseStream.Length);
        }

        public IEnumerable<FrontendChunk> ReadChunks(long length)
        {
            var endPos = Reader.BaseStream.Position + length;

            while (Reader.BaseStream.Position < endPos)
            {
                FrontendChunkBlock block = new FrontendChunkBlock
                {
                    Offset = Reader.BaseStream.Position,
                    ChunkType = (FrontendChunkType)Reader.ReadInt32(),
                    Size = Reader.ReadInt32()
                };

                FrontendChunk chunk = block.ChunkType switch
                {
                    FrontendChunkType.PackageHeader => new PackageHeaderChunk(),
                    _ => throw new ChunkReadingException($"Unknown chunk type: 0x{((int)block.ChunkType):X8}")
                };

                chunk.Read(Package, Reader);

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                {
                    throw new ChunkReadingException($"ERROR: Expected '{chunk.GetType()}' to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");
                }

                yield return chunk;
                //FrontendChunk frontendChunk =
            }
        }
    }
}