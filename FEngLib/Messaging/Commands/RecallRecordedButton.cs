namespace FEngLib.Messaging.Commands;

public class RecallRecordedButton : ObjectCommand
{
    public RecallRecordedButton(uint objectGuid) : base(objectGuid)
    {
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_RecallRecordedButton;
    }

    public override string GetCommandName()
    {
        return "RecallRecordedButton";
    }
}