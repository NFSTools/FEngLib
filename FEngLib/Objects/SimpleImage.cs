namespace FEngLib.Objects;

public class SimpleImage : BaseObject
{
    public SimpleImage(ObjectData data) : base(data)
    {
        Type = ObjectType.SimpleImage;
    }

    public override void InitializeData()
    {
        Data = new ObjectData();
    }
}