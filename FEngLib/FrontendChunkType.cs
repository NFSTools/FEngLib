namespace FEngLib
{
    public enum FrontendChunkType : uint
    {
        PackageHeader = 0x64486B50,
        TypeList = 0x53707954,
        ResourcesContainer = 0xCC736552,
        ResourceNames = 0x6D4E7352,
        ResourceRequests = 0x71527352
    }
}