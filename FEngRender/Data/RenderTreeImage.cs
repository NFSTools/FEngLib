using System.Numerics;
using FEngLib.Objects;
using FEngLib.Scripts;
using FEngLib.Structures;

namespace FEngRender.Data;

public abstract class RenderTreeImage<TImage, TScriptTracks> : RenderTreeNode<TImage, ImageScript<TScriptTracks>, TScriptTracks>
    where TImage : IImage<ImageData>, IScriptedObject<ImageScript<TScriptTracks>>
    where TScriptTracks : ImageScriptTracks, new()
{
    public Vector2 UpperLeft { get; set; }
    public Vector2 LowerRight { get; set; }

    protected RenderTreeImage(TImage frontendObject) : base(frontendObject)
    {
    }

    protected override void LoadProperties()
    {
        base.LoadProperties();

        UpperLeft = FrontendObject.Data.UpperLeft;
        LowerRight = FrontendObject.Data.LowerRight;
    }

    protected override void ApplyScript(ImageScript<TScriptTracks> script, TScriptTracks tracks)
    {
        base.ApplyScript(script, tracks);

        if (tracks.UpperLeft is { } upperLeftTrack)
            UpperLeft = InterpolateHelper(upperLeftTrack);
        if (tracks.LowerRight is { } lowerRightTrack)
            LowerRight = InterpolateHelper(lowerRightTrack);
    }
}

public class RenderTreeImage : RenderTreeImage<Image, ImageScriptTracks>
{
    public RenderTreeImage(Image frontendObject) : base(frontendObject)
    {
    }
}

public class RenderTreeColoredImage : RenderTreeImage<ColoredImage, ColoredImageScriptTracks>
{
    public Color4 TopLeft { get; set; }
    public Color4 TopRight { get; set; }
    public Color4 BottomRight { get; set; }
    public Color4 BottomLeft { get; set; }

    public RenderTreeColoredImage(ColoredImage frontendObject) : base(frontendObject)
    {
    }

    protected override void LoadProperties()
    {
        base.LoadProperties();

        TopLeft = FrontendObject.Data.TopLeft;
        TopRight = FrontendObject.Data.TopRight;
        BottomRight = FrontendObject.Data.BottomRight;
        BottomLeft = FrontendObject.Data.BottomLeft;
    }

    protected override void ApplyScript(ImageScript<ColoredImageScriptTracks> script, ColoredImageScriptTracks tracks)
    {
        base.ApplyScript(script, tracks);

        if (tracks.TopLeft is { } topLeftTrack)
            TopLeft = InterpolateHelper(topLeftTrack);
        if (tracks.TopRight is { } topRightTrack)
            TopRight = InterpolateHelper(topRightTrack);
        if (tracks.BottomRight is { } bottomRightTrack)
            BottomRight = InterpolateHelper(bottomRightTrack);
        if (tracks.BottomLeft is { } bottomLeftTrack)
            BottomLeft = InterpolateHelper(bottomLeftTrack);
    }
}

public class RenderTreeMultiImage : RenderTreeImage<MultiImage, MultiImageScriptTracks>
{
    public Vector2 TopLeft1 { get; set; }
    public Vector2 TopLeft2 { get; set; }
    public Vector2 TopLeft3 { get; set; }
    public Vector2 BottomRight1 { get; set; }
    public Vector2 BottomRight2 { get; set; }
    public Vector2 BottomRight3 { get; set; }
    public Vector3 PivotRotation { get; set; }

    public RenderTreeMultiImage(MultiImage frontendObject) : base(frontendObject)
    {
    }

    protected override void LoadProperties()
    {
        base.LoadProperties();

        TopLeft1 = FrontendObject.Data.TopLeft1;
        TopLeft2 = FrontendObject.Data.TopLeft2;
        TopLeft3 = FrontendObject.Data.TopLeft3;
        BottomRight1 = FrontendObject.Data.BottomRight1;
        BottomRight2 = FrontendObject.Data.BottomRight2;
        BottomRight3 = FrontendObject.Data.BottomRight3;
        PivotRotation = FrontendObject.Data.PivotRotation;
    }

    protected override void ApplyScript(ImageScript<MultiImageScriptTracks> script, MultiImageScriptTracks tracks)
    {
        base.ApplyScript(script, tracks);

        if (tracks.TopLeft1 is { } topLeft1Track)
            TopLeft1 = InterpolateHelper(topLeft1Track);
        if (tracks.TopLeft2 is { } topLeft2Track)
            TopLeft2 = InterpolateHelper(topLeft2Track);
        if (tracks.TopLeft3 is { } topLeft3Track)
            TopLeft3 = InterpolateHelper(topLeft3Track);
        if (tracks.BottomRight1 is { } bottomRight1Track)
            BottomRight1 = InterpolateHelper(bottomRight1Track);
        if (tracks.BottomRight2 is { } bottomRight2Track)
            BottomRight2 = InterpolateHelper(bottomRight2Track);
        if (tracks.BottomRight3 is { } bottomRight3Track)
            BottomRight3 = InterpolateHelper(bottomRight3Track);
        if (tracks.PivotRotation is { } pivotRotationTrack)
            PivotRotation = InterpolateHelper(pivotRotationTrack);
    }
}

public class RenderTreeSimpleImage : RenderTreeNode<SimpleImage>
{
    public RenderTreeSimpleImage(SimpleImage frontendObject) : base(frontendObject)
    {
    }
}