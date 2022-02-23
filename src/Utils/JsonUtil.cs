using System;
using System.IO;
using Newtonsoft.Json;

namespace Decoherence
{
#if HIDE_DECOHERENCE
    internal static class JsonUtil
#else
    public static class JsonUtil
#endif
    {
        public static void SerializeToFile(object jsonData, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            SerializeToFile(jsonData, fs);
        }

        public static void SerializeToFile(object jsonData, FileStream f)
        {
            using StreamWriter sw = new StreamWriter(f);
            
            JsonSerializer jsonSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
            };
            
            jsonSerializer.Serialize(sw, jsonData);
        }
        
        public static T DeserializeFromFile<T>(string filePath)
        {
            ThrowUtil.ThrowIfFileNotFound(filePath);

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return DeserializeFromFile<T>(fs);
        }
        
        public static T DeserializeFromFile<T>(FileStream f)
        {
            using StreamReader sr = new StreamReader(f);
            
            JsonSerializer jsonSerializer = new JsonSerializer();
            var obj = jsonSerializer.Deserialize(sr, typeof(T));
            if (obj == null)
            {
                throw new InvalidOperationException();
            }
            if (obj is not T ret)
            {
                throw new InvalidOperationException();
            }
            return ret;
        }
    }
}