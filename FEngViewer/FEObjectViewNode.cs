using System.Collections.Generic;
using FEngLib;

namespace FEngViewer
{
    
    public class FEObjectViewNode
    {
        public FrontendObject Obj;
        /// <summary>
        /// If Obj represents a group, this contains its children.
        /// </summary>
        public List<FEObjectViewNode> Children;

        public FEObjectViewNode(FrontendObject obj)
        {
            Obj = obj;
            Children = new List<FEObjectViewNode>();
        }
    }
}