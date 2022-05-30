using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable {

    /// <summary>
    /// 可序列化单元
    /// </summary>
    public interface ISerializable {

        /// <summary>
        /// 反序列化字节内容
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        void Deserialize(Span<byte> bytes);

        /// <summary>
        /// 反序列化字符串内容
        /// </summary>
        /// <param name="str"></param>
        void Deserialize(string str);

        /// <summary>
        /// 获取序列化结果
        /// </summary>
        /// <returns></returns>
        byte[] SerializeToBytes();

        /// <summary>
        /// 获取序列化结果
        /// </summary>
        /// <returns></returns>
        string SerializeToString();

    }
}
