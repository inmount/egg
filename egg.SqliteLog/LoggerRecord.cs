using System;
using System.Collections.Generic;
using System.Text;

namespace egg.SqliteLog {

    /// <summary>
    /// 日志记录
    /// </summary>
    public class LoggerRecord {

        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 对象名称
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public egg.Time Time { get; set; }

    }
}
