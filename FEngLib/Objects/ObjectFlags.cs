using System;

namespace FEngLib.Objects;

[Flags]
public enum ObjectFlags : uint
{
    Invisible = 0x1,
    PCOnly = 0x8,
    ConsoleOnly = 0x40,
    MouseObject = 0x20000,
    SaveStaticTracks = 0x40000,
    DontNavigate = 0x80000,
    UsesLibraryObject = 0x100000,
    CodeSuppliedResource = 0x200000,
    DirtyCode = 0x400000,
    DirtyColor = 0x800000,
    DirtyTransform = 0x1000000,
    Dirty = 0x2000000,
    IgnoreButton = 0x4000000,
    ObjectLocked = 0x8000000,
    HideInEdit = 0x80000000,
    IsButton = 0x10000000,
    PerspectiveProjection = 0x20000000,
    AffectAllScripts = 0x40000000,
}