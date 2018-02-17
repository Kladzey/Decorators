using System;
using System.Collections;
using System.Collections.Generic;

namespace Kladzey.Decorators.Collections
{
    /// <summary>
    /// More specified version of <see cref="CollectionAdpater{TInternal, TExternal}"/> that exposes dictionary keys.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class DictionaryKeysToCollectionAdapter<TKey, TValue> : ICollection<TKey>, IReadOnlyCollection<TKey>
    {
        protected readonly IDictionary<TKey, TValue> Dictionary;
        protected readonly Func<TKey, TValue> ValueFabric;

        public DictionaryKeysToCollectionAdapter(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> valueFabric)
        {
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            ValueFabric = valueFabric ?? throw new ArgumentNullException(nameof(valueFabric));
        }

        public int Count => Dictionary.Count;

        public bool IsReadOnly => Dictionary.IsReadOnly;

        public virtual void Add(TKey item)
        {
            Dictionary.Add(item, ValueFabric(item));
        }

        public virtual void Clear()
        {
            Dictionary.Clear();
        }

        public bool Contains(TKey item)
        {
            return Dictionary.ContainsKey(item);
        }

        public void CopyTo(TKey[] array, int arrayIndex)
        {
            Dictionary.Keys.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return Dictionary.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual bool Remove(TKey item)
        {
            return Dictionary.Remove(item);
        }
    }
}
