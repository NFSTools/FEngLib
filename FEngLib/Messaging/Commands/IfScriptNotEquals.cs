namespace FEngLib.Messaging.Commands;

public class IfScriptNotEquals : ScriptCommand
{
    public IfScriptNotEquals(uint scriptHash) : base(scriptHash)
    {
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_IfScriptNotEquals;
    }

    public override string GetCommandName()
    {
        return "IfScriptNotEquals";
    }
}