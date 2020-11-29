using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace egg.File {

    /// <summary>
    /// 以UTF8编码操作文件
    /// </summary>
    public class UTF8File : TextFile {

        /// <summary>
        /// 获取或设置是否带BOOM输出
        /// </summary>
        public static bool WriteWithBoom = true;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        public UTF8File(string path, System.IO.FileMode mode = FileMode.Open) : base(path, Encoding.UTF8, mode) { }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static string ReadAllText(string path, bool create) {

            // 判断是否为自动创建
            if (create) {
                if (!System.IO.File.Exists(path)) {
                    using (var file = new egg.File.BinaryFile(path, FileMode.OpenOrCreate)) {
                        if (WriteWithBoom) file.Write(new byte[] { 0xEF, 0xBB, 0xBF });
                    }
                }
            }

            byte[] res = BinaryFile.ReadAllBytes(path, false);
            if (res.Length >= 3) {
                if (res[0] == 0xEF && res[1] == 0xBB && res[2] == 0xBF) {
                    return System.Text.Encoding.UTF8.GetString(res, 3, res.Length - 3);
                }
            }
            return System.Text.Encoding.UTF8.GetString(res);
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadAllText(string path) {
            return ReadAllText(path, false);
        }

        /// <summary>
        /// 将内容写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cnt"></param>
        public static void WriteAllText(string path, string cnt) {
            using (var file = new egg.File.UTF8File(path, FileMode.Create)) {
                if (WriteWithBoom) file.Write(new byte[] { 0xEF, 0xBB, 0xBF });
                byte[] res = System.Text.Encoding.UTF8.GetBytes(cnt);
                file.Write(res);
            }
        }

        /// <summary>
        /// 将内容追加写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cnt"></param>
        public static void AppendText(string path, string cnt) {
            using (var file = new egg.File.UTF8File(path, FileMode.Open)) {
                byte[] res = System.Text.Encoding.UTF8.GetBytes(cnt);
                file.Append(res);
            }
        }

        /// <summary>
        /// 将内容追加写入文件并换行
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="cnt"></param>
        public static void AppendTextLine(string Path, string cnt) {
            AppendText(Path, String.Format("{0}\r\n", cnt));
        }

    }
}
