using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {

    /// <summary>
    /// 字节内容
    /// </summary>
    public class UTF8String : ISerializable {

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string Value { set; get; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="val"></param>
        public UTF8String(string val = null) {
            this.Value = val;
        }

        /// <summary>
        /// 存储大小
        /// </summary>
        public int Size {
            get {
                if (this.Value.IsNoneOrNull()) {
                    return 4;
                } else {
                    return System.Text.Encoding.UTF8.GetBytes(this.Value).Length + 4;
                }
            }
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void SetValue(byte[] val, int offset = 0, int count = 0) {
            // 读取数据长度
            int size = BitConverter.ToInt32(val, offset);
            this.Value = System.Text.Encoding.UTF8.GetString(val, offset + 4, size);
        }

        /// <summary>
        /// 获取字节数字表示形式
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {
            List<byte> ls = new List<byte>();
            // 添加字符串长度信息
            ls.AddRange(BitConverter.GetBytes(this.Size - 4));
            ls.AddRange(System.Text.Encoding.UTF8.GetBytes(this.Value));
            return ls.ToArray();
        }

        /// <summary>
        /// 赋值方式创建内容
        /// </summary>
        /// <param name="val">内容</param>
        public static implicit operator UTF8String(string val) {
            return new UTF8String(val);
        }

        /// <summary>
        /// 赋值防暑输出内容
        /// </summary>
        /// <param name="val">内容</param>
        public static implicit operator string(UTF8String val) {
            return val.Value;
        }

        /// <summary>
        /// 获取标准字符串表示形式
        /// </summary>
        /// <returns></returns>
        public new string ToString() { return $"{this.Value}"; }
    }
}
