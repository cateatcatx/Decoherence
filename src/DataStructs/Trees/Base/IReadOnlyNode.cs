#if !NET35
#nullable enable

namespace Decoherence.Collections.Trees
{
    public interface IReadOnlyNode
    {
        object? Data { get; }
    }
    
    public interface IReadOnlyNode<out TData> : IReadOnlyNode
    {
        new TData Data { get; }
    }
}

#nullable disable
#endif