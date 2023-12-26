using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using FEngLib;
using FEngLib.Packages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FEngCli;

[Verb("compile")]
public class CompileCommand : BaseCommand
{
    [Option('i', Required = true)] public IEnumerable<string> InputPath { get; set; }

    [Option('o', Required = true)] public string OutputPath { get; set; }

    public override int Execute()
    {
        var packages = InputPath.Select(path => JsonConvert.DeserializeObject<Package>(File.ReadAllText(path),
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                },
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore
            }));

        using var bw = new BinaryWriter(File.Create(OutputPath));

        foreach (var package in packages)
        {
            using var tms = new MemoryStream();
            using var tbw = new BinaryWriter(tms);
            new FrontendChunkWriter(package).Write(tbw);
            tms.Position = 0;

            bw.Write(0x30203);
            bw.Write((uint)tms.Length);
            tms.CopyTo(bw.BaseStream);
        }

        return 0;
    }
}