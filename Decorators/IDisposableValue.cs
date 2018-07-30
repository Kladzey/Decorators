using System;

namespace Kladzey.Decorators
{
    public interface IDisposableValue<out T> : IDisposable
    {
        T Value { get; }
    }
}
