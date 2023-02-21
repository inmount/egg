using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Egg;

namespace egg
{

    /// <summary>
    /// 输入输出相关函数
    /// </summary>
    public static partial class IO
    {
        // 路径分隔符
        private static char? _pathSeparator = null;

        /// <summary>
        /// 获取路径分隔符
        /// </summary>
        public static char GetPathSeparator()
        {
            if (_pathSeparator is null)
            {
                if (OS.IsWindows)
                {
                    _pathSeparator = '\\';
                }
                else
                {
                    _pathSeparator = '/';
                }
            }
            return _pathSeparator ?? '/';
        }

        /// <summary>
        /// 获取操作系统的标准路径格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetOSPathFormat(string path)
        {
            if (OS.IsWindows)
            {
                return path.Replace("/", "\\");
            }
            return path.Replace("\\", "/");
        }

        /// <summary>
        /// 获取闭合路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetClosedPath(string path)
        {
            // 获取分隔符
            var separator = GetPathSeparator();
            path = GetOSPathFormat(path);
            if (path.EndsWith(separator)) return path;
            return path + separator;
        }

        /// <summary>
        /// 获取非闭合路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetUnclosedPath(string path)
        {
            // 获取分隔符
            var separator = GetPathSeparator();
            path = GetOSPathFormat(path);
            if (path.EndsWith(separator)) return path.Substring(0, path.Length - 1);
            return path;
        }

        /// <summary>
        /// 获取执行目录下的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExecutionPath(string path)
        {
            if (path.IsNullOrWhiteSpace()) throw new Exception($"路径不能为空");
            return CombinePath(Assembly.ExecutionDirectory, path);
        }

        /// <summary>
        /// 获取执行目录下的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetWorkPath(string path)
        {
            if (path.IsNullOrWhiteSpace()) throw new Exception($"路径不能为空");
            return CombinePath(Assembly.WorkingDirectory, path);
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string CombinePath(string path1, string path2)
        {
            return GetClosedPath(path1) + GetOSPathFormat(path2);
        }

    }

}
