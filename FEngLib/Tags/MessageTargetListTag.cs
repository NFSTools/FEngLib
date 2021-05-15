using System.IO;
using FEngLib.Data;
using FEngLib.Object;

namespace FEngLib.Tags
{
    public class MessageTargetListTag : FrontendTag
    {
        public MessageTargetListTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, FrontendPackage package, ushort id,
            ushort length)
        {
            if (length % 4 != 0)
            {
                throw new ChunkReadingException("Length not divisible by 4");
            }

            FEMessageTargetList targetList = new FEMessageTargetList();
            targetList.MsgId = br.ReadUInt32();

            for (int i = 0; i < (length / 4) - 1; i++)
            {
                targetList.Targets.Add(br.ReadUInt32());
            }

            package.MessageTargetLists.Add(targetList);
        }
    }
}