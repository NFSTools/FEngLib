namespace FEngLib.Data
{
    public class FEResourceRequest
    {
        public uint ID { get; set; }
        public FEResourceType Type { get; set; }
        public string Name { get; set; }
        public uint NameOffset { get; set; }
        public uint Flags { get; set; }
        public uint Handle { get; set; }
        public uint UserParam { get; set; }
    }
}