using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngRender.Data
{
    /// <summary>
    /// Represents a list of <see cref="RenderTreeNode"/> objects.
    /// </summary>
    public class RenderTree : IEnumerable<RenderTreeNode>
    {
        private readonly List<RenderTreeNode> _nodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTree"/> class.
        /// </summary>
        /// <param name="nodes">The list of <see cref="RenderTreeNode"/> instances contained by the tree.</param>
        public RenderTree(List<RenderTreeNode> nodes)
        {
            _nodes = nodes;
        }

        /// <inheritdoc />
        public IEnumerator<RenderTreeNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public static IEnumerable<RenderTreeNode> GetAllTreeNodesForRendering(IEnumerable<RenderTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                // TODO should we maybe use HideInEdit instead of this separate attribute?
                //  The games ignore it anyway, so there's no real problem with persisting that flag.
                if (node.Hidden) continue;
                
                if ((node.FrontendObject.Flags & ObjectFlags.Invisible) != 0 ||
                    (node.FrontendObject.Flags & ObjectFlags.HideInEdit) != 0)
                {
                    continue;
                }

                yield return node;

                if (!(node is RenderTreeGroup grp)) continue;

                foreach (var rtn in GetAllTreeNodesForRendering(grp))
                {
                    yield return rtn;
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="RenderTree"/> from
        /// the data contained in the given <see cref="Package"/> object.
        /// </summary>
        /// <param name="package">The <see cref="Package"/> object to build a tree for.</param>
        /// <returns>The newly constructed <see cref="RenderTree"/> object.</returns>
        public static RenderTree Create(Package package)
        {
            var nodes = new List<RenderTreeNode>();

            var childrenDict = package.Objects
                .Where(o => o.Type == ObjectType.Group)
                .ToDictionary(
                    o => o.Guid, 
                    o => new List<IObject<ObjectData>>());

            // Build up children mapping
            foreach (var frontendObject in package.Objects.Where(o => o.Parent != null))
            {
                childrenDict[frontendObject.Parent.Guid].Add(frontendObject);
            }

            void GenerateNodes(IEnumerable<IObject<ObjectData>> frontendObjects, IList<RenderTreeNode> nodeList)
            {
                foreach (var frontendObject in frontendObjects)
                {
                    if (frontendObject is Group grp)
                    {
                        var subNodes = new List<RenderTreeNode>();
                        GenerateNodes(childrenDict[grp.Guid], subNodes);
                        nodeList.Add(new RenderTreeGroup(grp, subNodes));
                    }
                    else
                    {
                        nodeList.Add(new RenderTreeNode(frontendObject));
                    }
                }
            }

            // Start generating root nodes
            GenerateNodes(package.Objects.Where(o => o.Parent == null), nodes);

            return new RenderTree(nodes);
        }
    }
}