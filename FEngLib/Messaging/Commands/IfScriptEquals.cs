namespace FEngLib.Messaging.Commands;

public class IfScriptEquals : ScriptCommand
{
    public IfScriptEquals(uint scriptHash) : base(scriptHash)
    {
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_IfScriptEquals;
    }

    public override string GetCommandName()
    {
        return "IfScriptEquals";
    }
}