using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {

    /// <summary>
    /// 可序列化单元
    /// </summary>
    public interface ISerializable {


        /// <summary>
        /// 设置字节内容
        /// </summary>
        /// <param name="val"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        void SetValue(byte[] val, int offset = 0, int count = 0);

        /// <summary>
        /// 获取存储大小
        /// </summary>
        /// <returns></returns>
        int Size { get; }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();

    }
}
