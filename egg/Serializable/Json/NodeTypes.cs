using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeTypes {
        /// <summary>
        /// Null
        /// </summary>
        Null = 0x00,
        /// <summary>
        /// 数值
        /// </summary>
        Number = 0x01,
        /// <summary>
        /// 布尔
        /// </summary>
        Boolean = 0x02,
        /// <summary>
        /// 字符串
        /// </summary>
        String = 0x03,
        /// <summary>
        /// 对象
        /// </summary>
        Object = 0x11,
        /// <summary>
        /// 列表
        /// </summary>
        List = 0x12
    }
}
