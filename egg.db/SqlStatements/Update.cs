using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlStatements {

    /// <summary>
    /// 数据更新语句
    /// </summary>
    public class Update : egg.BasicObject, ISqlStringable {

        // 数据库连接
        private Connection _dbc;

        // 表对象
        private SqlUnits.Table _table;

        // 更新键字段对象
        private SqlUnits.TableField _keyField;

        // 行数据对象
        private Row _row;

        // 筛选条件
        private SqlUnits.Formula _where;

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return true; } set { } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="dbc">数据库连接</param>
        /// <param name="table">表对象</param>
        /// <param name="row">数据行对象</param>
        /// <param name="keyField">更新键字段</param>
        public Update(Connection dbc, SqlUnits.Table table, Row row, SqlUnits.TableField keyField = null) {
            _dbc = dbc;
            _table = table;
            _row = row;
            _keyField = keyField;
        }

        /// <summary>
        /// 设置筛选条件
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public Update Where(SqlUnits.Formula formula) {
            _where = formula;
            return this;
        }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string res = "UPDATE ";
            res += _table.ToSqlString(tp);
            res += " SET ";

            string cols = "";
            string keyCol = "";
            if (!Equals(_keyField, null)) keyCol = _keyField.ToString();

            foreach (var key in _row.Keys) {
                if (key != keyCol) {
                    if (cols != "") cols += ",";
                    using (SqlUnits.TableField field = new SqlUnits.TableField(_table, key)) {
                        cols += field.ToSqlString(tp);
                    }
                    //cols += $" = '{_row[key]}'";
                    switch (tp) {
                        case DatabaseTypes.MySQL:
                            cols += $" = '{_row[key].Replace("'", "\'")}'";
                            break;
                        //return $"'{_value.Replace("'", "\'")}'";
                        case DatabaseTypes.Microsoft_Office_Access:
                        case DatabaseTypes.Microsoft_Office_Access_v12:
                        case DatabaseTypes.Microsoft_SQL_Server:
                        case DatabaseTypes.SQLite:
                        case DatabaseTypes.SQLite_3:
                        case DatabaseTypes.PostgreSQL:
                            cols += $" = '{_row[key].Replace("'", "''")}'";
                            break;
                        //return $"'{_value.Replace("'", "''")}'";
                        default:
                            throw new Exception($"尚未支持数据库 {tp.ToString()} 中的字符串转义。");
                    }
                }
            }

            res += cols;

            if (!keyCol.IsEmpty()) {
                res += " WHERE ";
                using (SqlUnits.TableField field = new SqlUnits.TableField(_table, keyCol)) {
                    res += field.ToSqlString(tp);
                    switch (tp) {
                        case DatabaseTypes.MySQL:
                            res += $" = '{_row[keyCol].Replace("'", "\'")}'";
                            break;
                        //return $"'{_value.Replace("'", "\'")}'";
                        case DatabaseTypes.Microsoft_Office_Access:
                        case DatabaseTypes.Microsoft_Office_Access_v12:
                        case DatabaseTypes.Microsoft_SQL_Server:
                        case DatabaseTypes.SQLite:
                        case DatabaseTypes.SQLite_3:
                        case DatabaseTypes.PostgreSQL:
                            res += $" = '{_row[keyCol].Replace("'", "''")}'";
                            break;
                        //return $"'{_value.Replace("'", "''")}'";
                        default:
                            throw new Exception($"尚未支持数据库 {tp.ToString()} 中的字符串转义。");
                    }
                    //res += $" = '{_row[keyCol]}'";
                }
            } else {
                if (!Equals(_where, null)) {
                    res += " WHERE " + _where.ToSqlString(tp);
                }
            }


            return res;
        }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <returns></returns>
        public string ToSqlString() { return ToSqlString(_dbc.DatabaseType); }

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <returns></returns>
        public int Exec() {
            return _dbc.Exec(this.ToSqlString());
        }
    }
}
