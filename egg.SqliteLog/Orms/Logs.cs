using egg.db.Orm;
using System;
using System.Collections.Generic;
using System.Text;

namespace egg.SqliteLog.Orms {

    /// <summary>
    /// 日志记录
    /// </summary>
    [Table("Logs")]
    public class Logs : Row {

        /// <summary>
        /// 对象标识
        /// </summary>
        [Field(FieldType = FieldTypes.Decimal, Size = 18)]
        public long ObjectID { get { return base["ObjectID"]; } set { base["ObjectID"] = value; } }

        /// <summary>
        /// 事件标识
        /// </summary>
        [Field(FieldType = FieldTypes.Decimal, Size = 18)]
        public long EventID { get { return base["EventID"]; } set { base["EventID"] = value; } }

        /// <summary>
        /// 事件类型标识
        /// </summary>
        [Field(FieldType = FieldTypes.Decimal, Size = 18)]
        public long TypeID { get { return base["TypeID"]; } set { base["TypeID"] = value; } }

        /// <summary>
        /// 描述
        /// </summary>
        [Field(FieldType = FieldTypes.String, Size = 500)]
        public string Detail { get { return base["Detail"]; } set { base["Detail"] = value; } }

        /// <summary>
        /// 记录时间
        /// </summary>
        [Field(FieldType = FieldTypes.Decimal, Size = 18)]
        public long LogTime { get { return base["LogTime"]; } set { base["LogTime"] = value; } }

    }
}
