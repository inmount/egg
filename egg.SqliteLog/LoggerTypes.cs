using System;
using System.Collections.Generic;
using System.Text;

namespace egg.SqliteLog {

    /// <summary>
    /// 类型
    /// </summary>
    public enum LoggerTypes {

        /// <summary>
        /// 信息
        /// </summary>
        Info=0x00,

        /// <summary>
        /// 警告
        /// </summary>
        Warn=0x01,

        /// <summary>
        /// 错误
        /// </summary>
        Error=0x99

    }
}
