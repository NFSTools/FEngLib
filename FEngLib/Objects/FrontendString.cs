namespace FEngLib.Objects
{
    public class FrontendString : FrontendObject
    {
        public string Value { get; set; }

        public FrontendString()
        {
        }

        public FrontendString(FrontendObject original) : base(original)
        {
        }
        public FrontendString(FrontendString original) : this(original as FrontendObject)
        {
            Value = new string(original.Value.ToCharArray());
        }
    }
}