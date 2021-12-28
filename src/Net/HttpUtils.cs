
using System.Collections.Generic;
using System.Text;

namespace Decoherence
{
    public static class HttpUtils
    {

        public static string GenUrl(string url, Dictionary<string, object> kvs)
//#endif
        {
            ThrowHelper.ThrowIfArgumentNullOrWhiteSpace(url, nameof(url));

            if (kvs == null)
            {
                return url;
            }

            StringBuilder sb = new StringBuilder(url);

            sb.Append("?");

            foreach (var kv in kvs)
            {
                sb.Append($"{kv.Key}={kv.Value}&");
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}


