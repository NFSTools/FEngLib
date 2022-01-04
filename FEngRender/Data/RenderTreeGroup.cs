using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        /// <param name="frontendGroup">The <see cref="Group"/> that the <see cref="RenderTreeGroup"/> refers to.</param>
        /// <param name="nodes">The list of child <see cref="RenderTreeNode"/> instances.</param>
        // ReSharper disable once SuggestBaseTypeForParameter - We want to restrict this class to groups only! No other objects can have children.
        public RenderTreeGroup(Group frontendGroup, List<RenderTreeNode> nodes) : base(frontendGroup)
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

        // Groups extents are the smallest rectangle that contains all of its members' extents
        public override Rectangle? Get2DExtents()
        {
            if (FrontendObject.Type != ObjectType.Group) // this should never be possible?
                return base.Get2DExtents();

            if (_children.Count == 0)
                return null;
            
            var narrowestRectMaybe = _children.First().Get2DExtents();


            if (!narrowestRectMaybe.HasValue)
                return null;

            var narrowestRect = (Rectangle)narrowestRectMaybe;
            
            foreach (var child in _children)
            {
                var childExtents = child.Get2DExtents();
                if (childExtents.HasValue)
                    narrowestRect = Rectangle.Union(narrowestRect, childExtents.Value);
            }

            narrowestRect.Location += new Size((int)FrontendObject.Data.Position.X, (int) FrontendObject.Data.Position.Y);
            
            return narrowestRect;
        }
    }
}