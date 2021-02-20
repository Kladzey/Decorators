using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Wrappers.Collections
{
    public class ReadOnlyDictionaryValuesAdapter<TKey, TValue, TValueInternal> :
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValueInternal> dictionary;
        private readonly Func<TValueInternal, TValue> externalGetter;

        public ReadOnlyDictionaryValuesAdapter(
            IDictionary<TKey, TValueInternal> dictionary,
            Func<TValueInternal, TValue> externalGetter)
        {
            this.dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            this.externalGetter = externalGetter ?? throw new ArgumentNullException(nameof(externalGetter));
        }

        public int Count => dictionary.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

        public IEnumerable<TKey> Keys => dictionary.Keys;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                var keys = dictionary.Keys;
                return keys.IsReadOnly ? keys : keys.ToList();
            }
        }

        public IEnumerable<TValue> Values => dictionary.Values.Select(externalGetter);

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values.ToList();

        public TValue this[TKey key]
        {
            get => externalGetter(dictionary[key]);
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
            return dictionary.TryGetValue(item.Key, out var value) &&
                   EqualityComparer<TValue>.Default.Equals(item.Value, externalGetter(value));
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary
                .Select(p => new KeyValuePair<TKey, TValue>(p.Key, externalGetter(p.Value))).GetEnumerator();
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
            if (!dictionary.TryGetValue(key, out var internalValue))
            {
                value = default!;
                return false;
            }

            value = externalGetter(internalValue);
            return true;
        }
    }
}
