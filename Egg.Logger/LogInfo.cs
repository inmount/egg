using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Log {

    /// <summary>
    /// 记录实体
    /// </summary>
    public class LogInfo {

        /// <summary>
        /// 级别
        /// </summary>
        public virtual LogLevel Level { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public virtual string? Event { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public virtual string? Message { get; set; }

    }
}
