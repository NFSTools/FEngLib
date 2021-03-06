﻿using System.IO;
using FEngLib.Packages;
using FEngLib.Tags;

namespace FEngLib.Objects.Tags
{
    public class StringBufferLeadingTag : Tag
    {
        public StringBufferLeadingTag(IObject<ObjectData> frontendObject) : base(frontendObject)
        {
        }

        public int Leading { get; set; }

        public override void Read(BinaryReader br, FrontendChunkBlock chunkBlock, Package package,
            ushort id,
            ushort length)
        {
            Leading = br.ReadInt32();
        }
    }
}