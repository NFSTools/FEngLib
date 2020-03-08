using System;

namespace FEngLib.Data
{
    [Flags]
    public enum FE_ObjectFlags : uint
    {
        FF_HideInEdit = 0x80000000,
        FF_AffectAllScripts = 0x40000000,
        FF_PerspectiveProjection = 0x20000000,
        FF_IsButton = 0x10000000,
        FF_ObjectLocked = 0x8000000,
        FF_IgnoreButton = 0x4000000,
        FF_Dirty = 0x2000000,
        FF_DirtyTransform = 0x1000000,
        FF_DirtyColor = 0x800000,
        FF_DirtyCode = 0x400000,
        FF_CodeSuppliedResource = 0x200000,
        FF_UsesLibraryObject = 0x100000,
        FF_DontNavigate = 0x80000,
        FF_SaveStaticTracks = 0x40000,
        FF_MouseObject = 0x20000,
        FF_Unknown = 0x8
    }
}