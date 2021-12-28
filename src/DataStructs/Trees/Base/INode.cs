#if !NET35
#nullable enable

namespace Decoherence.Collections.Trees
{
    public interface INode : IReadOnlyNode
    {
        void SetData(object? data);
    }
    
    public interface INode<TData> : IReadOnlyNode<TData>, INode
    {
        void SetData(TData data);
    }
}

#nullable disable
#endif