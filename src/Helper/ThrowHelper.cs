using Decoherence.SystemExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decoherence
{
    public static class ThrowHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfArgumentNull(object arg, string argName = default, string message = default)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName, message);
            }
        }

        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfArgumentNullOrWhiteSpace(string arg, string paramName = null, string message = null)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentException(message ?? "Value cannot be null or whitespace.", paramName);
            }
        }
        
        public static void ThrowIfArgument(bool condition, string paramName = null, string message = null)
        {
            if (condition)
            {
                throw new ArgumentException(message, paramName);
            }
        }

        public static void ThrowIfArgument(Func<bool> condition, string paramName = null, string message = null)
        {
            var ret = condition?.Invoke();
            if (ret == null || ret.Value)
            {
                throw new ArgumentException(message, paramName);
            }
        }

        public static void ThrowIfPathNotFound(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new PathNotFoundException($"The path of '{path}' contains neither files nor directories.");
            }
        }

        /// <exception cref="FileNotFoundException"></exception>
        public static void ThrowIfFileNotFound(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file could not be found under the path of '{filePath}'.", filePath);
            }
        }

        public static void ThrowIfDirectoryNotFound(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                throw new DirectoryNotFoundException($"The directory could not be found under the path of '{dirPath}'.");
            }
        }

        
    }
}
