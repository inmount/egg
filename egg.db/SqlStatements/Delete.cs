using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlStatements {

    /// <summary>
    /// 数据更新语句
    /// </summary>
    public class Delete : egg.Object, ISqlStringable {

        // 数据库连接
        private Connection _dbc;

        // 表对象
        private SqlUnits.Table _table;

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
        public Delete(Connection dbc, SqlUnits.Table table) {
            _dbc = dbc;
            _table = table;
        }

        /// <summary>
        /// 设置筛选条件
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public Delete Where(SqlUnits.Formula formula) {
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
            string res = "DELETE FROM ";
            res += _table.ToSqlString(tp);

            if (!Equals(_where, null)) {
                res += " WHERE " + _where.ToSqlString(tp);
            }

            return res;
        }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <returns></returns>
        public string ToSqlString() { return ToSqlString(_dbc.DatabaseType); }

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <returns></returns>
        public int Exec() {
            return _dbc.Exec(this.ToSqlString());
        }
    }
}
