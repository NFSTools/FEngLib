using FEngLib.Data;

namespace FEngLib.Objects
{
    public class FrontendString : FrontendObject
    {
        public FrontendString()
        {
        }

        public FrontendString(FrontendObject original) : base(original)
        {
        }

        public FrontendString(FrontendString original) : this(original as FrontendObject)
        {
            Value = new string(original.Value.ToCharArray());
            Hash = original.Hash;
            Formatting = original.Formatting;
            Leading = original.Leading;
            MaxWidth = original.MaxWidth;
        }

        public string Value { get; set; }
        public string Label { get; set; }

        public uint Hash { get; set; }

        public FEStringFormatting Formatting { get; set; }

        public int Leading { get; set; }

        public uint MaxWidth { get; set; }
    }
}