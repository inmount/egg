using egg.db.Orm;
using egg.SqliteLog.Orms;
using System;
using System.Collections.Generic;
using System.Text;

namespace egg.SqliteLog {

    /// <summary>
    /// 日志记录
    /// </summary>
    public class LoggerRecord : Logs {

        /// <summary>
        /// 对象名称
        /// </summary>
        [Field(IsRealField = false)]
        public string ObjectName { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        [Field(IsRealField = false)]
        public string EventName { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        [Field(IsRealField = false)]
        public string TypeName { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Field(IsRealField = false)]
        public egg.Time Time { get; set; }

    }
}
