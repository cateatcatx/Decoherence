#if !NET35
#nullable enable

namespace Decoherence.Collections.Trees
{
    public interface ITree : IReadOnlyTree
    {
        INode SetRoot(object? data);
        
        INode AddChild(INode parent, object? data);
        
        bool RemoveChild(INode parent, INode child);
    }
    
    public interface ITree<TData> : IReadOnlyTree<TData>, ITree
    {
        INode<TData> SetRoot(TData data);
        
        INode<TData> AddChild(IReadOnlyNode<TData> parent, TData data);
        
        bool RemoveChild(IReadOnlyNode<TData> parent, IReadOnlyNode<TData> child);
    }
}

#nullable disable
#endif