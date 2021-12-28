#if !NET35
#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Decoherence.Collections.Trees
{
    public static class TreeAlgorithms
    {
        public static async Task GenerateChildrenAsync<TNode, TData>(this ITree<TData> tree,
            TNode parent,
            Func<TNode, Task<IEnumerable<TData>?>> onGetChildDatas, 
            bool recursive)
            where TNode : class, IReadOnlyNode<TData>
        {
            ThrowHelper.ThrowIfArgumentNull(tree, nameof(tree));
            ThrowHelper.ThrowIfArgumentNull(parent, nameof(parent));
            ThrowHelper.ThrowIfArgumentNull(onGetChildDatas, nameof(onGetChildDatas));

            var childDatas = await onGetChildDatas(parent);
            if (childDatas == null)
            {
                return;
            }
            
            foreach (var childData in childDatas)
            {
                var child = tree.AddChild((INode)parent, childData);

                if (recursive)
                {
                    await GenerateChildrenAsync(tree, (TNode)child, onGetChildDatas, recursive);
                }
            }
        }

        public static void ConvertTo<TSourceData, TTargetData>(this IReadOnlyTree<TSourceData> sourceTree,
            ITree<TTargetData> targetTree,
            Func<TSourceData, TTargetData> converter)
        {
            ThrowHelper.ThrowIfArgumentNull(sourceTree, nameof(sourceTree));
            ThrowHelper.ThrowIfArgumentNull(targetTree, nameof(targetTree));
            ThrowHelper.ThrowIfArgumentNull(converter, nameof(converter));
            
            if (sourceTree.Root == null)
            {
                return;
            }

            var targetRoot = targetTree.SetRoot(converter(sourceTree.Root.Data));
            _ConvertTo(sourceTree, sourceTree.Root, targetTree, targetRoot, converter);
        }

        public static ITree<TTargetData> ConvertAll<TSourceData, TTargetData>(this IReadOnlyTree<TSourceData> sourceTree, 
            Func<TSourceData, TTargetData> converter)
        {
            ThrowHelper.ThrowIfArgumentNull(sourceTree, nameof(sourceTree));
            ThrowHelper.ThrowIfArgumentNull(converter, nameof(converter));

            var targetTree = new Tree<TTargetData>();
            ConvertTo(sourceTree, targetTree, converter);
            return targetTree;
        }

        public static void PreDepthFirstTraverse<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode, bool> onTraverse)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }

            _DepthFirstTraverseAsync(true, tree, default, (TNode)tree.Root, (_, node) => Task.FromResult(onTraverse(node)))
                .Wait();
        }
        
        public static async Task PreDepthFirstTraverseAsync<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode, Task<bool>> onTraverseAsync)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }
            
            await _DepthFirstTraverseAsync(true, tree, default, (TNode)tree.Root, (_, node) => onTraverseAsync(node));
        }
        
        public static void PreDepthFirstTraverse<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode?, TNode, bool> onTraverse)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }
            
            _DepthFirstTraverseAsync(true, tree, default, (TNode)tree.Root, (parentNode, node) => Task.FromResult(onTraverse(parentNode, node)))
                .Wait();
        }
        
        public static async Task PreDepthFirstTraverseAsync<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode?, TNode, Task<bool>> onTraverseAsync)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }
        
            await _DepthFirstTraverseAsync(true, tree, default, (TNode)tree.Root, onTraverseAsync);
        }
        
        public static void PostDepthFirstTraverse<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode, bool> onTraverse)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }
        
            _DepthFirstTraverseAsync(false, tree, default, (TNode)tree.Root, (_, node) => Task.FromResult(onTraverse(node)))
                .Wait();
        }
        
        public static async Task PostDepthFirstTraverseAsync<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode, Task<bool>> onTraverseAsync)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }
            
            await _DepthFirstTraverseAsync(false, tree, default, (TNode)tree.Root, (_, node) => onTraverseAsync(node));
        }
        
        public static void PostDepthFirstTraverse<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode?, TNode, bool> onTraverse)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }
            
            _DepthFirstTraverseAsync(false, tree, default, (TNode)tree.Root, (parentNode, node) => Task.FromResult(onTraverse(parentNode, node)))
                .Wait();
        }
        
        public static async Task PostDepthFirstTraverseAsync<TNode, TData>(this IReadOnlyTree<TData> tree,
            Func<TNode?, TNode, Task<bool>> onTraverseAsync)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (tree.Root == null)
            {
                return;
            }
        
            await _DepthFirstTraverseAsync(false, tree, default, (TNode)tree.Root, onTraverseAsync);
        }
        
        private static void _ConvertTo<TSourceData, TTargetData>(IReadOnlyTree<TSourceData> sourceTree, IReadOnlyNode sourceParent, ITree<TTargetData> targetTree, IReadOnlyNode<TTargetData> targetParent, Func<TSourceData, TTargetData> converter)
        {
            foreach (var sourceChild in sourceTree.GetChildren(sourceParent))
            {
                var targetChild = targetTree.AddChild(targetParent, converter(sourceChild.Data));
                _ConvertTo(sourceTree, sourceChild, targetTree, targetChild, converter);
            }
        }
        
        private static async Task _DepthFirstTraverseAsync<TNode, TData>(bool pre, IReadOnlyTree<TData> tree, TNode? parentNode, TNode node, Func<TNode?, TNode, Task<bool>> onTraverseAsync)
            where TNode : class, IReadOnlyNode<TData>
        {
            if (pre && !await onTraverseAsync(parentNode, node))
            {
                return;
            }
        
            foreach (var childNode in tree.GetChildren(node))
            {
                await _DepthFirstTraverseAsync(pre, tree, node, (TNode)childNode, onTraverseAsync);
            }

            if (!pre)
            {
                await onTraverseAsync(parentNode, node);
            }
        }
    }
}


#nullable disable
#endif