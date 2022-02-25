using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Decoherence
{
#if HIDE_DECOHERENCE
    internal static class HttpUtil
#else
    public static class HttpUtil
#endif
    {
        public static string GenUrl(string url, Dictionary<string, object> kvs)
        {
            ThrowUtil.ThrowIfArgumentNullOrWhiteSpace(url, nameof(url));

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

        public static string Post(string url, string contentType, string content)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = contentType;

            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Flush();
                reqStream.Close();
            }

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream stream = resp.GetResponseStream()!;
                //获取响应内容
                using StreamReader reader = new (stream, Encoding.UTF8);
                result = reader.ReadToEnd();

                return result;
            }
            catch (WebException ex) when(ex.Response != null)
            {
                Stream stream = ex.Response.GetResponseStream()!;
                
                //获取响应内容
                using StreamReader reader = new (stream, Encoding.UTF8);
                result = reader.ReadToEnd();
                Console.Error.WriteLine(result);
                return result;
            }
        }
    }
}


