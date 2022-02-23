using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decoherence
{
#if HIDE_DECOHERENCE
    internal static class NetUtil
#else
    public static class NetUtil
#endif
    {
        public static bool SameIp(string ip1, string ip2)
        {
            if (string.Compare(ip1, "localhost", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ip1 = "127.0.0.1";
            }
            if (string.Compare(ip2, "localhost", StringComparison.OrdinalIgnoreCase) == 0)
            {
                ip2 = "127.0.0.1";
            }

            return string.Compare(ip1, ip2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static string GetUrlDirectoryName(string url)
        {
            return url.Substring(0, url.LastIndexOf('/'));
        }
        
        public static string GetUrlFileName(string url)
        {
            return url.Substring(url.LastIndexOf('/') + 1);
        }
    }
}
