using System;
using System.Threading;

namespace Kladzey.Decorators
{
    public abstract class BaseDisposable : IDisposable
    {
        private int _isDisposed;

        protected bool IsDisposed
        {
            get
            {
                Interlocked.MemoryBarrier();
                return _isDisposed != 0;
            }
        }

        public void Dispose()
        {
            var isDisposed = Interlocked.Exchange(ref _isDisposed, 1) != 0;
            if (isDisposed)
            {
                return;
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        protected void EnsureIsNotDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName, "The object is disposed.");
            }
        }
    }
}
