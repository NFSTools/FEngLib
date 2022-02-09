using System.Collections.Generic;

namespace FEngLib.Scripts;

public class ScriptTracks
{
    public ColorTrack Color { get; set; }
    public Vector3Track Pivot { get; set; }
    public Vector3Track Position { get; set; }
    public QuaternionTrack Rotation { get; set; }
    public Vector3Track Size { get; set; }
}

public abstract class Script
{
    protected Script()
    {
        // Tracks = new List<Track>();
        Events = new List<Event>();
    }

    public string Name { get; set; }
    public uint Id { get; set; }
    public uint ChainedId { get; set; } = 0xFFFFFFFF;
    public uint Length { get; set; }
    public uint Flags { get; set; }

    // public List<Track> Tracks { get; }
    public List<Event> Events { get; }

    public abstract ScriptTracks GetTracks();
}

public abstract class Script<TTracks> : Script where TTracks : ScriptTracks, new()
{
    protected Script()
    {
        Tracks = new TTracks();
    }

    public TTracks Tracks { get; protected init; }

    public override ScriptTracks GetTracks()
    {
        return Tracks;
    }
}