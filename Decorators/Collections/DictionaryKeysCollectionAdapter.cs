using System;
using System.Collections;
using System.Collections.Generic;

namespace Kladzey.Decorators.Collections
{
    public class DictionaryKeysCollectionAdapter<TKey, TValue> : ICollection<TKey>
    {
        private readonly IDictionary<TKey, TValue> _collectionImplementation;
        private readonly Func<TKey, TValue> _valueFabric;

        public DictionaryKeysCollectionAdapter(IDictionary<TKey, TValue> collectionImplementation, Func<TKey, TValue> valueFabric)
        {
            _collectionImplementation = collectionImplementation ?? throw new ArgumentNullException(nameof(collectionImplementation));
            _valueFabric = valueFabric ?? throw new ArgumentNullException(nameof(valueFabric));
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return _collectionImplementation.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey item)
        {
            _collectionImplementation.Add(item, _valueFabric(item));
        }

        public void Clear()
        {
            _collectionImplementation.Clear();
        }

        public bool Contains(TKey item)
        {
            return _collectionImplementation.ContainsKey(item);
        }

        public void CopyTo(TKey[] array, int arrayIndex)
        {
            _collectionImplementation.Keys.CopyTo(array, arrayIndex);
        }

        public bool Remove(TKey item)
        {
            return _collectionImplementation.Remove(item);
        }

        public int Count => _collectionImplementation.Count;

        public bool IsReadOnly => _collectionImplementation.IsReadOnly;
    }
}
