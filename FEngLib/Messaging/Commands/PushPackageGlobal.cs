namespace FEngLib.Messaging.Commands;

public class PushPackageGlobal : PackageCommand
{
    public PushPackageGlobal(string packageName) : base(packageName)
    {
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_PushPackageGlobal;
    }

    public override string GetCommandName()
    {
        return "PushPackageGlobal";
    }
}