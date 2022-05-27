using System;
using System.Collections.Generic;
using System.Text;

namespace egg.File {

    /// <summary>
    /// 文本文件
    /// </summary>
    public class TextFile : BinaryFile {

        //private long position = 0;

        /// <summary>
        /// 获取文本文件的编码形式
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="mode"></param>
        public TextFile(string path, Encoding encoding, System.IO.FileMode mode = System.IO.FileMode.Open) : base(path, mode) {
            this.Encoding = encoding;
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public string ReadToEnd(long pos) {
            byte[] bs = base.Read(pos, (int)(base.Length - pos));
            return this.Encoding.GetString(bs);
        }

        /// <summary>
        /// 读取所有内容
        /// </summary>
        /// <returns></returns>
        public string ReadToEnd() {
            byte[] bs = base.Read((int)(base.Length - base.Position));
            return this.Encoding.GetString(bs);
        }

        /// <summary>
        /// 读取一行字符
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public string ReadLine(long pos) {
            string res = "";
            List<byte> list = new List<byte>();
            base.Position = pos;
            byte bs = 0;
            while (!base.IsEnd && bs != 10) {
                bs = base.ReadByte();
                if (bs != 10 && bs != 13) {
                    list.Add((byte)bs);
                }
            }
            byte[] bytes = new byte[list.Count];
            list.CopyTo(bytes);
            res = this.Encoding.GetString(bytes);
            return res;
        }

        /// <summary>
        /// 读取一行字符
        /// </summary>
        /// <returns></returns>
        public string ReadLine() {
            string res = "";
            List<byte> list = new List<byte>();
            //base.Position = pos;
            byte bs = 0;
            while (!base.IsEnd && bs != 10) {
                bs = base.ReadByte();
                if (bs != 10 && bs != 13) {
                    list.Add((byte)bs);
                }
            }
            byte[] bytes = new byte[list.Count];
            list.CopyTo(bytes);
            res = this.Encoding.GetString(bytes);
            return res;
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public new string Read(long pos, int len) {
            byte[] bs = base.Read(pos, len);
            return this.Encoding.GetString(bs);
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public new string Read(int len) {
            byte[] bs = base.Read(len);
            return this.Encoding.GetString(bs);
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="content"></param>
        public void Write(long pos, string content) {
            base.Write(pos, this.Encoding.GetBytes(content));
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="content"></param>
        public void Write(string content) {
            base.Write(this.Encoding.GetBytes(content));
        }

        /// <summary>
        /// 写一行内容
        /// </summary>
        /// <param name="content"></param>
        public void WriteLine(string content) {
            base.Write(this.Encoding.GetBytes(content + "\r\n"));
        }

        /// <summary>
        /// 写一行内容
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="content"></param>
        public void WriteLine(long pos, string content) {
            base.Write(pos, this.Encoding.GetBytes(content + "\r\n"));
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="content"></param>
        public void Append(string content) {
            base.Append(this.Encoding.GetBytes(content));
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="content"></param>
        public void AppendLine(string content) {
            base.Append(this.Encoding.GetBytes(content + "\r\n"));
        }

    }
}
