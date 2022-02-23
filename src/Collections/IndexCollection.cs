using Decoherence.SystemExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Decoherence
{
#if !NET35

    public class IndexCollection<TIndex, TValue> : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, IReadOnlyCollection<TValue>, IReadOnlyDictionary<TIndex, TValue>, ICollection, IDeserializationCallback, ISerializable
    {
        private readonly Dictionary<TIndex, TValue> mDic = new Dictionary<TIndex, TValue>();
        private readonly Func<TValue, TIndex> mIndexGetter;

        public TValue this[TIndex key] => mDic[key];

        public IEnumerable<TIndex> Keys => ((IReadOnlyDictionary<TIndex, TValue>)mDic).Keys;

        public IEnumerable<TValue> Values => ((IReadOnlyDictionary<TIndex, TValue>)mDic).Values;

        public int Count => mDic.Count;

        public bool IsSynchronized => ((ICollection)mDic).IsSynchronized;

        public object SyncRoot => ((ICollection)mDic).SyncRoot;

        public bool IsReadOnly => ((ICollection<TValue>)mDic).IsReadOnly;

        public IndexCollection(Func<TValue, TIndex> indexGetter)
        {
            ThrowUtil.ThrowIfArgumentNull(indexGetter, nameof(indexGetter));

            mIndexGetter = indexGetter;
        }

        public IndexCollection(Func<TValue, TIndex> indexGetter, IEnumerable<TValue> initValues) : this(indexGetter)
        {
            this.AddRange(initValues);
        }

        public void Add(TValue item)
        {
            mDic.Add(mIndexGetter(item), item);
        }

        public bool Remove(TValue item)
        {
            return mDic.Remove(mIndexGetter(item));
        }

        public void Clear()
        {
            mDic.Clear();
        }

        public bool Contains(TValue item)
        {
            return mDic.ContainsKey(mIndexGetter(item));
        }

        public bool ContainsKey(TIndex key)
        {
            return mDic.ContainsKey(key);
        }

        public IEnumerator GetEnumerator()
        {
            return mDic.Values.GetEnumerator();
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)mDic).CopyTo(array, index);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            mDic.GetObjectData(info, context);
        }

        public void OnDeserialization(object sender)
        {
            mDic.OnDeserialization(sender);
        }

        public bool TryGetValue(TIndex key, out TValue value)
        {
            return mDic.TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<TIndex, TValue>> IEnumerable<KeyValuePair<TIndex, TValue>>.GetEnumerator()
        {
            throw new InvalidOperationException();
        }
    }

#endif
}
