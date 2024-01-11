namespace FEngLib.Messaging.Commands;

public class PostMessageToFEng : MessageCommand, ITargetedCommand
{
    public uint Target { get; set; }

    public PostMessageToFEng(uint messageHash, uint target) : base(messageHash)
    {
        Target = target;
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_PostMessageToFEng;
    }

    public override string GetCommandName()
    {
        return "PostMessageToFEng";
    }

    public override string ToString()
    {
        return $"{GetCommandName()}(0x{Target:X}, 0x{MessageHash:X8})";
    }
}