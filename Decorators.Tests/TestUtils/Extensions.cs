using System;
using System.Diagnostics.Contracts;

namespace Decorators.Tests.TestUtils
{
    public static class Extensions
    {
        [Pure]
        public static Action InvokingFunc<T, Y>(this T subject, Func<T, Y> func)
        {
            return () => func(subject);
        }
    }
}
