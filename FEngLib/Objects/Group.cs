namespace FEngLib.Objects;

public class Group : BaseObject
{
    public Group(ObjectData data) : base(data)
    {
    }

    public override void InitializeData()
    {
        Data = new ObjectData();
    }
}