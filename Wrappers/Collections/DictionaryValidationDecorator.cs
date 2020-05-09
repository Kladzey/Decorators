using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Wrappers.Collections
{
    /// <summary>
    /// Allows to forbid adding of invalid key/value pairs to dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class DictionaryValidationDecorator<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly Func<TKey, TValue, bool> _validationFunc;

        public DictionaryValidationDecorator(
            IDictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool> validationFunc)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            _validationFunc = validationFunc ?? throw new ArgumentNullException(nameof(validationFunc));
        }

        public int Count => _dictionary.Count;

        public bool IsAllItemsValid => _dictionary.All(p => _validationFunc(p.Key, p.Value));

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public ICollection<TKey> Keys => _dictionary.Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                EnsureIsValid(key, value);
                _dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            EnsureIsValid(item.Key, item.Value);
            _dictionary.Add(item);
        }

        public void Add(TKey key, TValue value)
        {
            EnsureIsValid(key, value);
            _dictionary.Add(key, value);
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

        private void EnsureIsValid(TKey key, TValue value)
        {
            if (!_validationFunc(key, value))
            {
                throw new ArgumentException("Key/value pair is not valid.");
            }
        }
    }
}
