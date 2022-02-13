namespace FEngLib.Objects;

public class Movie : BaseObject
{
    //
    public Movie(ObjectData data) : base(data)
    {
        Type = ObjectType.Movie;
    }

    public override void InitializeData()
    {
        Data = new ObjectData();
    }
}