namespace FEngLib
{
    public class FrontendChunkBlock
    {
        /// <summary>
        /// The type of the chunk
        /// </summary>
        public FrontendChunkType ChunkType { get; set; }

        /// <summary>
        /// The length of the chunk (excluding header)
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The offset of the chunk (before header)
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// The data offset of the chunk
        /// </summary>
        public long DataOffset => Offset + 8;

        /// <summary>
        /// The end offset of the chunk
        /// </summary>
        public long EndOffset => Offset + 8 + Size;
    }
}