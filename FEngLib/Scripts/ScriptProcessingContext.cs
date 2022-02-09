namespace FEngLib.Scripts;

public class ScriptProcessingContext
{
    public ScriptProcessingContext(Script script)
    {
        Script = script;
    }

    public Script Script { get; }
    public Track CurrentTrack { get; set; }
}