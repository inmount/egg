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

        /// <summary>
        /// 检测文件是否存在 - 可使用FileExists替代
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool CheckFileExists(string path) { return System.IO.File.Exists(path); }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path) { return System.IO.File.Exists(path); }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            try
            {
                System.IO.File.Delete(path); return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="access"></param>
        /// <param name="share"></param>
        /// <returns></returns>
        public static FileStream OpenFile(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None)
        {
            return System.IO.File.Open(path, mode, access, share);
        }

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMD5(string path)
        {
            string md5Str = "";
            using (FileStream file = new FileStream(path, System.IO.FileMode.Open))
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] bytes = md5.ComputeHash(file);
                for (int i = 0; i < bytes.Length; i++)
                {
                    md5Str += bytes[i].ToString("X").PadLeft(2, '0');
                }
            }
            return md5Str;
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static byte[] ReadAllFileBytes(string path, bool create = false)
        {
            byte[]? res = null;
            if (System.IO.File.Exists(path))
            {
                using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
                {
                    res = fs.ReadAllBytes();
                }
            }
            else
            {
                res = new byte[0];
                if (create) WriteAllFileBytes(path, new byte[0]);
            }
            return res;
        }

        /// <summary>
        /// 将内容写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cnt"></param>
        public static void WriteAllFileBytes(string path, byte[] cnt)
        {
            using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read))
            {
                fs.WriteAllBytes(cnt);
            }
        }

        /// <summary>
        /// 获取Utf8文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static string ReadUtf8FileContent(string path, bool create = false)
        {
            if (System.IO.File.Exists(path))
            {
                using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
                {
                    return fs.ReadAllUtf8Text();
                }
            }
            if (create) WriteAllFileBytes(path, new byte[] { 0xEF, 0xBB, 0xBF });
            return "";
        }

        /// <summary>
        /// 设置Utf8文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static void WriteUtf8FileContent(string path, string content)
        {
            using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read))
            {
                fs.WriteAllUtf8Text(content);
            }
        }

    }

}
