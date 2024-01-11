namespace FEngLib.Messaging.Commands;

public class Else : NonParameterizedCommand
{
    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_Else;
    }

    public override string GetCommandName()
    {
        return "Else";
    }

    public override string ToString()
    {
        return GetCommandName();
    }
}