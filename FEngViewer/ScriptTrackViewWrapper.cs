using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngViewer.TypeConverters;

namespace FEngViewer;

public abstract class TrackKeyHolder<TTrackValue> where TTrackValue : struct
{
    [Editor(typeof(TrackKeyEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(TrackKeyConverter))]
    public abstract TTrackValue KeyValue { get; set; }
}

public abstract class ScriptTrackViewWrapper<TTrack, TTrackValue> : TrackKeyHolder<TTrackValue>
    where TTrack : Track<TTrackValue> where TTrackValue : struct
{
    protected TTrack Track { get; }
    protected Script Script { get; }

    public ScriptTrackViewWrapper(TTrack track, Script script)
    {
        Track = track;
        Script = script;
    }

    [Category("Meta")]
    [DisplayName("Parameter Type")]
    [Description("The type of data stored in the track.")]
    public TrackParamType ParamType => Track.GetParamType();

    [Category("Properties")]
    [Description("The length of the track in milliseconds.")]
    public uint Length
    {
        get => Track.Length;
        set
        {
            if (value > Script.Length)
                throw new Exception("Track length cannot exceed script length.");
            Track.Length = value;
        }
    }

    [Category("Properties")]
    [DisplayName("Base Value")]
    [Description("The starting value for the track.")]
    public override TTrackValue KeyValue
    {
        get => GetBaseKey();
        set => SetBaseKey(value);
    }

    public Track GetTrack()
    {
        return Track;
    }

    public Script GetScript()
    {
        return Script;
    }

    protected virtual TTrackValue GetBaseKey()
    {
        return Track.BaseKey;
    }

    protected virtual void SetBaseKey(TTrackValue baseKey)
    {
        foreach (var deltaKey in Track.DeltaKeys)
        {
            var absoluteKey = TrackHelpers.AddKeys(Track.BaseKey, deltaKey.Val);
            var adjustedKey = (TTrackValue)TrackHelpers.SubtractKeys(absoluteKey, baseKey);
            deltaKey.Val = adjustedKey;
        }

        // TODO: validation?
        Track.BaseKey = baseKey;
    }
}

public class TrackKeyConverter : TypeConverter
{
    //
}

public class ColorTrackViewWrapper : ScriptTrackViewWrapper<ColorTrack, Color4>
{
    [TypeConverter(typeof(Color4TypeConverter))]
    public override Color4 KeyValue
    {
        get => GetBaseKey();
        set => SetBaseKey(value);
    }

    public ColorTrackViewWrapper(ColorTrack track, Script script) : base(track, script)
    {
    }
}

public class Vector2TrackViewWrapper : ScriptTrackViewWrapper<Vector2Track, Vector2>
{
    [TypeConverter(typeof(Vector2TypeConverter))]
    public override Vector2 KeyValue
    {
        get => GetBaseKey();
        set => SetBaseKey(value);
    }

    public Vector2TrackViewWrapper(Vector2Track track, Script script) : base(track, script)
    {
    }
}

public class Vector3TrackViewWrapper : ScriptTrackViewWrapper<Vector3Track, Vector3>
{
    [TypeConverter(typeof(Vector3TypeConverter))]
    public override Vector3 KeyValue
    {
        get => GetBaseKey();
        set => SetBaseKey(value);
    }

