using System.Diagnostics;
using System.IO;

namespace FEngLib.Tags
{
    public class MessageResponseCountTag : FrontendTag
    {
        public MessageResponseCountTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package,
            ushort length)
        {
            uint value = br.ReadUInt32();

            if (chunkBlock.ChunkType == FrontendChunkType.MessageResponses)
            {
                Debug.WriteLine("MsgResponse {0:X8} has {1} responses", FrontendObject.MessageResponses[^1].Id, value);
            }
            else
            {

            }
        }
    }
}