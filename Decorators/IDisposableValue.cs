using System;

namespace Kladzey.Decorators
{
    public interface IDisposableValue<T> : IDisposable
    {
        T Value { get; }
    }
}
