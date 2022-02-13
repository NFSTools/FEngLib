namespace FEngLib.Objects;

public class Group : BaseObject
{
    public Group(ObjectData data) : base(data)
    {
        Type = ObjectType.Group;
    }

    public override void InitializeData()
    {
        Data = new ObjectData();
    }
}