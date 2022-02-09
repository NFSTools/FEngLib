using System.Collections.Generic;
using System.IO;
using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FEngCli;

[Verb("decompile")]
public class DecompileCommand : BaseCommand
{
    [Option('i')] public string InputPath { get; set; }

    [Option('o')] public string OutputPath { get; set; }

    public override int Execute()
    {
        var package = PackageLoader.Load(InputPath);

        File.WriteAllText(OutputPath, JsonConvert.SerializeObject(package, new JsonSerializerSettings
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

        return 0;
    }
}