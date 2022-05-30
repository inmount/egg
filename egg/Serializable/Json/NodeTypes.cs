using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeTypes {
        Null = 0x00,
        Number = 0x01,
        Boolean = 0x02,
        String = 0x03,
        Object = 0x11,
        List = 0x12
    }
}
