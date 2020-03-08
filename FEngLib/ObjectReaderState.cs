namespace FEngLib
{
    public class ObjectReaderState
    {
        public ObjectReaderState(FrontendChunkBlock currentChunkBlock, FrontendChunkReader chunkReader)
        {
            CurrentChunkBlock = currentChunkBlock;
            ChunkReader = chunkReader;
        }

        public FrontendChunkBlock CurrentChunkBlock { get; }
        public FrontendChunkReader ChunkReader { get; }
    }
}