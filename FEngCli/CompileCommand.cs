using System.Collections.Generic;
using System.IO;
using CommandLine;
using FEngLib;
using FEngLib.Packages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FEngCli;

[Verb("compile")]
public class CompileCommand : BaseCommand
{
    [Option('i')] public string InputPath { get; set; }

    [Option('o')] public string OutputPath { get; set; }

    public override int Execute()
    {
        var package = JsonConvert.DeserializeObject<Package>(File.ReadAllText(InputPath), new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            },
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Error,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
        using var bw = new BinaryWriter(File.OpenWrite(OutputPath));
        new FrontendChunkWriter(package).Write(bw);

        return 0;
    }
}