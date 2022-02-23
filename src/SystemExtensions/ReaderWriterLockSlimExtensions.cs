using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Decoherence.SystemExtensions
{
#if HIDE_DECOHERENCE
    internal static class ReaderWriterLockSlimExtensions
#else
    public static class ReaderWriterLockSlimExtensions
#endif
    {
        public static void WriteDo(this ReaderWriterLockSlim lockSlim, Action action)
        {
            lockSlim.EnterWriteLock();
            try
            {
                action?.Invoke();
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public static void ReadDo(this ReaderWriterLockSlim lockSlim, Action action)
        {
            lockSlim.EnterReadLock();
            try
            {
                action?.Invoke();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }
    }
}
