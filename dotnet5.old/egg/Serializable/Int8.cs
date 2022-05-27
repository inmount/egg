using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {

    /// <summary>
    /// 字节内容
    /// </summary>
    public class Int8 : ISerializable {

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public byte Value { set; get; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="val"></param>
        public Int8(byte val = 0) {
            this.Value = val;
        }

        /// <summary>
        /// 存储大小
        /// </summary>
        public int Size => 1;

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void SetValue(byte[] val, int offset = 0, int count = 0) {
            if (val == null) {
                this.Value = 0;
            } else if (val.Length <= offset) {
                this.Value = 0;
            } else {
                this.Value = val[offset];
            }
        }

        /// <summary>
        /// 获取字节数字表示形式
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes() {
            return new byte[] { this.Value };
        }

        /// <summary>
        /// 赋值方式创建内容
        /// </summary>
        /// <param name="val">内容</param>
        public static implicit operator Int8(byte val) {
            return new Int8(val);
        }

        /// <summary>
        /// 赋值防暑输出内容
        /// </summary>
        /// <param name="val">内容</param>
        public static implicit operator byte(Int8 val) {
            return val.Value;
        }

        /// <summary>
        /// 获取标准字符串表示形式
        /// </summary>
        /// <returns></returns>
        public new string ToString() { return $"{this.Value}"; }
    }
}
