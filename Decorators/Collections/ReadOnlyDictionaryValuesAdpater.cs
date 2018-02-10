using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Decorators.Collections
{
    public class ReadOnlyDictionaryValuesAdpater<TKey, TValue, TValueInternal> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Func<TValueInternal, TValue> _externalGetter;
        private readonly IReadOnlyDictionary<TKey, TValueInternal> _readOnlyDictionary;

        public ReadOnlyDictionaryValuesAdpater(
            IReadOnlyDictionary<TKey, TValueInternal> readOnlyDictionary,
            Func<TValueInternal, TValue> externalGetter)
        {
            _readOnlyDictionary = readOnlyDictionary ?? throw new ArgumentNullException(nameof(readOnlyDictionary));
            _externalGetter = externalGetter ?? throw new ArgumentNullException(nameof(externalGetter));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _readOnlyDictionary
                .Select(p => new KeyValuePair<TKey, TValue>(p.Key, _externalGetter(p.Value))).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _readOnlyDictionary.Count;

        public bool ContainsKey(TKey key)
        {
            return _readOnlyDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_readOnlyDictionary.TryGetValue(key, out var internalValue))
            {
                value = default(TValue);
                return false;
            }
            value = _externalGetter(internalValue);
            return true;
        }

        public TValue this[TKey key] => _externalGetter(_readOnlyDictionary[key]);

        public IEnumerable<TKey> Keys => _readOnlyDictionary.Keys;

        public IEnumerable<TValue> Values => _readOnlyDictionary.Values.Select(_externalGetter);
    }
}
