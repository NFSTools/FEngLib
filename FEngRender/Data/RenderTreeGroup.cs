using System.Collections;
using System.Collections.Generic;
using FEngLib.Objects;

namespace FEngRender.Data
{
    /// <summary>
    /// Represents a node in the render tree that can have children.
    /// </summary>
    public class RenderTreeGroup : RenderTreeNode, IEnumerable<RenderTreeNode>
    {
        private readonly List<RenderTreeNode> _children;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderTreeGroup"/> class.
        /// </summary>
        /// <param name="frontendGroup">The <see cref="FrontendGroup"/> that the <see cref="RenderTreeGroup"/> refers to.</param>
        /// <param name="nodes">The list of child <see cref="RenderTreeNode"/> instances.</param>
        // ReSharper disable once SuggestBaseTypeForParameter - We want to restrict this class to groups only! No other objects can have children.
        public RenderTreeGroup(FrontendGroup frontendGroup, List<RenderTreeNode> nodes) : base(frontendGroup)
        {
            _children = nodes;
        }

        /// <inheritdoc />
        public IEnumerator<RenderTreeNode> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}