﻿using System;
using System.Diagnostics.Contracts;

namespace Kladzey.Decorators.Tests.TestUtils
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
