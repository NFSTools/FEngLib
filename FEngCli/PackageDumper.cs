using System;
using System.Collections.Generic;
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
            if (frontendObject.ResourceIndex > -1)
                Console.WriteLine("Resource : {0} ({1})", frontendObject.ResourceIndex,
                    package.ResourceRequests[frontendObject.ResourceIndex].Name);

            if (frontendObject.Scripts.Count > 0)
            {
                Console.WriteLine("Scripts ({0}):", frontendObject.Scripts.Count);

                foreach (var frontendScript in frontendObject.Scripts)
                {
                    Console.WriteLine("\t----------");
                    Console.WriteLine("\tID     : 0x{0:X8}", frontendScript.Id);
                    if (!string.IsNullOrEmpty(frontendScript.Name))
                        Console.WriteLine("\tName   : {0} ({1})", frontendScript.Name, frontendScript.Name.ToUpper());
                    Console.WriteLine("\tFlags  : 0x{0:X8}", frontendScript.Flags);
                    Console.WriteLine("\tLength : {0}", frontendScript.Length);
                    if (frontendScript.Events.Count > 0)
                    {
                        Console.WriteLine("\tEvents ({0}):", frontendScript.Events.Count);
                        foreach (var scriptEvent in frontendScript.Events)
                            Console.WriteLine("\t\tTarget={0:X8} Time={1} EventId={2:X8}", scriptEvent.Target,
                                scriptEvent.Time, scriptEvent.EventId);
                    }

                    if (frontendScript.Tracks.Count > 0)
                    {
                        Console.WriteLine("\tTracks ({0}):", frontendScript.Tracks.Count);

                        var offsetToIndex = new Dictionary<uint, string>
                        {
                            {0, "FETrack_Color"},
                            {4, "FETrack_Pivot"},
                            {7, "FETrack_Position"},
                            {10, "FETrack_Rotation"},
                            {14, "FETrack_Size"},
                            {17, "FETrack_UpperLeft"},
                            {19, "FETrack_UpperRight"},
                            {21, "FETrack_FrameNumber OR FETrack_Color1"},
                            {25, "FETrack_Color2"},
                            {29, "FETrack_Color3"},
                            {33, "FETrack_Color4"}
                        };
                        for (var trackIndex = 0; trackIndex < frontendScript.Tracks.Count; trackIndex++)
                        {
                            var track = frontendScript.Tracks[trackIndex];
                            Console.WriteLine("\t\t---------- {0}:", offsetToIndex[track.Offset]);
                            Console.WriteLine("\t\tLength       : {0}", track.Length);
                            Console.WriteLine("\t\tOffset       : {0}", track.Offset);
                            Console.WriteLine("\t\tParamType    : {0}", track.ParamType);
                            Console.WriteLine("\t\tParamSize    : {0}", track.ParamSize);
                            Console.WriteLine("\t\tInterpAction : {0}", track.InterpAction);
                            Console.WriteLine("\t\tInterpType   : {0}", track.InterpType);
                            Console.WriteLine("\t\tBaseKey      : {0}", DumpKey(track.BaseKey));

                            for (var i = 0; i < track.DeltaKeys.Count; i++)
                                Console.WriteLine("\t\tDeltaKeys[{0:D2}]: {1}", i, DumpKey(track.DeltaKeys[i]));

                            string DumpKey(FEKeyNode keyNode)
                            {
                                return $"T={keyNode.Time} V={keyNode.Val}";
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Extended data:");
            switch (frontendObject)
            {
                case FrontendString frontendString:
                    Console.WriteLine("\tString hash   : 0x{0:X8}", frontendString.Hash);
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
                case FrontendImage frontendImage:
                    Console.WriteLine("\tImage flags   : {0}", frontendImage.ImageFlags);
                    break;
            }
        }
    }
}