using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlStatements {

    /// <summary>
    /// 数据插入语句
    /// </summary>
    public class Insert : egg.BasicObject, ISqlStringable {

        // 数据库连接
        private Connection _dbc;

        // 表对象
        private SqlUnits.Table _table;

        // 行数据对象
        private Row _row;

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return true; } set { } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="table"></param>
        /// <param name="row"></param>
        public Insert(Connection dbc, SqlUnits.Table table, Row row) {
            _dbc = dbc;
            _table = table;
            _row = row;
        }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string res = "INSERT INTO ";
            res += _table.ToSqlString(tp);

            string cols = "";
            string vals = "";

            foreach (var key in _row.Keys) {
                if (!eggs.Object.IsNull(_row[key])) {
                    if (cols != "") cols += ",";
                    if (vals != "") vals += ",";
                    using (SqlUnits.TableField field = new SqlUnits.TableField(_table, key)) {
                        cols += field.ToSqlString(tp);
                    }
                    switch (tp) {
                        case DatabaseTypes.MySQL:
                            vals += $"'{_row[key].Replace("'", "\'")}'";
                            break;
                        //return $"'{_value.Replace("'", "\'")}'";
                        case DatabaseTypes.Microsoft_Office_Access:
                        case DatabaseTypes.Microsoft_Office_Access_v12:
                        case DatabaseTypes.Microsoft_SQL_Server:
                        case DatabaseTypes.SQLite:
                        case DatabaseTypes.SQLite_3:
                        case DatabaseTypes.PostgreSQL:
                            vals += $"'{_row[key].Replace("'", "''")}'";
                            break;
                        //return $"'{_value.Replace("'", "''")}'";
                        default:
                            throw new Exception($"尚未支持数据库 {tp.ToString()} 中的字符串转义。");
                    }
                }
            }

            res += $"({cols}) VALUES ({vals})";

            return res;
        }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <returns></returns>
        public string ToSqlString() { return ToSqlString(_dbc.DatabaseType); }

        /// <summary>
        /// 执行插入
        /// </summary>
        /// <returns></returns>
        public int Exec() {
            return _dbc.Exec(this.ToSqlString());
        }
    }
}
