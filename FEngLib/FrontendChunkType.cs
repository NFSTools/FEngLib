namespace FEngLib;

public enum FrontendChunkType : uint
{
    PackageHeader = 0x64486B50, // PkHd
    TypeList = 0x53707954, // TypS
    ResourcesContainer = 0xCC736552, // Res\xCC
    ResourceNames = 0x6D4E7352, // RsNm
    ResourceRequests = 0x71527352, // RsRq
    ObjectContainer = 0xCC6A624F, // Obj\xCC
    ButtonMapCount = 0x6E747542, // Butn
    FrontendObjectContainer = 0xEA624F46, // Obj\xEA
    ObjectData = 0x446A624F, // ObjD
    ScriptData = 0x70726353, // Scrp
    MessageResponses = 0x5267734D, // MsgR
    PackageResponses = 0x52676B50, // PkgR
    Targets = 0x67726154, // Targ
    TypeInfoList = 0xF4734C54, // TLs\xF4
    MessageDefinitions = 0x4E67734D, // MsgN
    EventData = 0x74614445 // EDat
}