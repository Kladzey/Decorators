using System;

namespace Kladzey.Decorators
{
    public class DisposableAdapter<T> : BaseDisposable, IDisposableValue<T>
    {
        private readonly Action _onDispose;

        public DisposableAdapter(T value, Action onDispose)
        {
            Value = value;
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        public delegate void DisposeDelegate(bool disposing);

        public T Value { get; }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            _onDispose();
        }
    }
}
