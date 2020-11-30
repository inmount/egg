using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 表定义
    /// </summary>
    public class Table : egg.Object, ISqlStringable, ISqlTableStringable {

        // 名称
        private string _name;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public Table(string name) {
            _name = name;
        }

        /// <summary>
        /// 获取字符串表现形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return _name;
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
        public string ToSqlString(egg.db.DatabaseTypes tp, bool multiTable = false) {
            switch (tp) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    return $"[{_name}]";
                case DatabaseTypes.MySQL:
                    return $"`{_name}`";
                case DatabaseTypes.PostgreSQL:
                    return $"\"{_name}\"";
                default:
                    throw new Exception($"尚未支持 {tp.ToString()} 类型数据库");
            }
        }

        /// <summary>
        /// 获取标准字段SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public string ToSqlTableString(egg.db.DatabaseTypes tp) {
            return ToSqlString(tp);
        }
    }
}
