namespace FEngLib.Objects
{
    public class Movie : BaseObject
    {
        //
        public Movie(ObjectData data) : base(data)
        {
        }

        public override void InitializeData()
        {
            Data = new ObjectData();
        }
    }
}