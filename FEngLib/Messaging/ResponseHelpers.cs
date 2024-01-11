using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FEngLib.Messaging.Commands;

namespace FEngLib.Messaging;

internal static class ResponseHelpers
{
    public static void PopulateMessageResponseList(
        IEnumerable<MessageResponseTagProcessor.IMessageResponseEntry> messageResponseEntries,
        IHaveMessageResponses entity)
    {
        foreach (var messageResponseEntry in messageResponseEntries)
        {
            var commands = messageResponseEntry.Select(CreateCommand).ToList();
            entity.MessageResponses.Add(new MessageResponse(messageResponseEntry.ID, commands));
        }
    }

    private static ResponseCommand CreateCommand(MessageResponseTagProcessor.ResponseCommandEntry entry)
    {
        var commandId = (FEMessageResponseCommands)entry.ID;
        var target = entry.Target ?? throw new Exception($"{commandId} command is missing target");
        ResponseCommand constructedCommand = (commandId, entry.IParam, entry.SParam) switch
        {
            (FEMessageResponseCommands.MR_PushPackageGlobal, null, { } packageName) => new PushPackageGlobal(packageName),
            (FEMessageResponseCommands.MR_PostMessageToSound, { } messageId, null) => new PostMessageToSound(messageId, target),
            (FEMessageResponseCommands.MR_SetScript, { } scriptId, null) => new SetScript(scriptId),
            (FEMessageResponseCommands.MR_PostMessageToFEng, { } messageId, null) => new PostMessageToFEng(messageId, target),
            (FEMessageResponseCommands.MR_PostMessageToGame, { } messageId, null) => new PostMessageToGame(messageId),
            (FEMessageResponseCommands.MR_PopPackage, 0, null) => new PopPackage(),
            (FEMessageResponseCommands.MR_SwitchToPackage, null, { } packageName) => new SwitchToPackage(packageName),
            (FEMessageResponseCommands.MR_SetActiveButton, { } objectGuid, null) => new SetActiveButton(objectGuid),
            (FEMessageResponseCommands.MR_IfScriptEquals, { } scriptId, null) => new IfScriptEquals(scriptId),
            (FEMessageResponseCommands.MR_EndIf, 0, null) => new EndIf(),
            (FEMessageResponseCommands.MR_Else, not null /* because of ONE BROKEN FNG, this can't be 0 */, null) => new Else(),
            (FEMessageResponseCommands.MR_IfScriptNotEquals, { } scriptId, null) => new IfScriptNotEquals(scriptId),
            (FEMessageResponseCommands.MR_SetInputProcessing, { } /* also because of that broken FNG (sigh) */ x, null) => new SetInputProcessing(x),
            (FEMessageResponseCommands.MR_RecallRecordedButton, { } objectGuid, null) => new RecallRecordedButton(objectGuid),
            (FEMessageResponseCommands.MR_RecordCurrentButton, 0, null) => new RecordCurrentButton(),
            var cmd => throw new Exception($"Invalid or unimplemented command: {cmd}")
        };

        // debugging stuff
        //Debug.Assert(constructedCommand.GetId() == entry.ID);

        //if (entry.Target is > 0 && (constructedCommand is not ITargetedCommand targetedCommand ||
        //                            targetedCommand.Target != entry.Target))
        //{
        //    Debug.WriteLine("warn: target mismatch between {0} (0x{1:X}) and {2}", commandId, entry.Target, constructedCommand.GetType());
        //}

        //if (constructedCommand is NonParameterizedCommand)
        //{
        //    if (entry.IParam is {} param && param != 0)
        //    {
        //        Debug.WriteLine("warn: explicitly non-parameterized command {0} has non-zero parameter 0x{1:X}", constructedCommand.GetType(), param);

        //        Debugger.Break();
        //    }

        //    if (entry.SParam is { Length: > 0 } sparam)
        //    {
        //        Debug.WriteLine("warn: explicitly non-parameterized command {0} has string parameter {1}", constructedCommand.GetType(), sparam);

        //        Debugger.Break();
        //    }
        //}
        //else
        //{
        //    if (entry.IParam != null && (constructedCommand is not IParameterizedCommand<uint> parameterizedCommand ||
        //                                 parameterizedCommand.GetParameter() != entry.IParam))
        //    {
        //        Debugger.Break();
        //    }

        //    if (entry.SParam != null && (constructedCommand is not IParameterizedCommand<string> parameterizedCommand2 ||
        //                                 parameterizedCommand2.GetParameter() != entry.SParam))
        //    {
        //        Debugger.Break();
        //    }
        //}

        //Debug.Assert((constructedCommand is not ITargetedCommand && (entry.Target ?? 0) == 0) ||
        //             (constructedCommand is ITargetedCommand targetedCommand && targetedCommand.Target == entry.Target));
        return constructedCommand;
    }

    internal enum FEMessageResponseCommands
    {
        MR_SetScript = 0x0,
        MR_PostMessageToFEng = 0x1,
        MR_PostMessageToGame = 0x2,
        MR_PostMessageToSound = 0x3,
        MR_SetActiveButton = 0x100,
        MR_SetInputProcessing = 0x101,
        MR_RecordCurrentButton = 0x102,
        MR_RecallRecordedButton = 0x103,
        MR_DontNavigate = 0x104,
        MR_PassControlToChildCurrent = 0x105,
        MR_PassControlToChildGlobal = 0x106,
        MR_PassControlToParentCurrent = 0x107,
        MR_PassControlToParentGlobal = 0x108,
        MR_SwitchToPackage = 0x200,
        MR_PushPackageGlobal = 0x201,
        MR_PushPackageCurrent = 0x202,
        MR_PopPackage = 0x203,
        MR_PushPackageNone = 0x204,
        MR_RecordPackageMarker = 0x2C0,
        MR_SwitchToPackageMarker = 0x2C1,
        MR_ClearPackageMarkers = 0x2C2,
        MR_IfScriptEquals = 0x300,
        MR_IfScriptNotEquals = 0x301,
        MR_Else = 0x500,
        MR_EndIf = 0x501,
    }
}