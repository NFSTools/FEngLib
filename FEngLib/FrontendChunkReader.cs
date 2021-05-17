using System;
using System.Collections.Generic;
using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngLib
{
    /// <summary>
    ///     Reads binary data as <see cref="FrontendChunk" /> instances
    /// </summary>
    public class FrontendChunkReader
    {
        public FrontendChunkReader(Package package, BinaryReader reader)
        {
            Package = package;
            Reader = reader;
        }

        public Package Package { get; }
        public BinaryReader Reader { get; }

        /// <summary>
        ///     Reads chunks
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
                var block = new FrontendChunkBlock
                {
                    Offset = Reader.BaseStream.Position,
                    ChunkType = (FrontendChunkType) Reader.ReadUInt32(),
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
                    FrontendChunkType.PackageResponses => new PackageResponsesChunk(),
                    FrontendChunkType.Targets => new PackageMessageTargetsChunk(),
                    FrontendChunkType.TypeInfoList => new TypeInfoListChunk(),
                    FrontendChunkType.MessageDefinitions => new MessageDefinitionsChunk(),
                    FrontendChunkType.EventData => new EventDataChunk(),
                    _ => throw new ChunkReadingException($"Unknown chunk type: 0x{(int) block.ChunkType:X8}")
                };

                chunk.Read(Package, block, this, Reader);

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                    throw new ChunkReadingException(
                        $"ERROR: Expected '{chunk.GetType()}' to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");

                yield return chunk;
            }
        }

        public void ReadObjects(long length)
        {
            var endPos = Reader.BaseStream.Position + length;

            while (Reader.BaseStream.Position < endPos)
            {
                var block = new FrontendChunkBlock
                {
                    Offset = Reader.BaseStream.Position,
                    ChunkType = (FrontendChunkType) Reader.ReadUInt32(),
                    Size = Reader.ReadInt32()
                };

                //FrontendObject frontendObject = new FrontendObject
                //{
                //    Package = Package
                //};
                //FrontendObjectChunk chunk = block.ChunkType switch
                //{
                //    FrontendChunkType.ButtonMapCount => new ButtonMapCountChunk(frontendObject),
                //    FrontendChunkType.FrontendObjectContainer => new FrontendObjectContainerChunk(frontendObject),
                //    _ => throw new ChunkReadingException($"Unknown chunk type: 0x{((int)block.ChunkType):X8}")
                //};

                switch (block.ChunkType)
                {
                    case FrontendChunkType.ButtonMapCount:
                        var buttonMapCountChunk = new ButtonMapCountChunk();
                        buttonMapCountChunk.Read(Package, block, this, Reader);
                        break;
                    case FrontendChunkType.FrontendObjectContainer:
                        var objectContainerChunk = new FrontendObjectContainerChunk(null);
                        var frontendObject = objectContainerChunk.Read(Package, new ObjectReaderState(block, this), Reader);
                        Package.Objects.Add(frontendObject);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                    throw new ChunkReadingException(
                        $"ERROR: Expected to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");
            }
        }

        /*
         *                 ObjectReaderState readerState = new ObjectReaderState(block, this);
                frontendObject = chunk.Read(Package, readerState, Reader);

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                {
                    throw new ChunkReadingException($"ERROR: Expected '{chunk.GetType()}' to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");
                }

                Package.Objects.Add(frontendObject);
         */

        public IObject<ObjectData> ReadFrontendObjectChunks(IObject<ObjectData> frontendObject, long length)
        {
            var endPos = Reader.BaseStream.Position + length;

            while (Reader.BaseStream.Position < endPos)
            {
                var block = new FrontendChunkBlock
                {
                    Offset = Reader.BaseStream.Position,
                    ChunkType = (FrontendChunkType) Reader.ReadUInt32(),
                    Size = Reader.ReadInt32()
                };

                FrontendObjectChunk chunk = block.ChunkType switch
                {
                    FrontendChunkType.ObjectData => new ObjectDataChunk(frontendObject),
                    FrontendChunkType.ScriptData => new ScriptDataChunk(frontendObject),
                    FrontendChunkType.MessageResponses => new MessageResponsesDataChunk(frontendObject),
                    _ => throw new ChunkReadingException($"Unknown chunk type: 0x{(int) block.ChunkType:X8}")
                };

                var readerState = new ObjectReaderState(block, this);
                frontendObject = chunk.Read(Package, readerState, Reader);

                if (Reader.BaseStream.Position - block.DataOffset != block.Size)
                    throw new ChunkReadingException(
                        $"ERROR: Expected '{chunk.GetType()}' to read {block.Size} bytes, but it read {Reader.BaseStream.Position - block.DataOffset} bytes instead.");
            }

            return frontendObject;
        }
    }
}