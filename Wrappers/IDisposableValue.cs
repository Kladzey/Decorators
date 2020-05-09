using System;

namespace Kladzey.Wrappers
{
    public interface IDisposableValue<out T> : IDisposable
    {
        T Value { get; }
    }
}
