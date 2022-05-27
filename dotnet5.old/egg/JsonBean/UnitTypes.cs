using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {
    /// <summary>
    /// 存储单元类型
    /// </summary>
    public enum UnitTypes {

        /// <summary>
        /// 空类型
        /// </summary>
        None=0x00,

        /// <summary>
        /// 布尔类型
        /// </summary>
        Boolean = 0x01,

        /// <summary>
        /// 数值类型
        /// </summary>
        Number = 0x02,

        /// <summary>
        /// 字符串类型
        /// </summary>
        String = 0x11,

        /// <summary>
        /// 对象类型
        /// </summary>
        Object = 0x21,

        /// <summary>
        /// 数组类型
        /// </summary>
        Array = 0x31

    }

}
