using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Egg {

    /// <summary>
    /// 文件流扩展
    /// </summary>
    public static class FileSreamExtensions {

        #region [=====读取=====]

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this FileStream file)
        {
            file.Position = 0;
            byte[] res = new byte[file.Length];
            file.Read(res, 0, res.Length);
            return res;
        }

        /// <summary>
        /// 获取所有文本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadAllText(this FileStream file, Encoding encoding)
        {
            return encoding.GetString(file.ReadAllBytes());
        }

        /// <summary>
        /// 以UTF8编码获取所有文本
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadAllUtf8Text(this FileStream file)
        {
            file.Position = 0;
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, buffer.Length);
            // 兼容BOM
            if (buffer.Length >= 3) {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF) {
                    return System.Text.Encoding.UTF8.GetString(buffer, 3, buffer.Length - 3);
                }
            }
            return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 以UTF8编码获取一行文本
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadUtf8Line(this FileStream file)
        {
            byte lr = (byte)'\r';
            byte ln = (byte)'\n';
            List<byte> list = new List<byte>();
            long posStart = file.Position;
            int bs = 0;
            do {
                bs = file.ReadByte();
                if (bs > 0) {
                    if (bs != ln && bs != lr) list.Add((byte)bs);
                }
            } while (bs == ln || bs < 0);
            // 兼容BOM
            if (posStart == 0 && list.Count >= 3) {
                if (list[0] == 0xEF && list[1] == 0xBB && list[2] == 0xBF) {
                    return System.Text.Encoding.UTF8.GetString(list.ToArray(), 3, list.Count - 3);
                }
            }
            return System.Text.Encoding.UTF8.GetString(list.ToArray());
        }

        #endregion

        #region [=====写入=====]

        /// <summary>
        /// 写入文件内容
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bytes"></param>
        public static void WriteAllBytes(this FileStream file, byte[] bytes)
        {
            file.Position = 0;
            file.Write(bytes);
        }

        /// <summary>
        /// 写入所有文本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encoding"></param>
        /// <param name="content"></param>
        public static void WriteAllText(this FileStream file, Encoding encoding, string content)
        {
            file.WriteAllBytes(encoding.GetBytes(content));
        }

        /// <summary>
        /// 以UTF8编码写入所有文本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        public static void WriteAllUtf8Text(this FileStream file, string content)
        {
            file.Position = 0;
            file.Write(new byte[] { 0xEF, 0xBB, 0xBF });
            file.Write(System.Text.Encoding.UTF8.GetBytes(content));
        }

        /// <summary>
        /// 以UTF8编码写入所有文本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        public static void WriteUtf8Line(this FileStream file, string content)
        {
            if (file.Position == 0)
                file.Write(new byte[] { 0xEF, 0xBB, 0xBF });
            file.Write(System.Text.Encoding.UTF8.GetBytes(content + "\r\n"));
        }

        #endregion


    }
}
