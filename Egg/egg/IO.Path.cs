using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Egg;

namespace egg {

    /// <summary>
    /// 输入输出相关函数
    /// </summary>
    public static partial class IO {

        /// <summary>
        /// 获取操作系统的标准路径格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetOSPathFormat(string path)
        {
            if (OS.IsWindows) {
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
            path = GetOSPathFormat(path);
            if (OS.IsWindows) {
                if (path.EndsWith("\\")) return path;
                return path + "\\";
            }
            if (path.EndsWith("/")) return path;
            return path + "/";
        }

        /// <summary>
        /// 获取执行目录下的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExecutionPath(string path)
        {
            return Assembly.ExecutionDirectory + GetOSPathFormat(path);
        }

        /// <summary>
        /// 获取执行目录下的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetWorkPath(string path)
        {
            return Assembly.WorkingDirectory + GetOSPathFormat(path);
        }

        /// <summary>
        /// 获取所属文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string[] GetFiles(string path, [Optional] string pattern)
        {
            if (pattern.IsNullOrEmpty()) {
                return System.IO.Directory.GetFiles(path);
            } else {
                return System.IO.Directory.GetFiles(path, pattern);
            }
        }

    }

}
