using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Decoherence.SystemExtensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccess(this HttpStatusCode code)
        {
            return 200 <= (int)code && (int)code < 300;
        }
    }
}
