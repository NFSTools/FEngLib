﻿using System.IO;
using FEngLib.Data;

namespace FEngLib.Tags
{
    public class ObjectDataTag : FrontendTag
    {
        public FEObjData Data { get; set; }

        public override void Read(BinaryReader br, ushort length)
        {
            Data = FrontendObject.Type switch
            {
                FEObjType.FE_Image => new FEImageData(),
                _ => new FEObjData()
            };

            Data.Read(br);
        }

        public ObjectDataTag(FrontendObject frontendObject) : base(frontendObject)
        {
        }
    }
}