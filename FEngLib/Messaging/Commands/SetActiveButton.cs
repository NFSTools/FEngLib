namespace FEngLib.Messaging.Commands;

public class SetActiveButton : ObjectCommand
{
    public SetActiveButton(uint objectGuid) : base(objectGuid)
    {
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_SetActiveButton;
    }

    public override string GetCommandName()
    {
        return "SetActiveButton";
    }
}