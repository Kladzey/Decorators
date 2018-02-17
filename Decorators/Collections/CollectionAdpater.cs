using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Decorators.Collections
{
    /// <summary>
    /// This adapter allows to expose one type collection as other type collection.
    /// </summary>
    /// <typeparam name="TInternal">The type of items in internal collection.</typeparam>
    /// <typeparam name="TExternal">The type of exposed items.</typeparam>
    public class CollectionAdpater<TInternal, TExternal> : ICollection<TExternal>, IReadOnlyCollection<TExternal>
    {
        protected readonly ICollection<TInternal> Collection;
        protected readonly IEqualityComparer<TExternal> Comparer;
        protected readonly Func<TInternal, TExternal> ExternalGetter;
        protected readonly Func<TExternal, TInternal> InternalFabric;

        public CollectionAdpater(
            ICollection<TInternal> collection,
            Func<TInternal, TExternal> externalGetter,
            Func<TExternal, TInternal> internalFabric,
            IEqualityComparer<TExternal> equalityComparer)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            ExternalGetter = externalGetter ?? throw new ArgumentNullException(nameof(externalGetter));
            InternalFabric = internalFabric ?? throw new ArgumentNullException(nameof(internalFabric));
            Comparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
        }

        public CollectionAdpater(ICollection<TInternal> collection, Func<TInternal, TExternal> externalGetter, Func<TExternal, TInternal> internalFabric)
            : this(collection, externalGetter, internalFabric, EqualityComparer<TExternal>.Default)
        {
        }

        public int Count => Collection.Count;

        public bool IsReadOnly => Collection.IsReadOnly;

        public virtual void Add(TExternal item)
        {
            Collection.Add(InternalFabric(item));
        }

        public virtual void Clear()
        {
            Collection.Clear();
        }

        public bool Contains(TExternal item)
        {
            return Collection.Any(i => Comparer.Equals(ExternalGetter(i), item));
        }

        public void CopyTo(TExternal[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (arrayIndex < 0 || arrayIndex > array.Length || Collection.Count + arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }
            var i = arrayIndex;
            foreach (var item in Collection)
            {
                array[i] = ExternalGetter(item);
                ++i;
            }
        }

        public IEnumerator<TExternal> GetEnumerator()
        {
            return Collection.Select(ExternalGetter).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual bool Remove(TExternal item)
        {
            var internalItemList = Collection
                .Where(i => Comparer.Equals(ExternalGetter(i), item))
                .Take(1)
                .ToList();
            if (internalItemList.Count == 0)
            {
                return false;
            }
            return Collection.Remove(internalItemList[0]);
        }
    }
}
