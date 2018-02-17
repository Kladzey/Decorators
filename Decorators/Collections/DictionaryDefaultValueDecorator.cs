using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Decorators.Collections
{
    public class DictionaryDefaultValueDecorator<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IEqualityComparer<TValue> _comparer;
        private readonly TValue _defaultValue;
        private readonly IDictionary<TKey, TValue> _dictionary;

        public DictionaryDefaultValueDecorator(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TValue> comparer, TValue defaultValue)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            _defaultValue = defaultValue;
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            var keysToRemove = _dictionary
                .Where(p => _comparer.Equals(p.Value, _defaultValue))
                .Select(p => p.Key)
                .ToList();
            foreach (var key in keysToRemove)
            {
                _dictionary.Remove(key);
            }
        }

        public DictionaryDefaultValueDecorator(IDictionary<TKey, TValue> dictionary) :
            this(dictionary, EqualityComparer<TValue>.Default, default(TValue))
        {
        }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        public ICollection<TKey> Keys => _dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public ICollection<TValue> Values => _dictionary.Values;

        public TValue this[TKey key]
        {
            get => _dictionary.TryGetValue(key, out var value) ? value : _defaultValue;
            set
            {
                if (_comparer.Equals(value, _defaultValue))
                {
                    _dictionary.Remove(key);
                }
                else
                {
                    _dictionary[key] = value;
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            if (!_comparer.Equals(value, _defaultValue))
            {
                _dictionary.Add(key, value);
            }
            else if (_dictionary.ContainsKey(key))
            {
                throw new ArgumentException("Duplicate key.", nameof(key));
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Remove(item);
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }
    }
}
