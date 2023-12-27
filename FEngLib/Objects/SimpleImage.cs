namespace FEngLib.Objects;

public class SimpleImage : BaseObject
{
    public SimpleImage(ObjectData data) : base(data)
    {
    }

    public override ObjectType GetObjectType()
    {
        return ObjectType.SimpleImage;
    }

    public override void InitializeData()
    {
        Data = new ObjectData();
    }
}