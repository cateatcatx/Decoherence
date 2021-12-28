using System;
using System.Collections.Generic;
using System.Text;

namespace Decoherence.SystemExtensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
#if NET35
            return string.IsNullOrEmpty(str) || str.Trim() == "";
#else
            return string.IsNullOrWhiteSpace(str);
#endif
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
