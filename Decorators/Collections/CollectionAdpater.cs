using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kladzey.Decorators.Collections
{
    /// <summary>
    ///  This adapter allows to expose one type collection as other type collection.
    /// </summary>
    /// <typeparam name="TInternal">The type of items in internal collection.</typeparam>
    /// <typeparam name="TExternal">The type of exposed items.</typeparam>
    public class CollectionAdpater<TInternal, TExternal> : ICollection<TExternal>
    {
        private readonly ICollection<TInternal> _collection;
        private readonly IEqualityComparer<TExternal> _comparer;
        private readonly bool _disposeOnRemove;
        private readonly Func<TInternal, TExternal> _externalGetter;
        private readonly Func<TExternal, TInternal> _internalFabric;

        public CollectionAdpater(
            ICollection<TInternal> collection,
            Func<TInternal, TExternal> externalGetter,
            Func<TExternal, TInternal> internalFabric,
            IEqualityComparer<TExternal> equalityComparer,
            bool disposeOnRemove)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _externalGetter = externalGetter ?? throw new ArgumentNullException(nameof(externalGetter));
            _internalFabric = internalFabric ?? throw new ArgumentNullException(nameof(internalFabric));
            _comparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
            _disposeOnRemove = disposeOnRemove;
        }

        public CollectionAdpater(ICollection<TInternal> collection, Func<TInternal, TExternal> externalGetter, Func<TExternal, TInternal> internalFabric, bool disposeOnRemove)
            : this(collection, externalGetter, internalFabric, EqualityComparer<TExternal>.Default, disposeOnRemove)
        {
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        public void Add(TExternal item)
        {
            _collection.Add(_internalFabric(item));
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(TExternal item)
        {
            return _collection.Any(i => _comparer.Equals(_externalGetter(i), item));
        }

        public void CopyTo(TExternal[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (arrayIndex < 0 || arrayIndex > array.Length || _collection.Count + arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }
            var i = arrayIndex;
            foreach (var item in _collection)
            {
                array[i] = _externalGetter(item);
                ++i;
            }
        }

        public IEnumerator<TExternal> GetEnumerator()
        {
            return _collection.Select(_externalGetter).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(TExternal item)
        {
            var internalItemList = _collection
                .Where(i => _comparer.Equals(_externalGetter(i), item))
                .Take(1)
                .ToList();
            if (internalItemList.Count == 0)
            {
                return false;
            }
            var internalItem = internalItemList[0];
            var removeResult = _collection.Remove(internalItem);
            if (removeResult && _disposeOnRemove && internalItem is IDisposable disposable)
            {
                disposable.Dispose();
            }
            return removeResult;
        }
    }
}
