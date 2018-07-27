using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Decorators.Collections
{
    public class ReadOnlyDictionaryValuesAdpater<TKey, TValue, TValueInternal> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Func<TValueInternal, TValue> _externalGetter;
        private readonly IDictionary<TKey, TValueInternal> _dictionary;

        public ReadOnlyDictionaryValuesAdpater(
            IDictionary<TKey, TValueInternal> dictionary,
            Func<TValueInternal, TValue> externalGetter)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            _externalGetter = externalGetter ?? throw new ArgumentNullException(nameof(externalGetter));
        }

        public int Count => _dictionary.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

        public IEnumerable<TKey> Keys => _dictionary.Keys;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                var keys = _dictionary.Keys;
                return keys.IsReadOnly ? keys : keys.ToList();
            }
        }

        public IEnumerable<TValue> Values => _dictionary.Values.Select(_externalGetter);

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values.ToList();

        public TValue this[TKey key]
        {
            get => _externalGetter(_dictionary[key]);
            set => throw new InvalidOperationException();
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new InvalidOperationException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new InvalidOperationException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!_dictionary.TryGetValue(item.Key, out var value))
            {
                return false;
            }
            return EqualityComparer<TValue>.Default.Equals(item.Value, _externalGetter(value));
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary
                .Select(p => new KeyValuePair<TKey, TValue>(p.Key, _externalGetter(p.Value))).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new InvalidOperationException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_dictionary.TryGetValue(key, out var internalValue))
            {
                value = default(TValue);
                return false;
            }
            value = _externalGetter(internalValue);
            return true;
        }
    }
}
