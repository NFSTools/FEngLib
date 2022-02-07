namespace FEngLib;

public enum FrontendTagType : ushort
{
    // part of ObjD
    ObjectType = 0x744F, // Ot
    ObjectHash = 0x684F, // Oh
    ObjectReference = 0x504F, // OP
    ImageInfo = 0x6649, // If
    ObjectData = 0x4153, // SA
    ObjectParent = 0x4150, // PA
    StringBufferLength = 0x6253, // Sb
    StringBufferText = 0x7453, // St
    StringBufferFormatting = 0x6A53, // Sj
    StringBufferLeading = 0x6C53, // Sl
    StringBufferMaxWidth = 0x7753, // Sw
    StringBufferLabelHash = 0x4853, // SH
    MultiImageTexture1 = 0x314D, // M1
    MultiImageTexture2 = 0x324D, // M2
    MultiImageTexture3 = 0x334D, // M3
    MultiImageTextureFlags1 = 0x614D, // Ma
    MultiImageTextureFlags2 = 0x624D, // Mb
    MultiImageTextureFlags3 = 0x634D, // Mc
    ObjectName = 0x6E4F, // On
    StringBufferLabel = 0x4C53, // SL

    // part of Scrp
    ScriptHeader = 0x6853, // Sh
    ScriptChain = 0x6353, // Sc
    ScriptKeyTrack = 0x4946, // FI
    ScriptTrackOffset = 0x6F54, // To
    ScriptKeyNode = 0x644B, // Kd
    ScriptEvents = 0x5645, // EV
    ScriptName = 0x6E53, // Sn

    // part of Message related chunks (MsgR, PkgR, Targ)
    MessageResponseInfo = 0x694D, // Mi
    MessageResponseCount = 0x434D, // MC
    ResponseId = 0x6952, // Ri
    ResponseIntParam = 0x7552, // Ru
    ResponseStringParam = 0x7352, // Rs
    ResponseTarget = 0x7452, // Rt
    MessageTargetCount = 0x6354, // Tc
    MessageTargetList = 0x744D, // Mt
}