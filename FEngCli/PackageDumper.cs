using System;
using System.Numerics;
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

            foreach (var frontendObject in package.Objects) DumpObject(frontendObject);
        }

        private static void DumpPackageMeta(FrontendPackage package)
        {
            Console.WriteLine("Package:");
            Console.WriteLine("\tName      : {0}", package.Name);
            Console.WriteLine("\tFile Name : {0}", package.Filename);
            Console.WriteLine("\tObjects   : {0}", package.Objects.Count);
            Console.WriteLine("\tResources : {0}", package.ResourceRequests.Count);
        }

        public static void DumpResourceRequest(int index, FEResourceRequest resourceRequest)
        {
            Console.WriteLine("Resource request:");
            Console.WriteLine("\tIndex  : {0}", index);
            Console.WriteLine("\tName   : {0}", resourceRequest.Name);
            Console.WriteLine("\tType   : {0}", resourceRequest.Type);
            Console.WriteLine("\tFlags  : {0}", resourceRequest.Flags);
            Console.WriteLine("\tHandle : {0}", resourceRequest.Handle);
            Console.WriteLine("\tID     : {0:X8}", resourceRequest.ID);
        }

        public static void DumpObject(FrontendObject frontendObject)
        {
            Console.WriteLine(frontendObject);
            Console.WriteLine("\tGUID     : 0x{0:X}", frontendObject.Guid);
            Console.WriteLine("\tFlags    : {0}", frontendObject.Flags);
            if (frontendObject.Parent != null)
                Console.WriteLine("\tParent   : 0x{0:X}", frontendObject.Parent.Guid);
            Console.WriteLine("\tPosition : {0}", frontendObject.Position);
            Console.WriteLine("\tSize     : {0}", frontendObject.Size);
            Console.WriteLine("\tExtent   : {0}",
                new Vector3(frontendObject.Position.X + frontendObject.Size.X,
                    frontendObject.Position.Y + frontendObject.Size.Y,
                    frontendObject.Position.Z + frontendObject.Size.Z));
            Console.WriteLine("\tPivot    : {0}", frontendObject.Pivot);
            Console.WriteLine("\tRotation : {0}", frontendObject.Rotation);
            Console.WriteLine("\tColor    : {0}", frontendObject.Color);
            Console.WriteLine("\tType     : {0}", frontendObject.Type);
            Console.WriteLine("\tResource : {0}", frontendObject.ResourceIndex);

            switch (frontendObject)
            {
                case FrontendString frontendString:
                    Console.WriteLine("\t\tText          : {0}", frontendString.Value);
                    Console.WriteLine("\t\tString hash   : {0:X8}", frontendString.Hash);
                    Console.WriteLine("\t\tMaximum width : {0}", frontendString.MaxWidth);
                    Console.WriteLine("\t\tText leading  : {0}", frontendString.Leading);
                    Console.WriteLine("\t\tText format   : {0}", frontendString.Formatting);
                    break;
            }
        }
    }
}