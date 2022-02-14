using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using FEngLib.Objects;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngRender.Scripts;

namespace FEngRender.Data;

/// <summary>
/// Base class for representing an item in a <see cref="RenderTree"/>.
/// </summary>
public abstract class RenderTreeNode
{
    public Matrix4x4 Transform { get; protected set; }

    public Color4 BlendedColor { get; protected set; }

    public Quaternion ObjectRotation { get; protected set; }

    /// <summary>
    /// Updates the state of the render node.
    /// </summary>
    /// <param name="context">The current rendering context.</param>
    /// <param name="deltaMs">The time (in milliseconds) that has passed since the last frame render.</param>
    public abstract void Update(RenderContext context, int deltaMs);

    public abstract IObject<ObjectData> GetObject();

    public abstract Script GetCurrentScript();

    public abstract void SetCurrentScript(uint? id);

    public abstract bool IsHidden();

    /// <summary>
    /// Gets the object's Z-coordinate.
    /// </summary>
    /// <returns></returns>
    public float GetZ()
    {
        return Transform.M43;
    }

    public virtual Rectangle? Get2DExtents()
    {
        // TODO
        return Rectangle.Empty;
        //switch (FrontendObject.Type)
        //{
        //    case ObjectType.Image:
        //    case ObjectType.Movie:
        //    case ObjectType.ColoredImage:
        //        var pos = FrontendObject.Data.Position;
        //        var objSize = new Size((int)FrontendObject.Data.Size.X, (int)FrontendObject.Data.Size.Y);
        //        var topLeft = new Point((int)(pos.X - objSize.Width * 0.5), (int)(pos.Y - objSize.Height * 0.5));
        //        return new Rectangle(topLeft, objSize);
        //    case ObjectType.String:
        //        //todo
        //        return new Rectangle();
        //    case ObjectType.Group:
        //        throw new Exception("This node should be a RenderTreeGroup!!!");
        //    case ObjectType.Model:
        //    case ObjectType.List:
        //    case ObjectType.CodeList:
        //    case ObjectType.Effect:
        //    case ObjectType.AnimImage:
        //    case ObjectType.SimpleImage:
        //    case ObjectType.MultiImage:
        //        return new Rectangle();
        //    default:
        //        throw new ArgumentOutOfRangeException();
        //}
    }
}

