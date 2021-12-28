using System;
using System.Collections.Generic;
using System.Text;

namespace Decoherence.SystemExtensions
{
    public static class CollectionExtensions
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
            ThrowHelper.ThrowIfArgumentNull(collection, nameof(collection));
            
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
            ThrowHelper.ThrowIfArgumentNull(collection, nameof(collection));

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
