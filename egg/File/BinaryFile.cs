using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace egg.File {

    /// <summary>
    /// 二进制操作文件
    /// </summary>
    public class BinaryFile : egg.BasicObject {

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// 文件操作流
        /// </summary>
        public FileStream Stream { get; private set; }

        /// <summary>
        /// 获取或设置当前操作位置
        /// </summary>
        public long Position { get { return this.Stream.Position; } set { this.Stream.Position = value; } }

        /// <summary>
        /// 获取是否在文档末尾
        /// </summary>
        public bool IsEnd {
            get {
                return (this.Stream.Position >= this.Stream.Length);
            }
        }

        /// <summary>
        /// 获取文件当前长度
        /// </summary>
        public long Length {
            get {
                return this.Stream.Length;
            }
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        public BinaryFile(string path, System.IO.FileMode mode = FileMode.Open) {
            this.FilePath = path;
            this.Stream = System.IO.File.Open(path, mode, FileAccess.ReadWrite);
            this.Position = 0;
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] Read(long pos, int len) {
            byte[] res = new byte[len];
            this.Stream.Position = pos;
            this.Stream.Read(res, 0, len);
            //this.Position = pos + len;
            return res;
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] Read(int len) {
            byte[] res = new byte[len];
            this.Stream.Read(res, 0, len);
            return res;
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <returns></returns>
        public byte ReadByte() {
            return Read(1)[0];
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void WriteByte(byte value) {
            this.Stream.WriteByte(value);
            this.Stream.Flush();
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public void Write(long pos, byte[] bytes) {
            this.Stream.Position = pos;
            this.Stream.Write(bytes, 0, bytes.Length);
            this.Stream.Flush();
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="bytes"></param>
        public void Write(byte[] bytes) {
            this.Stream.Write(bytes, 0, bytes.Length);
            this.Stream.Flush();
        }

        /// <summary>
        /// 在文件末尾添加内容
        /// </summary>
        /// <param name="bytes"></param>
        public void Append(byte[] bytes) {
            this.Stream.Position = this.Stream.Length;
            this.Stream.Write(bytes, 0, bytes.Length);
            this.Stream.Flush();
            //this.Position = this.Stream.Length;
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string Path, bool create) {
            byte[] res = null;
            if (System.IO.File.Exists(Path)) {
                using (System.IO.FileStream fs = System.IO.File.Open(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite)) {
                    res = new byte[fs.Length];
                    fs.Read(res, 0, (int)fs.Length);
                }
            } else {
                res = new byte[0];
                if (create) WriteAllBytes(Path, new byte[0]);
            }
            return res;
        }

        /// <summary>
        /// 将内容写入文件
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="cnt"></param>
        public static void WriteAllBytes(string Path, byte[] cnt) {
            using (System.IO.FileStream fs = System.IO.File.Open(Path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                //byte[] bs = System.Text.Encoding.UTF8.GetBytes(cnt);
                fs.Position = 0;
                fs.Write(cnt, 0, cnt.Length);
            }
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public void Close() {
            this.Stream.Close();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            try {
                this.Stream.Close();
                this.Stream.Dispose();
            } catch { }
            base.OnDispose();
        }

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        public static string GetMD5(string Path) {
            //System.Text.Encoding MyEnc = System.Text.Encoding.UTF8;
            //Byte[] StrByte = MyEnc.GetBytes(gszValue);
            string MD5Str = "";

            using (FileStream file = new FileStream(Path, System.IO.FileMode.Open)) {
                MD5CryptoServiceProvider TheMD5 = new MD5CryptoServiceProvider();
                Byte[] MD5byte = TheMD5.ComputeHash(file);

                for (int i = 0; i < MD5byte.Length; i++) {
                    MD5Str += MD5byte[i].ToString("X").PadLeft(2, '0');
                }
            }

            return MD5Str;
        }

    }
}
