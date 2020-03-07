using System;
using System.Runtime.Serialization;

namespace FEngLib
{
    [Serializable]
    public class ChunkReadingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ChunkReadingException()
        {
        }

        public ChunkReadingException(string message) : base(message)
        {
        }

        public ChunkReadingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ChunkReadingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}