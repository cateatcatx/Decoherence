using System;
using System.Collections.Generic;
using System.Text;

namespace Decoherence.SystemExtensions
{
#if HIDE_DECOHERENCE
    internal static class CollectionExtensions
#else
    public static class CollectionExtensions
#endif
    {
#if NET35
        public static bool IsEmpty<T>(this ICollection<T> collection)
#else
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection)
#endif
        {
            return collection == null || collection.Count <= 0;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> adds)
        {
            ThrowUtil.ThrowIfArgumentNull(collection, nameof(collection));
            
            if (adds != null)
            {
                foreach (var item in adds)
                {
                    collection.Add(item);
                }
            }
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> removes)
        {
            ThrowUtil.ThrowIfArgumentNull(collection, nameof(collection));

            if (removes != null)
            {
                foreach (var item in removes)
                {
                    collection.Remove(item);
                }
            }
        }
    }
}
