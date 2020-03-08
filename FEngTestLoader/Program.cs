using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FEngLib;
using FEngLib.Data;
using FEngLib.Objects;

namespace FEngTestLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var path = @"test-data\mw\InGameRivalBio.fng";
            FrontendPackage package = LoadDumpedChunk(path);
            stopwatch.Stop();
            long elapsed = stopwatch.ElapsedMilliseconds;

            Debug.WriteLine("Loaded {0} in {1}ms", path, elapsed);
            DumpPackageInfo(package);
        }

        private static void DumpPackageInfo(FrontendPackage package)
        {
            Debug.WriteLine("Package Info:");
            Debug.WriteLine("\tName:                 {0}", new object[] {package.Name});
            Debug.WriteLine("\tFile Name:            {0}", new object[] {package.Filename});
            Debug.WriteLine("\t# Objects:            {0}", package.Objects.Count);
            Debug.WriteLine("\t# Msg Responses:      {0}", package.MessageResponses.Count);
            Debug.WriteLine("\t# Target Lists:       {0}", package.MessageTargetLists.Count);
            Debug.WriteLine("\t# Resource Requests:  {0}", package.MessageTargetLists.Count);
            Debug.WriteLine("");
            Debug.WriteLine("Objects:");
            foreach (var frontendObject in package.Objects)
            {
                Debug.WriteLine("\tObject");
                Debug.WriteLine("\t\tHash:  0x{0:X8}", frontendObject.NameHash);
                Debug.WriteLine("\t\tType:  {0}", frontendObject.Type);
                Debug.WriteLine("\t\tFlags: {0}", frontendObject.Flags);
                Debug.WriteLine("\t\tScripts ({0}):", frontendObject.Scripts.Count);
                DumpScripts(frontendObject.Scripts);
                Debug.WriteLine("\t\tMsg Responses ({0}):", frontendObject.MessageResponses.Count);
                DumpMsgResponses(frontendObject.MessageResponses);
                DumpObjectInfo(frontendObject);
            }
        }

        private static void DumpMsgResponses(List<FEMessageResponse> messageResponses)
        {
            foreach (var messageResponse in messageResponses)
            {
                Debug.WriteLine("\t\t\tMessage Response");
                Debug.WriteLine("\t\t\t\tID: 0x{0:X8}", messageResponse.Id);
                Debug.WriteLine("\t\t\t\tResponses ({0}):", messageResponse.Responses.Count);

                foreach (var response in messageResponse.Responses)
                {
                    Debug.WriteLine("\t\t\t\t\tID:     0x{0:X8}", response.Id);
                    Debug.WriteLine("\t\t\t\t\tParam:  0x{0:X8}", response.Param);
                    Debug.WriteLine("\t\t\t\t\tTarget: 0x{0:X8}", response.Target);
                }
            }
        }

        private static void DumpScripts(IEnumerable<FrontendScript> scripts)
        {
            foreach (var script in scripts)
            {
                Debug.WriteLine("\t\t\tScript");
                Debug.WriteLine("\t\t\t\tID:     0x{0:X8}", script.Id);
                Debug.WriteLine("\t\t\t\tFlags:  {0}", script.Flags);
                Debug.WriteLine("\t\t\t\tLength: {0}", script.Length);
                Debug.WriteLine("\t\t\t\tEvents ({0}):", script.Events.Count);
                Debug.WriteLine("\t\t\t\tTracks ({0}):", script.Tracks.Count);
            }
        }

        private static void DumpObjectInfo(FrontendObject frontendObject)
        {
            switch (frontendObject)
            {
                case FrontendString frontendString:
                    DumpString(frontendString);
                    break;
            }
        }

        private static void DumpString(FrontendString frontendString)
        {
            Debug.WriteLine("\t\t(String) Value: {0}", new object[] { frontendString.Value });
        }

        private static FrontendPackage LoadDumpedChunk(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read) { Position = 0x10 };
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            ms.Position = 0;
            ms.SetLength(fs.Length - 0x10);
            using var br = new BinaryReader(ms);
            return new FrontendPackageLoader().Load(br);
        }
    }
}
