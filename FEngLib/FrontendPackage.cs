using System.Collections.Generic;

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
        public FrontendObject LastObject => Objects.Count > 0 ? Objects[^1] : null;

        public FrontendPackage()
        {
            Objects = new List<FrontendObject>();
        }
    }
}
