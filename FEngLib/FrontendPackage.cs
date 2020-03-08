using System.Collections.Generic;
using FEngLib.Data;

namespace FEngLib
{
    /// <summary>
    /// Stores frontend objects, resource names, etc.
    /// </summary>
    public class FrontendPackage
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public List<FrontendObject> Objects { get; set; }
        public List<FEMessageResponse> MessageResponses { get; set; }
        public List<FEMessageTargetList> MessageTargetLists { get; set; }

        public FrontendPackage()
        {
            Objects = new List<FrontendObject>();
            MessageResponses = new List<FEMessageResponse>();
            MessageTargetLists = new List<FEMessageTargetList>();
        }
    }
}
