using System;
using System.Collections;
using System.Collections.Generic;

namespace Kladzey.Decorators.Collections
{
    /// <summary>
    /// Allows to control access to internal dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class DictionaryAccessDecorator<TKey, TValue> : BaseAccessDecorator, IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        public DictionaryAccessDecorator(IDictionary<TKey, TValue> dictionary, Func<bool> isEnabledFunc) : base(isEnabledFunc)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public int Count
        {
            get
            {
                EnsureEnabled();
                return _dictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                EnsureEnabled();
                return _dictionary.IsReadOnly;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                EnsureEnabled();
                return _dictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                EnsureEnabled();
                return _dictionary.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                EnsureEnabled();
                return _dictionary[key];
            }
            set
            {
                EnsureEnabled();
                _dictionary[key] = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            EnsureEnabled();
            _dictionary.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            EnsureEnabled();
            _dictionary.Add(item);
        }

        public void Clear()
        {
            EnsureEnabled();
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            EnsureEnabled();
            return _dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            EnsureEnabled();
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            EnsureEnabled();
            _dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            EnsureEnabled();
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            EnsureEnabled();
            return _dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            EnsureEnabled();
            return _dictionary.Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            EnsureEnabled();
            return _dictionary.TryGetValue(key, out value);
        }
    }
}
