using System;
using System.Collections.Generic;
using System.Text;

namespace egg.SqliteLog {

    /// <summary>
    /// 日志对象
    /// </summary>
    public class LoggerObject {

        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }
}
