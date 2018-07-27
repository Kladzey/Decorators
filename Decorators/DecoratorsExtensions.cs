using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kladzey.Decorators.Collections;

namespace Kladzey.Decorators
{
    public static class DecoratorsExtensions
    {
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionay<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary as IReadOnlyDictionary<TKey, TValue> ?? new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        public static ICollection<TKey> WrapKeysToCollection<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<TKey, TValue> valueFabric)
        {
            return new DictionaryKeysToCollectionAdapter<TKey, TValue>(dictionary, valueFabric);
        }

        public static ICollection<TKey> WrapKeysToCollectionWithDisposing<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<TKey, TValue> valueFabric) where TValue : IDisposable
        {
            return new DictionaryKeysToCollectionAdapterWithDisposing<TKey, TValue>(dictionary, valueFabric);
        }

        public static ICollection<TExternal> WrapToCollection<TInternal, TExternal>(
            this ICollection<TInternal> collection,
            Func<TInternal, TExternal> externalGetter,
            Func<TExternal, TInternal> internalFabric)
        {
            return new CollectionAdpater<TInternal, TExternal>(collection, externalGetter, internalFabric);
        }

        public static ICollection<TExternal> WrapToCollection<TInternal, TExternal>(
            this ICollection<TInternal> collection,
            Func<TInternal, TExternal> externalGetter,
            Func<TExternal, TInternal> internalFabric,
            IEqualityComparer<TExternal> equalityComparer)
        {
            return new CollectionAdpater<TInternal, TExternal>(collection, externalGetter, internalFabric, equalityComparer);
        }

        public static ICollection<TExternal> WrapToCollectionWithDisposing<TInternal, TExternal>(
            this ICollection<TInternal> collection,
            Func<TInternal, TExternal> externalGetter,
            Func<TExternal, TInternal> internalFabric) where TInternal : IDisposable
        {
            return new CollectionAdpaterWithDisposing<TInternal, TExternal>(collection, externalGetter, internalFabric);
        }

        public static ICollection<TExternal> WrapToCollectionWithDisposing<TInternal, TExternal>(
            this ICollection<TInternal> collection,
            Func<TInternal, TExternal> externalGetter,
            Func<TExternal, TInternal> internalFabric,
            IEqualityComparer<TExternal> equalityComparer) where TInternal : IDisposable
        {
            return new CollectionAdpaterWithDisposing<TInternal, TExternal>(collection, externalGetter, internalFabric, equalityComparer);
        }

        public static IDictionary<TKey, TValue> WrapToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        public static IReadOnlyDictionary<TKey, TValue> WrapToReadOnlyDictionary<TKey, TValue, TValueInternal>(this IDictionary<TKey, TValueInternal> dictionary, Func<TValueInternal, TValue> externalGetter)
        {
            return new ReadOnlyDictionaryValuesAdpater<TKey, TValue, TValueInternal>(dictionary, externalGetter);
        }

        public static IDictionary<TKey, TValue> WrapWithAccessControl<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<bool> isEnabledFunc)
        {
            return new DictionaryAccessDecorator<TKey, TValue>(dictionary, isEnabledFunc);
        }

        public static IDictionary<TKey, TValue> WrapWithDefaultValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new DictionaryDefaultValueDecorator<TKey, TValue>(dictionary);
        }

        public static IDictionary<TKey, TValue> WrapWithValidation<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, TValue, bool> validationFunc)
        {
            return new DictionaryValidationDecorator<TKey, TValue>(dictionary, validationFunc);
        }

        public static DisposableAdapter<TValue> WrapToDisposable<TValue>(this TValue value, Action<TValue> onDispose)
        {
            return new DisposableAdapter<TValue>(value, onDispose);
        }
    }
}