    public Vector3TrackViewWrapper(Vector3Track track, Script script) : base(track, script)
    {
    }
}

public abstract class TrackDeltaKeyViewWrapper<TTrack, TTrackValue> : TrackKeyHolder<TTrackValue>
    where TTrackValue : struct where TTrack : Track<TTrackValue>
{
    protected TrackDeltaKeyViewWrapper(TTrack track, TrackNode<TTrackValue> trackNode)
    {
        Track = track;
        TrackNode = trackNode;
    }

    protected TTrack Track { get; }
    protected TrackNode<TTrackValue> TrackNode { get; }

    [Category("Properties")]
    [DisplayName("Time")]
    [Description("The time in milliseconds, relative to the start of the script, that the key begins to take full effect.")]
    public uint Time
    {
        get => (uint)TrackNode.Time;
        set
        {
            if (value > Track.Length)
                throw new Exception("Key must lie before the end of the track.");
            if (value != Time)
            {
                // TODO: all of this is a disgusting hack, there should really be a proper API
                if (Track.DeltaKeys.Any(dk => dk.Time == value))
                    throw new Exception("There is already a key at this time.");
                TrackNode.Time = (int)value;
                Track.DeltaKeys = new LinkedList<TrackNode<TTrackValue>>(Track.DeltaKeys.OrderBy(dk => dk.Time));
            }
        }
    }

    [Category("Properties")]
    [DisplayName("Value")]
    [Description("The actual (non-delta) value of the property controlled by the track at the given point in time.")]
    public override TTrackValue KeyValue
    {
        get => GetKeyValue();
        set => SetKeyValue(value);
    }

    protected virtual TTrackValue GetKeyValue()
    {
        return (TTrackValue)TrackHelpers.AddKeys(Track.BaseKey, TrackNode.Val);
    }

    protected virtual void SetKeyValue(TTrackValue value)
    {
        TrackNode.Val = (TTrackValue)TrackHelpers.SubtractKeys(value, Track.BaseKey);
    }
}

public class ColorDeltaKeyViewWrapper : TrackDeltaKeyViewWrapper<ColorTrack, Color4>
{
    public ColorDeltaKeyViewWrapper(ColorTrack track, TrackNode<Color4> trackNode) : base(track, trackNode)
    {
    }
}

public class Vector2DeltaKeyViewWrapper : TrackDeltaKeyViewWrapper<Vector2Track, Vector2>
{
    [TypeConverter(typeof(Vector2TypeConverter))]
    public override Vector2 KeyValue
    {
        get => GetKeyValue();
        set => SetKeyValue(value);
    }

    public Vector2DeltaKeyViewWrapper(Vector2Track track, TrackNode<Vector2> trackNode) : base(track, trackNode)
    {
    }
}

public class Vector3DeltaKeyViewWrapper : TrackDeltaKeyViewWrapper<Vector3Track, Vector3>
{
    [TypeConverter(typeof(Vector3TypeConverter))]
    public override Vector3 KeyValue
    {
        get => GetKeyValue();
        set => SetKeyValue(value);
    }

    public Vector3DeltaKeyViewWrapper(Vector3Track track, TrackNode<Vector3> trackNode) : base(track, trackNode)
    {
    }
}

public class TrackKeyEditor : UITypeEditor
{
    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        if (context.Instance is TrackKeyHolder<Color4> colorHolder)
        {
            var fngColor = colorHolder.KeyValue;
            if (IsValidColor(fngColor))
            {
                ColorDialog colorDialog = new()
                {
                    Color = Color.FromArgb(fngColor.Alpha, fngColor.Red, fngColor.Green, fngColor.Blue)
                };

                if (colorDialog.ShowDialog() != DialogResult.OK) return fngColor;

                var chosenColor = colorDialog.Color;
                return new Color4(chosenColor.B, chosenColor.G, chosenColor.R, chosenColor.A);
            }
        }

        return base.EditValue(context, provider, value);
    }

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
        return context.Instance is TrackKeyHolder<Color4> colorHolder && IsValidColor(colorHolder.KeyValue)
            ? UITypeEditorEditStyle.Modal
            : base.GetEditStyle(context);
    }

    public override bool GetPaintValueSupported(ITypeDescriptorContext context)
    {
        return context.Instance is TrackKeyHolder<Color4> colorHolder
               && IsValidColor(colorHolder.KeyValue);
    }

    public override void PaintValue(PaintValueEventArgs e)
    {
        if (e.Value is Color4 color)
        {
            using SolidBrush b = new(Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue));
            e.Graphics.FillRectangle(b, e.Bounds);
        }
    }

    private static bool IsValidColor(Color4 color)
    {
        return color is
        {
            Alpha: >= 0 and <= 255,
            Red: >= 0 and <= 255,
            Green: >= 0 and <= 255,
            Blue: >= 0 and <= 255,
        };
    }
}