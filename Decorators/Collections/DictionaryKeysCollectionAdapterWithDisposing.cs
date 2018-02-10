using System;
using System.Collections.Generic;

namespace Kladzey.Decorators.Collections
{
    /// <summary>
    /// The variation of <see cref="DictionaryKeysCollectionAdapter{TKey, TValue}"/> that disposes values on remove.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The <see cref="IDisposable"/> type of values in the dictionary.</typeparam>
    public class DictionaryKeysCollectionAdapterWithDisposing<TKey, TValue> : DictionaryKeysCollectionAdapter<TKey, TValue> where TValue : IDisposable
    {
        public DictionaryKeysCollectionAdapterWithDisposing(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> valueFabric) : base(dictionary, valueFabric)
        {
        }

        public override void Clear()
        {
            foreach (var item in Dictionary.Values)
            {
                item.Dispose();
            }
            Dictionary.Clear();
        }

        public override bool Remove(TKey item)
        {
            if (!Dictionary.TryGetValue(item, out var value))
            {
                return false;
            }
            var removeResult = Dictionary.Remove(item);
            if (removeResult)
            {
                value?.Dispose();
            }
            return removeResult;
        }
    }
}
