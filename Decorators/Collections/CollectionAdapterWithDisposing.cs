using System;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Decorators.Collections
{
    /// <summary>
    /// The variation of <see cref="CollectionAdapter{TInternal,TExternal}"/> that disposes values on remove.
    /// </summary>
    /// <typeparam name="TInternal">The <see cref="IDisposable"/> type of items in internal collection.</typeparam>
    /// <typeparam name="TExternal">The type of exposed items.</typeparam>
    public class CollectionAdapterWithDisposing<TInternal, TExternal> : CollectionAdapter<TInternal, TExternal> where TInternal : IDisposable
    {
        public CollectionAdapterWithDisposing(
            ICollection<TInternal> collection,
            Func<TInternal, TExternal> externalGetter,
            Func<TExternal, TInternal> internalFabric,
            IEqualityComparer<TExternal> equalityComparer) : base(collection, externalGetter, internalFabric, equalityComparer)
        {
        }

        public CollectionAdapterWithDisposing(ICollection<TInternal> collection, Func<TInternal, TExternal> externalGetter, Func<TExternal, TInternal> internalFabric)
           : this(collection, externalGetter, internalFabric, EqualityComparer<TExternal>.Default)
        {
        }

        public override void Add(TExternal item)
        {
            var internalValue = InternalFabric(item);
            try
            {
                Collection.Add(internalValue);
            }
            catch
            {
                internalValue?.Dispose();
                throw;
            }
        }

        public override void Clear()
        {
            foreach (var item in Collection)
            {
                item?.Dispose();
            }
            Collection.Clear();
        }

        public override bool Remove(TExternal item)
        {
            using (var enumerator =
                Collection
                    .Where(i => Comparer.Equals(ExternalGetter(i), item))
                    .GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return false;
                }

                var removeResult = Collection.Remove(enumerator.Current);
                if (removeResult)
                {
                    enumerator.Current?.Dispose();
                }
                return removeResult;
            }
        }
    }
}
