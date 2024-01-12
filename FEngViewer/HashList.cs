using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FEngLib.Utils;

namespace FEngViewer;

internal class HashList
{
    private Dictionary<uint, string> _dictionary;

    internal static HashList FromEmbeddedFile(string name)
    {
        var dict = new Dictionary<uint, string>();
        using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        if (s == null)
            throw new Exception($"Could not find embedded file: {name}");
        using var sr = new StreamReader(s);
        while (sr.ReadLine() is { } line)
        {
            var hash = Hashing.BinHash(line.ToUpper());
            if (!dict.TryGetValue(hash, out var existing))
                dict.Add(hash, line);
            else if (!string.Equals(line, existing, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception($"Hash conflict in {name}: {line} and {existing} both hash to 0x{hash:X8}");
            // dict.Add(Hashing.BinHash(line), line);
        }

        return new HashList
        {
            _dictionary = dict
        };
    }

    internal string Lookup(uint hash)
    {
        return _dictionary.TryGetValue(hash, out var s) ? s : $"0x{hash:X8}";
    }
}