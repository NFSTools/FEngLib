namespace FEngLib;

public enum FrontendTagType : ushort
{
    // part of ObjD
    ObjectType = 0x744F,
    ObjectHash = 0x684F,
    ObjectReference = 0x504F,
    ImageInfo = 0x6649,
    ObjectData = 0x4153,
    ObjectParent = 0x4150,
    StringBufferLength = 0x6253,
    StringBufferText = 0x7453,
    StringBufferFormatting = 0x6A53,
    StringBufferLeading = 0x6C53,
    StringBufferMaxWidth = 0x7753,
    StringBufferLabelHash = 0x4853,
    MultiImageTexture1 = 0x314D,
    MultiImageTexture2 = 0x324D,
    MultiImageTexture3 = 0x334D,
    MultiImageTextureFlags1 = 0x614D,
    MultiImageTextureFlags2 = 0x624D,
    MultiImageTextureFlags3 = 0x634D,
    ObjectName = 0x6E4F,
    StringBufferLabel = 0x4C53,

    // part of Scrp
    ScriptHeader = 0x6853,
    ScriptChain = 0x6353,
    ScriptKeyTrack = 0x4946,
    ScriptTrackOffset = 0x6F54,
    ScriptKeyNode = 0x644B,
    ScriptEvents = 0x5645,
    ScriptName = 0x6E53,

    // part of Message related chunks (MsgR, PkgR, Targ)
    MessageResponseInfo = 0x694D,
    MessageResponseCount = 0x434D,
    ResponseId = 0x6952,
    ResponseIntParam = 0x7552,
    ResponseStringParam = 0x7352,
    ResponseTarget = 0x7452,
    MessageTargetCount = 0x6354,
    MessageTargetList = 0x744D,
}