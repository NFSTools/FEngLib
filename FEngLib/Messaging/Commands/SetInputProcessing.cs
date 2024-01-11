namespace FEngLib.Messaging.Commands;

public class SetInputProcessing : ResponseCommand, IIntegerCommand
{
    public bool Enabled { get; private set; }

    public SetInputProcessing(bool enabled) : this(enabled ? 1u : 0u)
    {
    }

    internal SetInputProcessing(uint parameter)
    {
        SetParameter(parameter);
    }

    public override uint GetId()
    {
        return (uint)ResponseHelpers.FEMessageResponseCommands.MR_SetInputProcessing;
    }

    public override string GetCommandName()
    {
        return "SetInputProcessing";
    }

    public override string ToString()
    {
        return $"{GetCommandName()}({Enabled})";
    }

    public uint GetParameter()
    {
        return Enabled ? 1u : 0u;
    }

    public void SetParameter(uint parameter)
    {
        Enabled = parameter == 1;
    }
}