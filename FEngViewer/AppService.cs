using System;
using System.Collections.Generic;
using System.IO;
using FEngLib.Packages;

namespace FEngViewer;

public class AppService
{
    private static readonly Lazy<AppService> LazyInstance = new(() => new AppService());
    private Package _currentPackage;

    private AppService()
    {
    }

    public static AppService Instance => LazyInstance.Value;

    public Package LoadFile(string path)
    {
        using var fs = new FileStream(path, FileMode.Open);
        using var fr = new BinaryReader(fs);
        var marker = fr.ReadUInt32();
        switch (marker)
        {
            case 0x30203:
                fs.Seek(0x10, SeekOrigin.Begin);
                break;
            case 0xE76E4546:
                fs.Seek(0x8, SeekOrigin.Begin);
                break;
            default:
                throw new InvalidDataException($"Invalid FEng chunk file: {path}");
        }

        using var ms = new MemoryStream();
        fs.CopyTo(ms);
        ms.Position = 0;

        using var mr = new BinaryReader(ms);
        return _currentPackage = new FrontendPackageLoader().Load(mr);
    }

    public List<ResourceRequest> GetResourceRequests()
    {
        return _currentPackage?.ResourceRequests ??
               throw new NullReferenceException("Received a request for resource requests when no package is loaded!");
    }
}