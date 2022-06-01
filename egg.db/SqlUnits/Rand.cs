using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 随机函数内容
    /// </summary>
    public class Rand : egg.BasicObject, ISqlStringable {

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
            switch (tp) {
                case DatabaseTypes.Microsoft_Office_Access:
                case DatabaseTypes.Microsoft_Office_Access_v12:
                    return "RND(ID)";
                case DatabaseTypes.Microsoft_SQL_Server:
                    return "NEWID()";
                case DatabaseTypes.SQLite:
                case DatabaseTypes.SQLite_3:
                case DatabaseTypes.PostgreSQL:
                    return "RANDOM()";
                case DatabaseTypes.MySQL:
                    return "RAND()";
                default:
                    throw new Exception($"尚未支持 {tp.ToString()} 类型数据库");
            }
        }

    }
}
