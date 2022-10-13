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
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFolder(string path)
        {
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 检测文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckFolderExists(string path) { return System.IO.Directory.Exists(path); }

        /// <summary>
        /// 获取所属子文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFolders(string path)
        {
            return System.IO.Directory.GetDirectories(path);
        }

    }

}
