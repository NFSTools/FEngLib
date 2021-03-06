﻿using System.IO;
using FEngLib.Packages;
using FEngLib.Tags;
using FEngLib.Utils;

namespace FEngLib.Objects.Tags
{
    public class StringBufferFormattingTag : Tag
    {
        public StringBufferFormattingTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public TextFormat Formatting { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            Formatting = br.ReadEnum<TextFormat>();
        }
    }
}