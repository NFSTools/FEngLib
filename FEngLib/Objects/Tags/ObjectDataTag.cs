using System.IO;

namespace FEngLib.Objects.Tags;

public class ObjectDataTag : ObjectTag
{
    public ObjectDataTag(IObject<ObjectData> frontendObject) : base(frontendObject)
    {
    }

    public override void Read(BinaryReader br,
        ushort id,
        ushort length)
    {
        FrontendObject.InitializeData();
        FrontendObject.Data.Read(br);
    }
}