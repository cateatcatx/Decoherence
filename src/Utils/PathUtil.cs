using System;
using System.IO;
using System.Text;
using Decoherence.SystemExtensions;

namespace Decoherence
{
#if HIDE_DECOHERENCE
    internal enum PathState
#else
    public enum PathState
#endif
    {
        None, // 路径不含文件和目录
        File, // 路径存在文件
        Dir, // 路径存在目录
    }

#if HIDE_DECOHERENCE
    internal enum PathRelation
#else
    public enum PathRelation
#endif
    {
        Same, // 路径一致
        Child, // 子关系，比如 A/B/C 是 A 的子  
        Parent, // 父关系，比如 A 是 A/B/C 的父
        Brother, // 兄弟关系，在同目录下
        Irrelevant, // 不相关，比如 A/B/C 和 A/D 
    }

    /// <summary>
    /// 路径相关工具函数，路径的概念包括文件和文件夹
    /// thread safe
    /// </summary>
#if HIDE_DECOHERENCE
    internal static class PathUtil
#else
    public static class PathUtil
#endif
    {
        public static string directorySeparator
        {
            get;
            private set;
        }

        static PathUtil()
        {
            directorySeparator = Path.DirectorySeparatorChar + "";
        }

        public static string GetFirstMatchPath(string pathPattern)
        {
            if (string.IsNullOrWhiteSpace(pathPattern))
            {
                throw new ArgumentNullException(nameof(pathPattern));
            }

            var dir = new DirectoryInfo(Path.GetDirectoryName(pathPattern));
            foreach (var fi in dir.GetFiles(Path.GetFileName(pathPattern)))
            {
                return fi.FullName;
            }
            foreach (var di in dir.GetDirectories(Path.GetFileName(pathPattern)))
            {
                return di.FullName;
            }

            return null;
        }

        /// <summary>
        /// 转换路径为Linux的标准路径格式
        /// </summary>
        public static string ToNormalizeLinuxPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            return path.Replace('\\', '/').TrimEnd('/');

        }

