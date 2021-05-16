using System.IO;
using FEngLib.Chunks;
using FEngLib.Objects;
using FEngLib.Tags;

namespace FEngLib.Packages.Tags
{
    public class MessageTargetListTag : Tag
    {
        public MessageTargetListTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package, ushort id,
            ushort length)
        {
            if (length % 4 != 0)
            {
                throw new ChunkReadingException("Length not divisible by 4");
            }

            MessageTargetList targetList = new MessageTargetList();
            targetList.MsgId = br.ReadUInt32();

            for (int i = 0; i < (length / 4) - 1; i++)
            {
                targetList.Targets.Add(br.ReadUInt32());
            }

            package.MessageTargetLists.Add(targetList);
        }
    }
}