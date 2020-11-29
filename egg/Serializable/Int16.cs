using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {

    /// <summary>
    /// 字节内容
    /// </summary>
    public class Int16 : ISerializable {

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public System.Int16 Value { set; get; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="val"></param>
        public Int16(int val = 0) {
            this.Value = (System.Int16)val;
        }

        /// <summary>
        /// 存储大小
        /// </summary>
        public int Size => 2;

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void SetValue(byte[] val, int offset = 0, int count = 0) {
            this.Value = BitConverter.ToInt16(val, offset);
        }

        /// <summary>
        /// 获取字节数字表示形式
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {
            return BitConverter.GetBytes(this.Value);
        }

        /// <summary>
        /// 赋值方式创建内容
        /// </summary>
        /// <param name="val">内容</param>
        public static implicit operator Int16(int val) {
            return new Int16(val);
        }

        /// <summary>
        /// 赋值防暑输出内容
        /// </summary>
        /// <param name="val">内容</param>
        public static implicit operator int(Int16 val) {
            return val.Value;
        }

        /// <summary>
        /// 获取标准字符串表示形式
        /// </summary>
        /// <returns></returns>
        public new string ToString() { return $"{this.Value}"; }
    }
}
