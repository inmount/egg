using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.Orm {

    /// <summary>
    /// Orm列字段类型
    /// </summary>
    public enum FieldTypes {

        /// <summary>
        /// 整型数据
        /// </summary>
        Integer = 0x01,

        /// <summary>
        /// 整型数据
        /// </summary>
        Long = 0x02,

        /// <summary>
        /// 带精度的数据
        /// </summary>
        Decimal = 0x11,

        /// <summary>
        /// 字符串类型
        /// </summary>
        String = 0x21,

    }
}
