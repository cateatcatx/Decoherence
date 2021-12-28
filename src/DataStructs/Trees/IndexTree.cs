#if !NET35
#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Decoherence.Collections.Trees
{
    public class IndexTree<TKey, TData> : ITree<TData>, IReadOnlyDictionary<TKey, IReadOnlyNode<TData>>
        where TKey : notnull
    {
        public IReadOnlyNode<TData>? Root => mTree.Root;
        IEnumerable<IReadOnlyNode> IReadOnlyTree.GetChildren(IReadOnlyNode parent)
        {
            return ((IReadOnlyTree)mTree).GetChildren(parent);
        }

        public int Count => mDic.Count;
        

        public IReadOnlyNode<TData> this[TKey key] => mDic[key];
        public IEnumerable<TKey> Keys => mDic.Keys;
        public IEnumerable<IReadOnlyNode<TData>> Values => mDic.Values;
        
        private readonly Tree<TData> mTree = new();
        private readonly Dictionary<TKey, IReadOnlyNode<TData>> mDic = new();
        private readonly Func<TData, TKey> mData2Key;

        public IndexTree(Func<TData, TKey> data2Key)
        {
            ThrowHelper.ThrowIfArgumentNull(data2Key, nameof(data2Key));

            mData2Key = data2Key;
        }
        
        public INode<TData> SetRoot(TData data)
        {
            var key = mData2Key(data);
            ThrowHelper.ThrowIfArgument(mDic.ContainsKey(key), nameof(data), $"Key {key} already exists.");
            
            var root = mTree.SetRoot(data);
            mDic.Add(key, root);
            return root;
        }

        public INode<TData> AddChild(IReadOnlyNode<TData> parent, TData data)
        {
            var key = mData2Key(data);
            
            ThrowHelper.ThrowIfArgument(mDic.ContainsKey(key), nameof(data), $"Key {key} already exists.");

            var child = mTree.AddChild(parent, data);
            mDic.Add(key, child);
            return child;
        }

        public bool RemoveChild(IReadOnlyNode<TData> parent, IReadOnlyNode<TData> child)
        {
            if (mTree.RemoveChild(parent, child))
            {
                mDic.Remove(mData2Key(child.Data));
                return true;
            }

            return false;
        }
        
        public IEnumerable<IReadOnlyNode<TData>> GetChildren(IReadOnlyNode parent)
        {
            return mTree.GetChildren(parent);
        }

        public IEnumerator<KeyValuePair<TKey, IReadOnlyNode<TData>>> GetEnumerator()
        {
            return mDic.GetEnumerator();
        }

        public bool ContainsKey(TKey key)
        {
            return mDic.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out IReadOnlyNode<TData> value)
        {
            return mDic.TryGetValue(key, out value);
        }

        #region Hidden
        
        IReadOnlyNode? IReadOnlyTree.Root => Root;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)mDic).GetEnumerator();
        }
        
        INode ITree.SetRoot(object? data)
        {
            return SetRoot((TData)data!);
        }

        INode ITree.AddChild(INode parent, object? data)
        {
            return AddChild((IReadOnlyNode<TData>)parent, (TData)data!);
        }

        bool ITree.RemoveChild(INode parent, INode child)
        {
            return RemoveChild((IReadOnlyNode<TData>)parent, (IReadOnlyNode<TData>)child);
        }
        
        #endregion
    }
}

#nullable disable
#endif