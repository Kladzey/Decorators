using System;

namespace Kladzey.Decorators
{
    /// <summary>
    /// Base logic for access decorators.
    /// </summary>
    public abstract class BaseAccessDecorator
    {
        private readonly Func<bool> _isEnabledFunc;

        protected BaseAccessDecorator(Func<bool> isEnabledFunc)
        {
            _isEnabledFunc = isEnabledFunc ?? throw new ArgumentNullException(nameof(isEnabledFunc));
        }

        public bool IsEnabled => _isEnabledFunc();

        protected void EnsureEnabled()
        {
            if (!_isEnabledFunc())
            {
                throw new InvalidOperationException("Access to object is disabled.");
            }
        }
    }
}
