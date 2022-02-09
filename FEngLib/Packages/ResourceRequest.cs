namespace FEngLib.Packages;

public enum ResourceType
{
    None = 0x0,
    Image = 0x1,
    Font = 0x2,
    Model = 0x3,
    Movie = 0x4,
    Effect = 0x5,
    AnimatedImage = 0x6,
    MultiImage = 0x7,
}

public class ResourceRequest
{
    public uint ID { get; set; }
    public ResourceType Type { get; set; }
    public string Name { get; set; }
    public uint Flags { get; set; }
    public uint UserParam { get; set; }
}