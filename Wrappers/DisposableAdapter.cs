using System;

namespace Kladzey.Wrappers
{
    public sealed class DisposableAdapter<TValue> : IDisposable
    {
        private readonly Action<TValue> _onDispose;

        public DisposableAdapter(TValue value, Action<TValue> onDispose)
        {
            Value = value;
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        public TValue Value { get; }

        public void Dispose()
        {
            _onDispose(Value);
        }
    }
}
