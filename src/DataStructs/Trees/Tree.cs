#if !NET35
#nullable enable

using System;
using System.Collections.Generic;


namespace Decoherence.Collections.Trees
{
    public class Tree<T> : ITree<T>
    {
        private static readonly IEnumerable<IReadOnlyNode<T>> EmptyNodes = new IReadOnlyNode<T>[0];

        public IReadOnlyNode<T>? Root => mRoot;
        IEnumerable<IReadOnlyNode> IReadOnlyTree.GetChildren(IReadOnlyNode parent)
        {
            return GetChildren(parent);
        }

        private Node? mRoot;

        public Tree()
        {
        }

        public Tree(T rootData)
        {
            SetRoot(rootData);
        }

        IReadOnlyNode? IReadOnlyTree.Root => Root;

        public IEnumerable<IReadOnlyNode<T>> GetChildren(IReadOnlyNode parent)
        {
            var node = _CheckNode(parent, nameof(parent));
            return node.mChildren ?? EmptyNodes;
        }

        public INode<T> SetRoot(T data)
        {
            mRoot = _NewNode(data);
            return mRoot;
        }

        public INode<T> AddChild(IReadOnlyNode<T> parent, T data)
        {
            var node = _CheckNode(parent, nameof(parent));
            node.mChildren ??= new List<Node>();
            var child = _NewNode(data);
            node.mChildren.Add(child);
            return child;
        }

        public bool RemoveChild(IReadOnlyNode<T> parent, IReadOnlyNode<T> child)
        {
            var parentNode = _CheckNode(parent, nameof(parent));
            var childNode = _CheckNode(child, nameof(child));
            return parentNode.mChildren != null && parentNode.mChildren.Remove(childNode);
        }

        private Node _NewNode(T data)
        {
            return new(data, this);
        }

        private Node _CheckNode(IReadOnlyNode node, string paramName)
        {
            ThrowUtil.ThrowIfArgumentNull(node, paramName);
            
            var checkingNode = (Node)node;
            if (checkingNode.mTree != this)
            {
                throw new ArgumentException($"Not current tree node.");
            }

            return checkingNode;
        }

        private class Node : INode<T>
        {
            public T Data { get; private set; }
            
            internal readonly IReadOnlyTree<T> mTree;
            internal List<Node>? mChildren;
            
            public Node(T data, IReadOnlyTree<T> tree)
            {
                Data = data;
                mTree = tree;
            }

            public void SetData(T data)
            {
                Data = data;
            }

            #region Hidden

            object? IReadOnlyNode.Data => Data;
            
            void INode.SetData(object? data)
            {
                Data = (T)data!;
            }

            #endregion

            
        }

        #region Hidden
        
        INode ITree.SetRoot(object? data)
        {
            return SetRoot((T)data!);
        }

        INode ITree.AddChild(INode parent, object? data)
        {
            return AddChild((INode<T>)parent, (T)data!);
        }

        bool ITree.RemoveChild(INode parent, INode child)
        {
            return RemoveChild((INode<T>)parent, (INode<T>)child);
        }
        
        #endregion
    }
}

#nullable disable
#endif