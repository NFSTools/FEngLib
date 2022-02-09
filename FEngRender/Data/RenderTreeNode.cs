using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using FEngLib.Objects;
using FEngLib.Scripts;
using FEngLib.Structures;
using FEngRender.Script;

namespace FEngRender.Data;

/// <summary>
/// Base class for representing an item in a <see cref="RenderTree"/>.
/// </summary>
public class RenderTreeNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderTreeNode"/> class.
    /// </summary>
    /// <param name="frontendObject">
    ///   The <see cref="IObject{TData}"/> instance owned by the <see cref="RenderTreeNode"/> instance.
    /// </param>
    public RenderTreeNode(IObject<ObjectData> frontendObject)
    {
        FrontendObject = frontendObject;
        SetScript(frontendObject.FindScript(0x1744B3)); // INIT
    }

    public Matrix4x4 ObjectMatrix { get; private set; }

    public Quaternion ObjectRotation { get; private set; }

    public Color4 ObjectColor { get; private set; }

    // TODO: THESE SHOULD *NOT* BE HERE!!!
    // TODO cont: REFACTORING IS NECESSARY!!!!!!!!!!!
    public Vector2 UpperLeft { get; private set; }
    public Vector2 LowerRight { get; private set; }

    /// <summary>
    /// The <see cref="IObject{TData}"/> instance owned by the node.
    /// </summary>
    public IObject<ObjectData> FrontendObject { get; }

    /// <summary>
    /// The <see cref="Script"/> that is currently running.
    /// </summary>
    public FEngLib.Scripts.Script CurrentScript { get; private set; }

    /// <summary>
    /// The current time offset of the current script.
    /// </summary>
    public int CurrentScriptTime { get; private set; }

    /// <summary>
    /// Applies script state and computes transformations.
    /// </summary>
    /// <param name="viewMatrix"></param>
    /// <param name="parentNode"></param>
    /// <param name="deltaTime"></param>
    /// <param name="matrixRotate"></param>
    public void PrepareForRender(Matrix4x4 viewMatrix, RenderTreeNode parentNode, int deltaTime,
        bool matrixRotate = false)
    {
        var frontendObjectData = FrontendObject.Data;
        var size = frontendObjectData.Size;
        var position = frontendObjectData.Position;
        var rotation = frontendObjectData.Rotation;
        var pivot = frontendObjectData.Pivot;
        var color = frontendObjectData.Color;

        if (frontendObjectData is ImageData imageData)
        {
            UpperLeft = imageData.UpperLeft;
            LowerRight = imageData.LowerRight;
        }

        if (CurrentScript != null
            && CurrentScriptTime >= 0)
        {
            var scriptTracks = CurrentScript.GetTracks();

            if (scriptTracks.Color is { } colorTrack)
                color = TrackInterpolation.Interpolate(colorTrack, CurrentScriptTime);

            if (scriptTracks.Pivot is { } pivotTrack)
                pivot = TrackInterpolation.Interpolate(pivotTrack, CurrentScriptTime);

            if (scriptTracks.Position is { } positionTrack)
                position = TrackInterpolation.Interpolate(positionTrack, CurrentScriptTime);

            if (scriptTracks.Rotation is { } rotationTrack)
                rotation = TrackInterpolation.Interpolate(rotationTrack, CurrentScriptTime);

            if (scriptTracks.Size is { } sizeTrack) size = TrackInterpolation.Interpolate(sizeTrack, CurrentScriptTime);

            if (scriptTracks is ImageScriptTracks imageScriptTracks)
            {
                if (imageScriptTracks.LowerRight is { } lowerRightTrack)
                    LowerRight = TrackInterpolation.Interpolate(lowerRightTrack, CurrentScriptTime);
                if (imageScriptTracks.UpperLeft is { } upperLeftTrack)
                    UpperLeft = TrackInterpolation.Interpolate(upperLeftTrack, CurrentScriptTime);
            }

            if (CurrentScriptTime >= CurrentScript.Length)
            {
                if ((CurrentScript.Flags & 1) == 1)
                {
                    Debug.WriteLine("looping script {0:X} for object {1:X}", CurrentScript.Id,
                        FrontendObject.NameHash);
                    CurrentScriptTime = 0;
                }
                else if (CurrentScript.ChainedId != 0xFFFFFFFF)
                {
                    var nextScript = FrontendObject.FindScript(CurrentScript.ChainedId) ??
                                     throw new Exception(
                                         $"Cannot find chained script (object {FrontendObject.NameHash:X}, base script {CurrentScript.Id:X}): {CurrentScript.ChainedId:X}");
                    Debug.WriteLine("activating chained script for object {1:X}: {0}",
                        nextScript.Name ?? nextScript.Id.ToString("X"), FrontendObject.NameHash);

                    SetScript(nextScript);
                }
                else
                {
                    Debug.WriteLine("Current script for object {0:X} has ended", FrontendObject.NameHash);
                    SetScript(null);
                }
            }

            CurrentScriptTime += deltaTime;
        }

        var scaleMatrix = Matrix4x4.CreateScale(size.X, size.Y, size.Z);
        var rotateMatrix = Matrix4x4.CreateFromQuaternion(rotation);
        var transMatrix = Matrix4x4.CreateTranslation(position.X, position.Y, position.Z);
        var outMatrix = Matrix4x4.Identity;

        outMatrix *= scaleMatrix;

        if (matrixRotate && rotation != Quaternion.Identity)
        {
            outMatrix *= Matrix4x4.CreateTranslation(-pivot);
            outMatrix *= rotateMatrix;
            outMatrix *= Matrix4x4.CreateTranslation(pivot);
        }

        outMatrix *= transMatrix;
        outMatrix *= viewMatrix;

        ObjectMatrix = outMatrix;

        ObjectRotation = rotation;
        ObjectColor = color;

        if (parentNode != null)
        {
            ObjectRotation *= parentNode.ObjectRotation;
            ObjectColor = Color4.Blend(ObjectColor, parentNode.ObjectColor);
        }
    }

    /// <summary>
    /// Sets the currently running script and resets the script time.
    /// </summary>
    /// <param name="script">The script to run.</param>
    public void SetScript(FEngLib.Scripts.Script script)
    {
        this.CurrentScript = script;
        this.CurrentScriptTime = script == null ? -1 : 0;
    }

    /// <summary>
    /// Gets the object's Z-coordinate.
    /// </summary>
    /// <returns></returns>
    public float GetZ()
    {
        return ObjectMatrix.M43;
    }

    // private Track GetKeyTrack(FEngLib.Scripts.Script script, KeyTrackType trackType)
    // {
    //     uint offset = (uint)trackType;
    //
    //     return script.Tracks.Find(e => e.Offset == offset);
    // }

    public virtual Rectangle? Get2DExtents()
    {
        switch (FrontendObject.Type)
        {
            case ObjectType.Image:
            case ObjectType.Movie:
            case ObjectType.ColoredImage:
                var pos = FrontendObject.Data.Position;
                var objSize = new Size((int)FrontendObject.Data.Size.X, (int)FrontendObject.Data.Size.Y);
                var topLeft = new Point((int)(pos.X - objSize.Width * 0.5), (int)(pos.Y - objSize.Height * 0.5));
                return new Rectangle(topLeft, objSize);
            case ObjectType.String:
                //todo
                return new Rectangle();
            case ObjectType.Group:
                throw new Exception("This node should be a RenderTreeGroup!!!");
            case ObjectType.Model:
            case ObjectType.List:
            case ObjectType.CodeList:
            case ObjectType.Effect:
            case ObjectType.AnimImage:
            case ObjectType.SimpleImage:
            case ObjectType.MultiImage:
                return new Rectangle();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private enum KeyTrackType
    {
        Color = 0,
        Pivot = 4,
        Position = 7,
        Rotation = 10,
        Size = 14,
        UpperLeft = 17,
        LowerRight = 19
    }
}