using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
#if !NET35
using System.Threading.Tasks;
#endif

namespace Decoherence.SystemExtensions
{
#if HIDE_DECOHERENCE
    internal static class WaitHandleExtensions
#else
    public static class WaitHandleExtensions
#endif
    {
#if !NET35
        public static Task WaitOneAsync(this WaitHandle waitHandle)
        {
            if (waitHandle == null)
                throw new ArgumentNullException("waitHandle");

            var tcs = new TaskCompletionSource<bool>();
            var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle,
                delegate { tcs.TrySetResult(true); }, null, -1, true);
            var t = tcs.Task;
            t.ContinueWith((antecedent) => rwh.Unregister(null));
            return t;
        }
#endif
    }
}
