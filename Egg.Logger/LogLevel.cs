using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Log {

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel {
        /// <summary>
        /// 调试
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 信息
        /// </summary>
        Info = 1,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 2,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 4,
        /// <summary>
        /// 致命
        /// </summary>
        Fatal = 8,
    }
}
