using CommandLine;

namespace FEngCli;

internal static class Program
{
    private static int Main(string[] args)
    {
        return Parser.Default
            .ParseArguments(args, typeof(DecompileCommand), typeof(CompileCommand))
            .MapResult((BaseCommand bc) => bc.Execute(), errs => 1);
    }
}