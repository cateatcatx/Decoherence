using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Decoherence.SystemExtensions
{
#if HIDE_DECOHERENCE
    internal static class HttpStatusCodeExtensions
#else
    public static class HttpStatusCodeExtensions
#endif
    {
        public static bool IsSuccess(this HttpStatusCode code)
        {
            return 200 <= (int)code && (int)code < 300;
        }
    }
}
