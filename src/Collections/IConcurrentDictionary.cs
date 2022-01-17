#if !NET35

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Decoherence
{
    public interface IConcurrentDictionary<TKey, TValue> : 
        ICollection<KeyValuePair<TKey, TValue>>,
        ICollection,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>, 
        IEnumerable,
        IDictionary<TKey, TValue>,
        IDictionary,
        IReadOnlyDictionary<TKey, TValue>
    {
        TValue AddOrUpdate<TArg>(TKey key, Func<TKey, TArg, TValue> addValueFactory, Func<TKey, TValue, TArg, TValue> updateValueFactory, TArg factoryArgument);
        TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument);

        bool TryAdd(TKey key, TValue value);
        bool TryRemove(TKey key, out TValue value);
        bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue);
    }
}

#endif