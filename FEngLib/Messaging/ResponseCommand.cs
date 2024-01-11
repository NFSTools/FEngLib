using System;

namespace FEngLib.Messaging;

//public class Response
//{
//    public uint Id { get; set; }
//    public uint? IntParam { get; set; }
//    public string StringParam { get; set; }
//    public uint Target { get; set; }
//}

public abstract class ResponseCommand
{
    //public abstract uint Id { get; }

    public abstract uint GetId();
    public abstract string GetCommandName();
    public abstract override string ToString();
}

public interface ITargetedCommand
{
    uint Target { get; set; }
}

public interface IParameterizedCommand<TParameter>
{
    TParameter GetParameter();
    void SetParameter(TParameter parameter);
}

public interface IStringCommand : IParameterizedCommand<string>
{
}

public interface IIntegerCommand : IParameterizedCommand<uint>
{ }

public abstract class PackageCommand : ResponseCommand, IStringCommand
{
    public string PackageName { get; private set; }

    protected PackageCommand(string packageName)
    {
        PackageName = packageName;
    }

    public string GetParameter()
    {
        return PackageName;
    }

    public void SetParameter(string parameter)
    {
        PackageName = parameter ?? throw new ArgumentNullException(nameof(parameter));
    }

    public override string ToString()
    {
        var escapedPkgName = PackageName.Replace(@"\", @"\\");
        return $"{GetCommandName()}(\"{escapedPkgName}\")";
    }
}

public abstract class MessageCommand : ResponseCommand, IIntegerCommand
{
    public uint MessageHash { get; private set; }

    protected MessageCommand(uint messageHash)
    {
        MessageHash = messageHash;
    }

    public uint GetParameter()
    {
        return MessageHash;
    }

    public void SetParameter(uint parameter)
    {
        MessageHash = parameter;
    }
}

public abstract class ScriptCommand : ResponseCommand, IIntegerCommand
{
    public uint ScriptHash { get; private set; }

    protected ScriptCommand(uint scriptHash)
    {
        ScriptHash = scriptHash;
    }

    public uint GetParameter()
    {
        return ScriptHash;
    }

    public void SetParameter(uint parameter)
    {
        ScriptHash = parameter;
    }

    public override string ToString()
    {
        return $"{GetCommandName()}(0x{ScriptHash:X})";
    }
}

public abstract class ObjectCommand : ResponseCommand, IIntegerCommand
{
    public uint ObjectGuid { get; private set; }

    protected ObjectCommand(uint objectGuid)
    {
        ObjectGuid = objectGuid;
    }

    public uint GetParameter()
    {
        return ObjectGuid;
    }

    public void SetParameter(uint parameter)
    {
        ObjectGuid = parameter;
    }

    public override string ToString()
    {
        return $"{GetCommandName()}(0x{ObjectGuid:X})";
    }
}

public abstract class NonParameterizedCommand : ResponseCommand
{
    public override string ToString()
    {
        return $"{GetCommandName()}()";
    }
}