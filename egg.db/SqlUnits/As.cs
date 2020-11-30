using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 重命名
    /// </summary>
    public class As : egg.Object, ISqlStringable, ISqlTableStringable {

        // 排序对象
        private ISqlStringable _object;

        // 排序信息
        private string _name;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        public As(ISqlStringable obj, string name) {
            _object = obj;
            _name = name;
        }

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return false; } set { } }

        /// <summary>
        /// 获取标准字段SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string name = "";
            switch (tp) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                case DatabaseTypes.Microsoft_SQL_Server:
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                    name = $"[{_name}]";
                    break;
                case DatabaseTypes.MySQL:
                    name = $"`{_name}`";
                    break;
                case DatabaseTypes.PostgreSQL:
                    name = $"\"{_name}\"";
                    break;
                default:
                    throw new Exception($"尚未支持 {tp.ToString()} 类型数据库");
            }
            return $"{_object.ToSqlString(tp, multiTable)} AS {name}";
        }

        /// <summary>
        /// 获取标准表SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public string ToSqlTableString(DatabaseTypes tp) {
            return ToSqlString(tp);
        }
    }
}
