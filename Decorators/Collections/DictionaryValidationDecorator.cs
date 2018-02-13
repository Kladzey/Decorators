using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Decorators.Collections
{
    public class DictionaryValidationDecorator<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly Func<TKey, TValue, bool> _validationFunc;

        public DictionaryValidationDecorator(
            IDictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool> validationFunc)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            _validationFunc = validationFunc ?? throw new ArgumentNullException(nameof(validationFunc));
            if (_dictionary.Any(p => !_validationFunc(p.Key, p.Value)))
            {
                throw new ArgumentException("Dictionary contains not valid key/value pairs.", nameof(dictionary));
            }
        }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                if (!_validationFunc(key, value))
                {
                    throw new ArgumentException(nameof(value));
                }
                _dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (!_validationFunc(item.Key, item.Value))
            {
                throw new ArgumentException(nameof(item));
            }
            _dictionary.Add(item);
        }

        public void Add(TKey key, TValue value)
        {
            if (!_validationFunc(key, value))
            {
                throw new ArgumentException(nameof(value));
            }
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
    }
}
