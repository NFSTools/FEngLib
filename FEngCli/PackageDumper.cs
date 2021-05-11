using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FEngLib;
using FEngLib.Data;
using FEngLib.Objects;

namespace FEngCli
{
    public static class PackageDumper
    {
        public static void DumpPackage(FrontendPackage package)
        {
            DumpPackageMeta(package);
            for (var index = 0; index < package.ResourceRequests.Count; index++)
            {
                var resourceRequest = package.ResourceRequests[index];
                DumpResourceRequest(index, resourceRequest);
            }

            foreach (var frontendObject in package.Objects.OrderBy(o => o.Parent?.Guid).ThenBy(o => o.Guid))
                DumpObject(frontendObject, package);

            foreach (var messageDefinition in package.MessageDefinitions)
            {
                Console.WriteLine("--------- MESSAGE DEFINITION ---------");
                Console.WriteLine("MSG Name     : {0} ({1})", messageDefinition.Name, messageDefinition.Name.ToUpper());
                if (!string.IsNullOrEmpty(messageDefinition.Category))
                    Console.WriteLine("MSG Category : {0} ({1})", messageDefinition.Category,
                        messageDefinition.Category.ToUpper());
            }

            foreach (var messageTargetList in package.MessageTargetLists)
            {
                Console.WriteLine("--------- MESSAGE TARGET LIST: 0x{0:X} ---------", messageTargetList.MsgId);
                messageTargetList.Targets.ForEach(t => Console.WriteLine("- 0x{0:X}", t));
            }

            foreach (var messageResponse in package.MessageResponses)
            {
                Console.WriteLine("--------- MESSAGE RESPONSE: 0x{0:X} ---------", messageResponse.Id);
                messageResponse.Responses.ForEach(t =>
                    Console.WriteLine("- ID: 0x{0:X}; Target: 0x{1:X}; Param: {2}", t.Id, t.Target, t.Param));
            }
        }

        private static void DumpPackageMeta(FrontendPackage package)
        {
            Console.WriteLine("--------- PACKAGE ---------");
            Console.WriteLine("Name      : {0}", package.Name);
            Console.WriteLine("File Name : {0}", package.Filename);
            Console.WriteLine("Objects   : {0}", package.Objects.Count);
            Console.WriteLine("Resources : {0}", package.ResourceRequests.Count);
        }

        public static void DumpResourceRequest(int index, FEResourceRequest resourceRequest)
        {
            Console.WriteLine("--------- RESOURCE REQUEST ---------");
            Console.WriteLine("Index  : {0}", index);
            Console.WriteLine("Name   : {0}", resourceRequest.Name);
            Console.WriteLine("Type   : {0}", resourceRequest.Type);
            Console.WriteLine("Flags  : {0}", resourceRequest.Flags);
            Console.WriteLine("Handle : {0}", resourceRequest.Handle);
            Console.WriteLine("ID     : 0x{0:X8}", resourceRequest.ID);
        }

