using System.ComponentModel;
using System.Drawing.Design;
using System.Numerics;
using FEngLib.Objects;
using FEngLib.Structures;
using FEngViewer.TypeConverters;
using FEngViewer.UIEditors;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace FEngViewer;

public class ObjectViewWrapper
{
    private IObject<ObjectData> _obj;

    public ObjectViewWrapper(IObject<ObjectData> obj)
    {
        _obj = obj;
    }
    
    #region Basic object properties
    
    [Category("Meta")]
    [TypeConverter(typeof(HexTypeConverter))]
    public uint Guid
    {
        get => _obj.Guid;
        set => _obj.Guid = value;
    }
    
    #endregion

    #region Common object data
    [Category("Data")]
    [TypeConverter(typeof(Color4TypeConverter))]
    [Editor(typeof(Color4Editor), typeof(UITypeEditor))]
    public Color4 Color
    {
        get => _obj.Data.Color;
        set => _obj.Data.Color = value;
    }
        
    [Category("Data")]
    [TypeConverter(typeof(Vector3TypeConverter))]
    public Vector3 Pivot
    {
        get => _obj.Data.Pivot;
        set => _obj.Data.Pivot = value;
    }
        
    [Category("Data")]
    [TypeConverter(typeof(Vector3TypeConverter))]
    public Vector3 Position
    {
        get => _obj.Data.Position;
        set => _obj.Data.Position = value;
    }
        
    [Category("Data")]
    [TypeConverter(typeof(QuaternionTypeConverter))]
    public Quaternion Rotation
    {
        get => _obj.Data.Rotation;
        set => _obj.Data.Rotation = value;
    }
        
    [Category("Data")]
    [TypeConverter(typeof(Vector3TypeConverter))]
    public Vector3 Size
    {
        get => _obj.Data.Size;
        set => _obj.Data.Size = value;
    }
    #endregion

    #region Flags

    [Category("Flags")]
    public bool Invisible
    {
        get => (_obj.Flags & ObjectFlags.Invisible) != 0;
        set => _obj.SetFlag(ObjectFlags.Invisible, value);
    }

    public enum EPlatformSpecific
    {
        PcOnly,
        ConsoleOnly,
        None
    }

    [Category("Flags")]
    [Description("Whether this object should only be shown on a specific platform.")]
    public EPlatformSpecific PlatformSpecific
    {
        get {
            switch (_obj.Flags & (ObjectFlags.PCOnly | ObjectFlags.ConsoleOnly))
            {
                case ObjectFlags.PCOnly:
                    return EPlatformSpecific.PcOnly;
                case ObjectFlags.ConsoleOnly:
                    return EPlatformSpecific.ConsoleOnly;
                case ObjectFlags.PCOnly | ObjectFlags.ConsoleOnly:
                    // this is invalid state, so we do not allow setting it.
                    // however, technically this is possible, so at least display it if that's the case.
                    return EPlatformSpecific.PcOnly | EPlatformSpecific.ConsoleOnly;
                default:
                    return EPlatformSpecific.None;
            }
        }
        set
        {
            switch (value)
            {
                case EPlatformSpecific.PcOnly:
                    _obj.SetFlag(ObjectFlags.PCOnly, true);
                    _obj.SetFlag(ObjectFlags.ConsoleOnly, false);
                    break;
                case EPlatformSpecific.ConsoleOnly:
                    _obj.SetFlag(ObjectFlags.PCOnly, false);
                    _obj.SetFlag(ObjectFlags.ConsoleOnly, true);
                    break;
                case EPlatformSpecific.None:
                    _obj.SetFlag(ObjectFlags.PCOnly, false);
                    _obj.SetFlag(ObjectFlags.ConsoleOnly, false);
                    break;
            }
        }
    }
    
    [Category("Flags")]
    public bool MouseObject
    {
        get => (_obj.Flags & ObjectFlags.MouseObject) != 0;
        set => _obj.SetFlag(ObjectFlags.MouseObject, value);
    }
    
    [Category("Flags")]
    public bool SaveStaticTracks
    {
        get => (_obj.Flags & ObjectFlags.SaveStaticTracks) != 0;
        set => _obj.SetFlag(ObjectFlags.SaveStaticTracks, value);
    }
    
    [Category("Flags")]
    [Description("Button related. TODO")]
    public bool DontNavigate
    {
        get => (_obj.Flags & ObjectFlags.DontNavigate) != 0;
        set => _obj.SetFlag(ObjectFlags.DontNavigate, value);
    }
    
    [Category("Flags")]
    [Description("If set, this object's GUID is used to look up an object from another package to use as a 'template' of sorts. " +
                 "Not supported, setting this flag WILL make the game fail reading this package.")]
    public bool UsesLibraryObject
    {
        get => (_obj.Flags & ObjectFlags.UsesLibraryObject) != 0;
        set => _obj.SetFlag(ObjectFlags.UsesLibraryObject, value);
    }
    
    [Category("Flags")]
    public bool CodeSuppliedResource
    {
        get => (_obj.Flags & ObjectFlags.CodeSuppliedResource) != 0;
        set => _obj.SetFlag(ObjectFlags.CodeSuppliedResource, value);
    }

    [Category("Flags")]
    public bool ObjectLocked
    {
        get => (_obj.Flags & ObjectFlags.ObjectLocked) != 0;
        set => _obj.SetFlag(ObjectFlags.ObjectLocked, value);
    }
    
    [Category("Flags")]
    [Description("Whether this object should be displayed in the preview.")]
    public bool HideInEdit
    {
        get => (_obj.Flags & ObjectFlags.HideInEdit) != 0;
        set => _obj.SetFlag(ObjectFlags.HideInEdit, value);
    }
    
    [Category("Flags")]
    [Description("Whether this object is a button. Buttons have a special role with regard to input/interaction. " +
                 "TODO: write up details on buttons somewhere")]
    public bool IsButton
    {
        get => (_obj.Flags & ObjectFlags.IsButton) != 0;
        set => _obj.SetFlag(ObjectFlags.IsButton, value);
    }
    
    [Category("Flags")]
    [Description("Whether this button should be ignored. Can only be set with IsButton." +
                 "TODO: write up details on buttons somewhere")]
    public bool IgnoreButton
    {
        get => (_obj.Flags & ObjectFlags.IgnoreButton) != 0;
        set
        {
            if (value && IsButton) // only allow setting this flag on buttons
                _obj.SetFlag(ObjectFlags.IgnoreButton, true);
            else // always allow clearing it
                _obj.SetFlag(ObjectFlags.IgnoreButton, false);
            
        }
    }
    
    [Category("Flags")]
    public bool PerspectiveProjection
    {
        get => (_obj.Flags & ObjectFlags.PerspectiveProjection) != 0;
        set => _obj.SetFlag(ObjectFlags.PerspectiveProjection, value);
    }
    
    [Category("Flags")]
    public bool AffectAllScripts
    {
        get => (_obj.Flags & ObjectFlags.AffectAllScripts) != 0;
        set => _obj.SetFlag(ObjectFlags.AffectAllScripts, value);
    }
    
    #endregion
}