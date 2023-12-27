using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FEngLib.Objects;
using FEngLib.Packages;

namespace FEngRender.Data;

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
            // TODO make this behavior controllable at runtime (i.e. "show all hidden", "show all invisible")
            if (node.IsHidden())
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
            .Where(o => o.GetObjectType() == ObjectType.Group)
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
                RenderTreeGroup GroupToRenderTreeNode(Group group)
                {
                    var subNodes = new List<RenderTreeNode>();
                    GenerateNodes(childrenDict[group.Guid], subNodes);
                    return new RenderTreeGroup(group, subNodes);
                }

                nodeList.Add(frontendObject switch
                {
                    ColoredImage ci => new RenderTreeColoredImage(ci),
                    Group grp => GroupToRenderTreeNode(grp),
                    Image img => new RenderTreeImage(img),
                    MultiImage mi => new RenderTreeMultiImage(mi),
                    SimpleImage si => new RenderTreeSimpleImage(si),
                    Movie movie => new RenderTreeMovie(movie),
                    Text text => new RenderTreeText(text),
                    _ => throw new NotImplementedException($"Unsupported object type: {frontendObject.GetObjectType()}")
                });
            }
        }

        // Start generating root nodes
        GenerateNodes(package.Objects.Where(o => o.Parent == null), nodes);

        return new RenderTree(nodes);
    }
}