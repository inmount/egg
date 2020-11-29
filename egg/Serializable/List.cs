using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {

    /// <summary>
    /// 可序列化列表
    /// </summary>
    public class List : List<ISerializable>, ISerializable {

        /// <summary>
        /// 获取存储尺寸
        /// </summary>
        public int Size {
            get {
                int res = 4;
                foreach (var item in this) {
                    res += item.Size;
                }
                return res;
            }
        }

        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="val"></param>
        public void Add(byte val) {
            base.Add(new Int8(val));
        }

        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="val"></param>
        public void Add(int val) {
            base.Add(new Int16(val));
        }

        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="val"></param>
        public void Add(long val) {
            base.Add(new Int32(val));
        }

        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="val"></param>
        public void Add(float val) {
            base.Add(new Single(val));
        }

        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="val"></param>
        public void Add(double val) {
            base.Add(new Double(val));
        }

        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="val"></param>
        public void Add(string val) {
            base.Add(new UTF8String(val));
        }

        /// <summary>
        /// 内容
        /// </summary>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void SetValue(byte[] val, int offset = 0, int count = 0) {

            // 读取数据长度
            int size = BitConverter.ToInt32(val, offset);
            offset += 4;

            // 按顺序设置内容
            foreach (var item in this) {

                // 建立数组片段
                item.SetValue(val, offset, val.Length - offset);
                offset += item.Size;

            }

        }

        /// <summary>
        /// 获取字节数字表示形式
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {
            List<byte> ls = new List<byte>();
            // 添加长度信息
            ls.AddRange(BitConverter.GetBytes(this.Size - 4));
            // 按顺序填充字节数组
            foreach (var item in this) {
                ls.AddRange(item.ToBytes());
            }
            return ls.ToArray();
        }

    }
}