        public static void DumpObject(FrontendObject frontendObject, FrontendPackage package)
        {
            // if ((frontendObject.Type != FEObjType.FE_Image || frontendObject.ResourceIndex != 4) && frontendObject.Type != FEObjType.FE_Group) return;

            Console.WriteLine("--------- OBJECT ---------");
            if (!string.IsNullOrEmpty(frontendObject.Name))
                Console.WriteLine("Name     : {0} ({1})", frontendObject.Name, frontendObject.Name.ToUpper());
            Console.WriteLine("Hash     : 0x{0:X8}", frontendObject.NameHash);
            Console.WriteLine("GUID     : 0x{0:X}", frontendObject.Guid);
            Console.WriteLine("Flags    : {0}", frontendObject.Flags);
            if (frontendObject.Parent != null)
                Console.WriteLine("Parent   : 0x{0:X}", frontendObject.Parent.Guid);
            Console.WriteLine("Position : {0}", frontendObject.Position);
            Console.WriteLine("Size     : {0}", frontendObject.Size);
            Console.WriteLine("Pivot    : {0}", frontendObject.Pivot);
            Console.WriteLine("Rotation : {0}", frontendObject.Rotation);
            Console.WriteLine("Color    : {0}", frontendObject.Color);
            Console.WriteLine("Type     : {0}", frontendObject.Type);
            if (frontendObject.ResourceRequest != null)
                Console.WriteLine("Resource : {0}", frontendObject.ResourceRequest.Name);

            if (frontendObject.Scripts.Count > 0)
            {
                Console.WriteLine("Scripts ({0}):", frontendObject.Scripts.Count);

                foreach (var frontendScript in frontendObject.Scripts)
                {
                    Console.WriteLine("\t----------");
                    Console.WriteLine("\tID     : 0x{0:X}", frontendScript.Id);
                    if (!string.IsNullOrEmpty(frontendScript.Name))
                        Console.WriteLine("\tName   : {0} ({1})", frontendScript.Name, frontendScript.Name.ToUpper());
                    if (frontendScript.ChainedId != 0xFFFFFFFF)
                        Console.WriteLine("\tChained to: 0x{0:X}", frontendScript.ChainedId);
                    Console.WriteLine("\tFlags  : 0x{0:X}", frontendScript.Flags);
                    Console.WriteLine("\tLength : {0}", frontendScript.Length);
                    if (frontendScript.Events.Count > 0)
                    {
                        Console.WriteLine("\tEvents ({0}):", frontendScript.Events.Count);
                        foreach (var scriptEvent in frontendScript.Events)
                            Console.WriteLine("\t\tTarget=0x{0:X} Time={1} EventId=0x{2:X}", scriptEvent.Target,
                                scriptEvent.Time, scriptEvent.EventId);
                    }

                    if (frontendScript.Tracks.Count > 0)
                    {
                        Console.WriteLine("\tTracks ({0}):", frontendScript.Tracks.Count);

                        foreach (var track in frontendScript.Tracks)
                        {
                            Console.WriteLine("\t\t---------- TRACK:");
                            Console.WriteLine("\t\tLength       : {0}", track.Length);
                            Console.WriteLine("\t\tOffset       : {0}", track.Offset);
                            Console.WriteLine("\t\tParamType    : {0}", track.ParamType);
                            Console.WriteLine("\t\tParamSize    : {0}", track.ParamSize);
                            Console.WriteLine("\t\tInterpAction : {0}", track.InterpAction);
                            Console.WriteLine("\t\tInterpType   : {0}", track.InterpType);
                            Console.WriteLine("\t\tBaseKey      : {0}", DumpKey(track.BaseKey));

                            var deltaKeys = track.DeltaKeys.ToList();
                            for (var i = 0; i < deltaKeys.Count; i++)
                                Console.WriteLine("\t\tDeltaKeys[{0:D2}]: {1}", i, DumpKey(deltaKeys[i]));

                            static string DumpKey(FEKeyNode keyNode)
                            {
                                return $"T={keyNode.Time} V={keyNode.Val}";
                            }
                        }
                    }
                }
            }

            if (frontendObject.MessageResponses.Count > 0)
            {
                Console.WriteLine("Message Responses ({0}):", frontendObject.MessageResponses.Count);
                foreach (var messageResponse in frontendObject.MessageResponses)
                {
                    Console.WriteLine("\t----------");
                    Console.WriteLine("\tID: 0x{0:X}", messageResponse.Id);
                    Console.WriteLine("\tResponses ({0}):", messageResponse.Responses.Count);
                    messageResponse.Responses.ForEach(t =>
                        Console.WriteLine("\t\t- ID: 0x{0:X}; Target: 0x{1:X}; Param: {2}", t.Id, t.Target, t.Param));
                }
            }

            Console.WriteLine("Extended data:");
            switch (frontendObject)
            {
                case FrontendString frontendString:
                    Console.WriteLine("\tString hash   : 0x{0:X}", frontendString.Hash);
                    if (!string.IsNullOrEmpty(frontendString.Label))
                        Console.WriteLine("\tString ID     : {0} ({1})", frontendString.Label,
                            frontendString.Label.ToUpper());
                    Console.WriteLine("\tDefault Text  : {0}", frontendString.Value);
                    Console.WriteLine("\tMaximum width : {0}", frontendString.MaxWidth);
                    Console.WriteLine("\tText leading  : {0}", frontendString.Leading);
                    Console.WriteLine("\tText format   : {0}", frontendString.Formatting);
                    break;
                case FrontendMultiImage frontendMultiImage:
                    Console.WriteLine("\tImage flags   : {0}", frontendMultiImage.ImageFlags);
                    Console.WriteLine("\tTextures      : {0}",
                        string.Join(", ", frontendMultiImage.Texture.Select(h => h.ToString("X8"))));
                    break;
                case FrontendColoredImage frontendMultiImage:
                    Console.WriteLine("\tImage flags   : {0}", frontendMultiImage.ImageFlags);
                    Console.WriteLine("\tColors        : {0}",
                        string.Join(", ", frontendMultiImage.VertexColors.Select(h => h.ToString())));
                    break;
                case FrontendImage frontendImage:
                    Console.WriteLine("\tImage flags   : {0}", frontendImage.ImageFlags);
                    Console.WriteLine("\tUpper Left    : {0}", frontendImage.UpperLeft);
                    Console.WriteLine("\tLower Right   : {0}", frontendImage.LowerRight);
                    break;
            }
        }
    }
}