public abstract class RenderTreeNode<TObject, TScript, TScriptTracks> : RenderTreeNode
    where TObject : IObject<ObjectData>, IScriptedObject<TScript>
    where TScript : Script<TScriptTracks>
    where TScriptTracks : ScriptTracks, new()
{
    protected RenderTreeNode(TObject frontendObject)
    {
        FrontendObject = frontendObject;
        SetCurrentScript(0x1744B3);
    }

    /// <summary>
    /// The <see cref="IObject{TData}"/> instance owned by the node.
    /// </summary>
    public TObject FrontendObject { get; }

    /// <summary>
    /// The <see cref="Script"/> that is currently running.
    /// </summary>
    public TScript CurrentScript { get; private set; }

    /// <summary>
    /// The current time offset of the current script.
    /// </summary>
    public int CurrentScriptTime { get; private set; }

    public Vector3 Position { get; private set; }
    public Vector3 Size { get; private set; }
    public Quaternion Rotation { get; private set; }
    public Vector3 Pivot { get; private set; }
    public Color4 Color { get; private set; }

    public override void Update(RenderContext context, int deltaMs)
    {
        LoadProperties();

        if (CurrentScript is { } currentScript
            && CurrentScriptTime >= 0)
        {
            ApplyScript(currentScript, currentScript.Tracks);

            if (CurrentScriptTime >= CurrentScript.Length)
            {
                if ((CurrentScript.Flags & 1) == 1)
                {
                    Debug.WriteLine("Looping script {0:X} for object {1:X}", CurrentScript.Id,
                        FrontendObject.NameHash);
                    CurrentScriptTime = 0;
                }
                else if (CurrentScript.ChainedId is { } currentScriptChainedId)
                {
                    var nextScript = FrontendObject.FindScript(currentScriptChainedId) ??
                                     throw new Exception(
                                         $"Cannot find chained script (object {FrontendObject.NameHash:X}, base script {CurrentScript.Id:X}): {currentScriptChainedId:X}");
                    Debug.WriteLine("Activating chained script for object {1:X}: {0}",
                        nextScript.Name ?? nextScript.Id.ToString("X"), FrontendObject.NameHash);

                    SetCurrentScript(nextScript);
                }
                else
                {
                    Debug.WriteLine("Current script for object {0:X} has ended", FrontendObject.NameHash);
                    SetCurrentScript(null);
                }
            }
            else
            {
                CurrentScriptTime += deltaMs;
            }
        }

        var scaleMatrix = Matrix4x4.CreateScale(Size);
        var rotateMatrix = Matrix4x4.CreateFromQuaternion(Rotation);
        var transMatrix = Matrix4x4.CreateTranslation(Position);
        var outMatrix = Matrix4x4.Identity;

        outMatrix *= scaleMatrix;

        if (Rotation != Quaternion.Identity)
        {
            outMatrix *= Matrix4x4.CreateTranslation(-Pivot);
            outMatrix *= rotateMatrix;
            outMatrix *= Matrix4x4.CreateTranslation(Pivot);
        }

        outMatrix *= transMatrix;
        outMatrix *= context.ViewMatrix;

        ObjectRotation = Rotation;
        Transform = outMatrix;
        BlendedColor = Color;

        if (context.Parent is { } parentNode)
        {
            ObjectRotation *= parentNode.ObjectRotation;
            BlendedColor = Color4.Blend(Color, parentNode.BlendedColor);
        }
    }

    public override Script GetCurrentScript() => CurrentScript;

    public override IObject<ObjectData> GetObject() => FrontendObject;

    public sealed override void SetCurrentScript(uint? id)
    {
        SetCurrentScript(id != null ? this.FrontendObject.FindScript(id.Value) : null);
    }

    public override bool IsHidden()
    {
        return (FrontendObject.Flags & ObjectFlags.HideInEdit) != 0 ||
               (FrontendObject.Flags & ObjectFlags.Invisible) != 0;
    }

    private void SetCurrentScript(TScript script)
    {
        CurrentScript = script;
        CurrentScriptTime = script == null ? -1 : 0;
    }

    protected virtual void LoadProperties()
    {
        Color = FrontendObject.Data.Color;
        Pivot = FrontendObject.Data.Pivot;
        Position = FrontendObject.Data.Position;
        Rotation = FrontendObject.Data.Rotation;
        Size = FrontendObject.Data.Size;
    }

    protected virtual void ApplyScript(TScript script, TScriptTracks tracks)
    {
        if (tracks.Color is { } colorTrack)
            Color = InterpolateHelper(colorTrack);
        if (tracks.Pivot is { } pivotTrack)
            Pivot = InterpolateHelper(pivotTrack);
        if (tracks.Position is { } positionTrack)
            Position = InterpolateHelper(positionTrack);
        if (tracks.Rotation is { } rotationTrack)
            Rotation = InterpolateHelper(rotationTrack);
        if (tracks.Size is { } sizeTrack) Size = InterpolateHelper(sizeTrack);
    }

    protected T InterpolateHelper<T>(Track<T> track) where T : struct
    {
        return TrackInterpolation.Interpolate(track, CurrentScriptTime);
    }
}

public abstract class RenderTreeNode<TObject> : RenderTreeNode<TObject, BaseObjectScript, ScriptTracks>
    where TObject : IObject<ObjectData>, IScriptedObject<BaseObjectScript>
{
    protected RenderTreeNode(TObject frontendObject) : base(frontendObject)
    {
    }
}