        /// <summary>
        /// 转换路径为Windows的标准路径格式
        /// </summary>
        public static string ToNormalizeWindowsPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            return path.Replace('/', '\\').TrimEnd('\\');
        }

        /// <summary>
        /// 尝试删除指定路径
        /// </summary>
        public static bool TryDeletePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            else if (Directory.Exists(path))
            {
                if (string.Equals(path.Trim(), "/", StringComparison.Ordinal))
                    throw new Exception("Fatal Error: try to delete root dir");

                Directory.Delete(path, true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 尝试删除指定路径
        /// </summary>
        public static void DeletePath(string path)
        {
            if (!TryDeletePath(path))
            {
                throw new IOException(string.Format("删除目录 {0} 失败", path));
            }
        }

        /// <summary>
        /// 路径存在性判断
        /// </summary>
        public static bool Exists(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else if (Directory.Exists(path))
            {
                return true;
            }

            return false;
        }

        public static void MovePath(string sourPath, string destPath, bool force)
        {
            var sourState = GetPathState(sourPath);
            if (sourState == PathState.None) throw new ArgumentException($"源路径'{sourPath}'不存在！", nameof(sourPath));

            DirUtil.TryCreateParentDir(destPath);

            var destState = GetPathState(destPath);
            if (destState != PathState.None)
            {
                if (!force) throw new ArgumentException($"目标路径已经存在！", nameof(destPath));

                DeletePath(destPath);
            }

            if (sourState == PathState.Dir)
            {
                Directory.Move(sourPath, destPath);
            }
            else
            {
                File.Move(sourPath, destPath);
            }
        }

        public static void MovePathToDir(string sourPath, string destDir, bool force)
        {
            MovePath(sourPath, Path.Combine(destDir, Path.GetFileName(sourPath)), force);
        }

        public static void CopyPathToDir(string sourPath, string destDir, bool force)
        {
            CopyPath(sourPath, Path.Combine(destDir, Path.GetFileName(sourPath)), force);
        }

        /// <summary>
        /// 拷贝源路径到目标路径
        /// </summary>
        /// <param name="sourPath"> 源路径 </param>
        /// <param name="destPath"> 目标路径 </param>
        /// <param name="force"> 是否强制覆盖目标路径 </param>
        public static void CopyPath(string sourPath, string destPath, bool force)
        {
            if (sourPath == destPath)
            {
                throw new IOException("sourPath and destPath can't be same!");
            }


            int wildcardIndex = sourPath.LastIndexOf("*", System.StringComparison.Ordinal);
            bool srcWildcard = wildcardIndex >= 0;
            string needCheckPath = sourPath;
            if (srcWildcard)
            {
                int lastSlashIndex = sourPath.LastIndexOf('/');
                if (lastSlashIndex > wildcardIndex)
                {
                    throw new IOException(string.Format("sourPath {0} 是不合法的路径！", sourPath));
                }

                needCheckPath = Path.GetDirectoryName(sourPath);
            }

            var sourState = GetPathState(needCheckPath);
            if (sourState == PathState.None)
            {
                throw new IOException(string.Format("{0} 路径不存在！", needCheckPath));
            }

            var destState = GetPathState(destPath);
            if (!force && destState != PathState.None)
            {
                throw new IOException("destPath is not empty!");
            }

            if (srcWildcard)
            {
                if (destState == PathState.File)
                {
                    if (force)
                    {
                        PathUtil.DeletePath(destPath);
                    }
                    else
                    {
                        throw new IOException(string.Format("destPath {0} 不能是一个文件，除非指明force！", destPath));
                    }
                }

                
                DirUtil.TryCreateDir(destPath);

                var files = Directory.GetFiles(Path.GetDirectoryName(sourPath), Path.GetFileName(sourPath));
                foreach (var f in files)
                {
                    PathUtil.CopyPath(f, Path.Combine(destPath, Path.GetFileName(f)), force);
                }

                var dirs = Directory.GetDirectories(Path.GetDirectoryName(sourPath), Path.GetFileName(sourPath));
                foreach (var d in dirs)
                {
                    PathUtil.CopyPath(d, Path.Combine(destPath, Path.GetFileName(d)), force);
                }
            }
            else
            {
                var relation = GetPathRelation(sourPath, destPath);
                if (sourState == PathState.Dir && relation == PathRelation.Parent)
                {
                    throw new IOException("can't copy a directory into itself!");
                }

                if (destState == PathState.Dir && relation == PathRelation.Child)
                {
                    throw new IOException("can't overwrite sourPath's parent dir!");
                }

                DirUtil.TryCreateParentDir(destPath, force);
                TryDeletePath(destPath);
                if (sourState == PathState.File)
                {
                    File.Copy(sourPath, destPath);
                }
                else if (sourState == PathState.Dir)
                {
                    DirUtil.CopyDir(sourPath, destPath, true);
                }
            }
        }

        /// <summary>
        /// 获取path1与path2的关系
        /// </summary>
        public static PathRelation GetPathRelation(string path1, string path2)
        {
            if (path1 == path2)
            {
                return PathRelation.Same;
            }

            if (Path.GetDirectoryName(path1) == Path.GetDirectoryName(path2))
            {
                return PathRelation.Brother;
            }

            if (path1.StartsWith(path2))
            {
                return PathRelation.Child;
            }

            if (path2.StartsWith(path1))
            {
                return PathRelation.Parent;
            }

            return PathRelation.Irrelevant;
        }

        /// <summary>
        /// 获取指定路径的状态
        /// </summary>
        public static PathState GetPathState(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return PathState.None;
            }

            if (File.Exists(path))
            {
                return PathState.File;
            }
            else if (Directory.Exists(path))
            {
                return PathState.Dir;
            }

            return PathState.None;
        }

        public static string GetRelativePath(string relativeTo, string path)
        {
            path = Path.GetFullPath(path).Replace("\\", "/");
            relativeTo = Path.GetFullPath(relativeTo).Replace("\\", "/") + "/";
            if (path.StartsWith(relativeTo))
            {
                return path.Remove(0, relativeTo.Length);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
