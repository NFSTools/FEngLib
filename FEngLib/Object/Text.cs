using FEngLib.Data;

namespace FEngLib.Object
{
    public class Text : BaseObject
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public uint Hash { get; set; }
        public FEStringFormatting Formatting { get; set; }
        public int Leading { get; set; }
        public uint MaxWidth { get; set; }

        public Text(ObjectData data) : base(data)
        {
        }

        public override void InitializeData()
        {
            Data = new ObjectData();
        }
    }
}