using FEngLib.Objects;

namespace FEngLib.Object
{
    public class SimpleImage : BaseObject
    {
        public SimpleImage(ObjectData data) : base(data)
        {
        }

        public override void InitializeData()
        {
            Data = new ObjectData();
        }
    }
}