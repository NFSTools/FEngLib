namespace FEngLib.Scripts;

public abstract class TrackNode
{
    public int Time { get; set; }

    public abstract object GetValue();
}

public class TrackNode<TValue> : TrackNode where TValue : struct
{
    public TrackNode()
    {
        Val = default;
    }

    public TValue Val { get; set; }

    public override object GetValue()
    {
        return Val;
    }
}