#if !NET35
#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Decoherence.Collections.Trees
{
    public interface IReadOnlyTree
    {
        IReadOnlyNode? Root { get; }
    
        IEnumerable<IReadOnlyNode> GetChildren(IReadOnlyNode parent);
    }

    public interface IReadOnlyTree<out TData> : IReadOnlyTree
    {
        new IReadOnlyNode<TData>? Root { get; }

        new IEnumerable<IReadOnlyNode<TData>> GetChildren(IReadOnlyNode parent);
    }
}

#nullable disable
#endif