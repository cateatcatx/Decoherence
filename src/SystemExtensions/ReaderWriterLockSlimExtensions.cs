using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Decoherence.SystemExtensions
{
    public static class ReaderWriterLockSlimExtensions
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
