using System;

namespace Kladzey.Wrappers
{
    /// <summary>
    /// Base logic for access decorators.
    /// </summary>
    public abstract class BaseAccessDecorator
    {
        private readonly Func<bool> isEnabledFunc;

        protected BaseAccessDecorator(Func<bool> isEnabledFunc)
        {
            this.isEnabledFunc = isEnabledFunc ?? throw new ArgumentNullException(nameof(isEnabledFunc));
        }

        public bool IsEnabled => isEnabledFunc();

        protected void EnsureEnabled()
        {
            if (!isEnabledFunc())
            {
                throw new InvalidOperationException("Access to object is disabled.");
            }
        }
    }
}
