using System;
using System.Collections.Generic;
using System.Text;

namespace Decoherence
{
    public static class NetUtil
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
    }
}
