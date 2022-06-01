using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 数字单元
    /// </summary>
    public class String : egg.BasicObject, ISqlStringable {

        // 值
        private string _value;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="value"></param>
        public String(string value) {
            _value = value;
        }

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return false; } set { } }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            if (_value == null) return "NULL";
            switch (tp) {
                case DatabaseTypes.MySQL:
                    return $"'{_value.Replace("'", "\'")}'";
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    return $"'{_value.Replace("'", "''")}'";
                case DatabaseTypes.PostgreSQL:
                    return $"'{_value.Replace("'", "''").Replace("\"","\\\"")}'";
                default:
                    throw new Exception($"尚未支持数据库 {tp.ToString()} 中的字符串转义。");
            }

        }
    }
}
