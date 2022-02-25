using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Decoherence
{
#if HIDE_DECOHERENCE
    internal static class DirUtil
#else
    public static class DirUtil
#endif
    {
        public static bool IsDirEmpty(string dirPath)
        {
            ThrowUtil.ThrowIfDirectoryNotFound(dirPath);

            return (Directory.GetFiles(dirPath).Length + Directory.GetDirectories(dirPath).Length) <= 0;
        }
        
        /// <summary>
        /// 拷贝目录
        /// </summary>
        /// <param name="limitExts"> 约束文件后缀，如果有约束则只拷贝约束限定的文件，没有任何约束即为拷贝目录内所有文件 </param>
        public static void CopyDir(string sourDir, string destDir, bool recursive, params string[] limitExts)
        {
            if (!Directory.Exists(sourDir))
            {
                throw new IOException(string.Format("Source dir '{0}' isn't exist!", sourDir));
            }

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            CopyDirImpl(new DirectoryInfo(sourDir), destDir, recursive, limitExts);
        }

        /// <summary>
        /// 尝试创建目录，会创建所有需要的父级目录
        /// </summary>
        /// <param name="path"> 路径 </param>
        /// <param name="force"> 是否覆盖，如果覆盖，则当路径中存在文件时会删除它再创建需要的文件夹。 </param>
        /// <returns></returns>
        public static bool TryCreateDir(string dirPath, bool force = false)
        {
            var create = false;
            var dirs = dirPath.Split('/', '\\');
            var rootPath = Path.GetPathRoot(dirPath);
            for (int i = string.IsNullOrEmpty(rootPath) ? 0 : 1; i < dirs.Length; ++i)
            {
                var dir = dirs[i];
                var fullPathDir = rootPath + dir;
                var state = PathUtil.GetPathState(fullPathDir);
                if (state == PathState.File)
                {
                    if (force)
                    {
                        PathUtil.TryDeletePath(fullPathDir);
                    }
                    else
                    {
                        throw new IOException("a file in the dirPath!");
                    }
                }

                if (state == PathState.None)
                {
                    Directory.CreateDirectory(fullPathDir);
                    create = true;
                }

                rootPath += dir + Path.DirectorySeparatorChar;
            }

            return create;
        }

        /// <summary>
        /// 尝试创建路径需要的所有父级目录
        /// </summary>
        /// <param name="path"> 路径 </param>
        /// <param name="force"> 是否覆盖，如果覆盖，则当路径中存在文件时会删除它再创建需要的文件夹。 </param>
        /// <returns></returns>
        public static bool TryCreateParentDir(string path, bool force = false)
        {
            var dirName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dirName) && dirName != PathUtil.directorySeparator)
            {
                return TryCreateDir(dirName, force);
            }
            return false;
        }

        public static void WalkDir(string dir, bool recursive, Action<string> action, string searchPattern = null)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException(string.Format("Dir '{0}' isn't exist!", dir));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            WalkDirImpl(new DirectoryInfo(dir), dir, recursive,
                path =>
                {
                    action?.Invoke(path);
                    return true;
                }, searchPattern);
        }

        public static void WalkDir(string dir, bool recursive, Func<string, bool> func, string searchPattern = null)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException(string.Format("Dir '{0}' isn't exist!", dir));
            }
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            WalkDirImpl(new DirectoryInfo(dir), dir, recursive, func, searchPattern);
        }

        private static void CopyDirImpl(DirectoryInfo curSourDir, string destDir, bool recursive, string[] limitExts)
        {
            foreach (var fi in curSourDir.GetFiles())
            {
                bool canCopy = true;
                if (limitExts != null && limitExts.Length > 0)
                {
                    canCopy = false;
                    for (int i = 0; i < limitExts.Length; ++i)
                    {
                        if (fi.Extension == limitExts[i])
                        {
                            canCopy = true;
                            break;
                        }
                    }
                }

                if (canCopy)
                {
                    File.Copy(fi.FullName, Path.Combine(destDir, fi.Name));
                }
            }

            if (recursive)
            {
                foreach (var di in curSourDir.GetDirectories())
                {
                    var childDirName = Path.Combine(destDir, di.Name);
                    Directory.CreateDirectory(childDirName);
                    CopyDirImpl(di, childDirName, recursive, limitExts);
                }
            }
        }

        private static void WalkDirImpl(DirectoryInfo curSourDir, string destDir, bool recursive, Func<string, bool> func, string searchPattern)
        {
            var fis = searchPattern == null ? curSourDir.GetFiles() : curSourDir.GetFiles(searchPattern);
            foreach (var fi in fis)
            {
                func(Path.Combine(destDir, fi.Name));
            }

            var dis = searchPattern == null ? curSourDir.GetDirectories() : curSourDir.GetDirectories(searchPattern);
            foreach (var di in dis)
            {
                bool into = func(Path.Combine(destDir, di.Name));
                if (recursive && into)
                {
                    var childDirName = Path.Combine(destDir, di.Name);
                    WalkDirImpl(di, childDirName, recursive, func, searchPattern);
                }
            }
        }
    }
}
