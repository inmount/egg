using System;
using System.Collections.Generic;
using System.Text;
using egg.db.Orm;

namespace egg.SqliteLog.Orms {

    /// <summary>
    /// 日志对象
    /// </summary>
    [Table("Events")]
    public class Events : Row {

        /// <summary>
        /// 对象标识
        /// </summary>
        [Field(FieldType = FieldTypes.Decimal, Size = 18)]
        public long ObjectID { get { return base["ObjectID"]; } set { base["ObjectID"] = value; } }

        /// <summary>
        /// 名称
        /// </summary>
        [Field(FieldType = FieldTypes.String, Size = 50)]
        public string Name { get { return base["Name"]; } set { base["Name"] = value; } }

        /// <summary>
        /// 描述
        /// </summary>
        [Field(FieldType = FieldTypes.String, Size = 500)]
        public string Description { get { return base["Description"]; } set { base["Description"] = value; } }

    }
}
