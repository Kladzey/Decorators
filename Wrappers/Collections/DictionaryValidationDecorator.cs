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
    public class DictionaryValidationDecorator<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dictionary;
        private readonly Func<TKey, TValue, bool> validationFunc;

        public DictionaryValidationDecorator(
            IDictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool> validationFunc)
        {
            this.dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            this.validationFunc = validationFunc ?? throw new ArgumentNullException(nameof(validationFunc));
        }

        public int Count => dictionary.Count;

        public bool IsAllItemsValid => dictionary.All(p => validationFunc(p.Key, p.Value));

        public bool IsReadOnly => dictionary.IsReadOnly;

        public ICollection<TKey> Keys => dictionary.Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        public ICollection<TValue> Values => dictionary.Values;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set
            {
                EnsureIsValid(key, value);
                dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            EnsureIsValid(item.Key, item.Value);
            dictionary.Add(item);
        }

        public void Add(TKey key, TValue value)
        {
            EnsureIsValid(key, value);
            dictionary.Add(key, value);
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

        private void EnsureIsValid(TKey key, TValue value)
        {
            if (!validationFunc(key, value))
            {
                throw new ArgumentException("Key/value pair is not valid.");
            }
        }
    }
}
