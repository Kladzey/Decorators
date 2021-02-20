using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Wrappers.Collections
{
    public class DictionaryDefaultValueDecorator<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IEqualityComparer<TValue> comparer;
        private readonly TValue defaultValue;
        private readonly IDictionary<TKey, TValue> dictionary;

        public DictionaryDefaultValueDecorator(
            IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TValue> comparer,
            TValue defaultValue)
        {
            this.dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            this.defaultValue = defaultValue;
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            var keysToRemove = this.dictionary
                .Where(p => this.comparer.Equals(p.Value, this.defaultValue))
                .Select(p => p.Key)
                .ToList();
            foreach (var key in keysToRemove)
            {
                this.dictionary.Remove(key);
            }
        }

        public DictionaryDefaultValueDecorator(IDictionary<TKey, TValue> dictionary, TValue defaultValue) :
            this(dictionary, EqualityComparer<TValue>.Default, defaultValue)
        {
        }

        public int Count => dictionary.Count;

        public bool IsReadOnly => dictionary.IsReadOnly;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        public ICollection<TKey> Keys => dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public ICollection<TValue> Values => dictionary.Values;

        public TValue this[TKey key]
        {
            get => dictionary.TryGetValue(key, out var value) ? value : defaultValue;
            set
            {
                if (comparer.Equals(value, defaultValue))
                {
                    dictionary.Remove(key);
                }
                else
                {
                    dictionary[key] = value;
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            if (comparer.Equals(value, defaultValue))
            {
                if (dictionary.ContainsKey(key))
                {
                    throw new ArgumentException("Duplicate key.", nameof(key));
                }
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Remove(item);
        }

        public bool Remove(TKey key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }
    }
}
