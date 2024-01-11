namespace FEngLib.Messaging.Commands;

public class SwitchToPackage : PackageCommand
{
    public SwitchToPackage(string packageName) : base(packageName)
    {
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_SwitchToPackage;
    }

    public override string GetCommandName()
    {
        return "SwitchToPackage";
    }
}