using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 字符串集合单元
    /// </summary>
    public class Strings : egg.Object, ISqlStringable {

        // 值
        private string[] _value;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="value"></param>
        public Strings(string[] value) {
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
            string res = "";
            for (int i = 0; i < _value.Length; i++) {
                if (res != "") res += ",";
                //res += $"'{_value[i]}'";
                switch (tp) {
                    case DatabaseTypes.MySQL:
                        res += $"'{_value[i].Replace("'", "\'")}'";
                        break;
                    case DatabaseTypes.Microsoft_Office_Access:
                    case DatabaseTypes.Microsoft_Office_Access_v12:
                    case DatabaseTypes.Microsoft_SQL_Server:
                    case DatabaseTypes.SQLite:
                    case DatabaseTypes.SQLite_3:
                    case DatabaseTypes.PostgreSQL:
                        res += $"'{_value[i].Replace("'", "''")}'";
                        break;
                    default:
                        throw new Exception($"尚未支持数据库 {tp.ToString()} 中的字符串转义。");
                }
            }
            return $"({res})";
        }
    }
}
