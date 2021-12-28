using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Decoherence.SystemExtensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null && action != null)
            {
                foreach (var item in enumerable)
                {
                    action(item);
                }
            }
        }

        public static int FindIndex<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var i = 0;
            foreach (var item in enumerable)
            {
                if (predicate(item))
                {
                    return i;
                }
                ++i;
            }

            return -1;
        }
    }
}
