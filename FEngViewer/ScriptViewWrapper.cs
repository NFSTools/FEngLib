using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FEngLib.Objects;
using FEngLib.Scripts;

namespace FEngViewer;

internal class ScriptViewWrapper
{
    private readonly HashList _scriptHashList;

    public ScriptViewWrapper(Script wrappedScript, IObject<ObjectData> wrappedObject, HashList scriptHashList)
    {
        _scriptHashList = scriptHashList;
        WrappedScript = wrappedScript;
        WrappedObject = wrappedObject;
    }

    protected Script WrappedScript { get; }
    protected IObject<ObjectData> WrappedObject { get; }

    [Category("Meta")]
    [Description("The hash of the script's UPPERCASE NAME.")]
    public uint Id => WrappedScript.Id;


    [Category("Properties")]
    [Description("The length of the script in milliseconds.")]
    public uint Length
    {
        get => WrappedScript.Length;
        set
        {
            if (value < TrackHelpers.GetAllTracks(WrappedScript).Max(t => t.Length))
            {
                throw new Exception("Script length must be greater than or equal to all track lengths.");
            }

            WrappedScript.Length = value;
        }
    }

    [Category("Properties")]
    [DisplayName("Chained Script")]
    [Description("The script that should play after this script ends.")]
    [Editor(typeof(ScriptChainEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(ScriptChainTypeConverter))]
    public uint? ChainedScriptId
    {
        get => WrappedScript.ChainedId;
        set => WrappedScript.ChainedId = value;
    }

    [Category("Properties")]
    [DisplayName("Loop")]
    [Description("If set to True, the script will restart after reaching its end. This is useful for repetitive animations like loading spinners.")]
    public bool LoopEnabled
    {
        get => (WrappedScript.Flags & 1) == 1;
        set
        {
            if (value)
                WrappedScript.Flags |= 1u;
            else
                WrappedScript.Flags &= ~1u;
        }
    }

    public IObject<ObjectData> GetWrappedObject()
    {
        return WrappedObject;
    }

    // This is an abomination of API design, but it works, so I'm keeping it!
    public string ResolveHash(uint hash)
    {
        return _scriptHashList.Lookup(hash);
    }
}

public class ScriptChainTypeConverter : TypeConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return destinationType == typeof(string);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        var scriptViewWrapper = (ScriptViewWrapper)context!.Instance;
        return value switch
        {
            uint hash => scriptViewWrapper.ResolveHash(hash),
            null => "(none)",
            _ => throw new Exception($"Invalid script chain value: {value}")
        };
    }
}

public class ScriptChainEditor : UITypeEditor
{
    class ScriptRecord
    {
        public uint? Hash { get; }
        public string Name { get; }

        public ScriptRecord(uint? hash, string name)
        {
            Hash = hash;
            Name = name;
        }
    }

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
        // drop down mode (we'll host a listbox in the drop down)
        return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService)) ??
                         throw new NullReferenceException("Could not get instance of IWindowsFormEditorService!");
        var scriptViewWrapper = (ScriptViewWrapper)context.Instance;
        // use a list box
        var lb = new ListBox();
        lb.SelectionMode = SelectionMode.One;
        lb.SelectedValueChanged += (_, _) => editorService.CloseDropDown();

        var scriptItems = new List<ScriptRecord> { new(null, "(none)") };
        scriptItems.AddRange(scriptViewWrapper.GetWrappedObject().GetScripts().Select(s => new ScriptRecord(s.Id, scriptViewWrapper.ResolveHash(s.Id))));

        foreach (var item in scriptItems)
        {
            var index = lb.Items.Add(item);
            if (item.Hash == (uint?)value)
                lb.SelectedIndex = index;
        }

        lb.DisplayMember = nameof(ScriptRecord.Name);

        // show this model stuff
        editorService.DropDownControl(lb);

        if (lb.SelectedItem is ScriptRecord sr)
            return sr.Hash;
        return value;
    }
}