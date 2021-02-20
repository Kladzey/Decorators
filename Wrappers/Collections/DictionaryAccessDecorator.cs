using System;
using System.Collections;
using System.Collections.Generic;

namespace Kladzey.Wrappers.Collections
{
    /// <summary>
    /// Allows to control access to internal dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class DictionaryAccessDecorator<TKey, TValue> :
        BaseAccessDecorator,
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dictionary;

        public DictionaryAccessDecorator(IDictionary<TKey, TValue> dictionary, Func<bool> isEnabledFunc) :
            base(isEnabledFunc)
        {
            this.dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public int Count
        {
            get
            {
                EnsureEnabled();
                return dictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                EnsureEnabled();
                return dictionary.IsReadOnly;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                EnsureEnabled();
                return dictionary.Keys;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        public ICollection<TValue> Values
        {
            get
            {
                EnsureEnabled();
                return dictionary.Values;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public TValue this[TKey key]
        {
            get
            {
                EnsureEnabled();
                return dictionary[key];
            }
            set
            {
                EnsureEnabled();
                dictionary[key] = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            EnsureEnabled();
            dictionary.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            EnsureEnabled();
            dictionary.Add(item);
        }

        public void Clear()
        {
            EnsureEnabled();
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            EnsureEnabled();
            return dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            EnsureEnabled();
            return dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            EnsureEnabled();
            dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            EnsureEnabled();
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            EnsureEnabled();
            return dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            EnsureEnabled();
            return dictionary.Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            EnsureEnabled();
            return dictionary.TryGetValue(key, out value);
        }
    }
}
