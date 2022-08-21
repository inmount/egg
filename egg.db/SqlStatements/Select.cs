using System;
using System.Collections.Generic;
using System.Text;
using egg.db.SqlUnits;

namespace egg.db.SqlStatements {

    /// <summary>
    /// 查询对象
    /// </summary>
    public class Select : egg.BasicObject, ISqlStringable, ISqlAsable {

        // 数据库连接
        private Connection _dbc;

        // 字段对象
        private ISqlStringable[] _sqlFields;

        // 表格对象
        private ISqlTableStringable[] _sqlTables;

        // 筛选条件
        private SqlUnits.Formula _where;

        // 排序条件
        private List<SqlUnits.SelectOrder> _order;

        // 组合对象
        private ISqlStringable[] _group;

        // 游标设置
        private SqlUnits.TableField _limitFeild;
        private int _limitIndex;
        private int _limitLength;

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get { return true; } set { } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="sqlTables"></param>
        public Select(Connection dbc, params ISqlTableStringable[] sqlTables) {
            _dbc = dbc;
            _sqlTables = sqlTables;
            _sqlFields = null;
            _where = null;
            _order = new List<SqlUnits.SelectOrder>();
            _group = null;
            _limitFeild = null;
            _limitIndex = 0;
            _limitLength = 0;
        }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <returns></returns>
        public string ToSqlString() { return ToSqlString(_dbc.DatabaseType); }

        /// <summary>
        /// 设置返回字段
        /// </summary>
        /// <param name="sqlFields"></param>
        /// <returns></returns>
        public Select Columns(params ISqlStringable[] sqlFields) {
            _sqlFields = sqlFields;
            return this;
        }

        /// <summary>
        /// 设置筛选条件
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public Select Where(SqlUnits.Formula formula) {
            _where = formula;
            return this;
        }

        /// <summary>
        /// 设置升序条件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Select Asc(ISqlStringable obj) {
            _order.Add(new SqlUnits.SelectOrder(obj, "ASC"));
            return this;
        }

        /// <summary>
        /// 设置升序条件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Select Desc(ISqlStringable obj) {
            _order.Add(new SqlUnits.SelectOrder(obj, "Desc"));
            return this;
        }

        /// <summary>
        /// 设置返回字段
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public Select Group(params ISqlStringable[] objs) {
            _group = objs;
            return this;
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public As As(string name) { return new As(this, name); }

        /// <summary>
        /// 设置游标
        /// </summary>
        /// <param name="field"></param>
        /// <param name="idx"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public Select Limit(SqlUnits.TableField field, int idx, int len) {
            _limitFeild = field;
            _limitIndex = idx;
            if (_limitIndex < 0) _limitIndex = 0;
            _limitLength = len;
            if (_limitLength < 0) _limitLength = 0;
            return this;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public Rows GetRows() {
            return _dbc.GetRows(this.ToSqlString());
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <returns></returns>
        public Row GetRow() {
            return _dbc.GetRow(this.ToSqlString());
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public Orm.Rows<T> GetRows<T>() where T : Orm.Row, new() {
            return _dbc.GetRows<T>(this.ToSqlString());
        }

        /// <summary>
        /// 获取单行数据
        /// </summary>
        /// <returns></returns>
        public T GetRow<T>() where T : Orm.Row, new() {
            return _dbc.GetRow<T>(this.ToSqlString());
        }

        /// <summary>
        /// 获取标准SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string res = "SELECT";

            // 字段列表字符串
            string fieldsSql = "";

            // 表列表字符串
            string tablesSql = "";

            // 筛选字符串
            string whereSql = "";

            // 排序列表字符串
            string ordersSql = "";

            // 组合列表字符串
            string groupsSql = "";

            // 生成字段列表字符串
            if (_sqlFields == null) {
                if (_sqlTables.Length > 1) throw new Exception("多表查询请指定返回字段");
                fieldsSql = "*";
            } else {
                multiTable = _sqlTables.Length > 1;
                for (int i = 0; i < _sqlFields.Length; i++) {
                    var field = _sqlFields[i];
                    if (fieldsSql != "") fieldsSql += ",";
                    fieldsSql += field.ToSqlString(tp, multiTable);
                }
            }

            // 判断浮标情况
            if (!Equals(_limitFeild, null)) {
                switch (tp) {
                    case DatabaseTypes.Microsoft_Office_Access:
                    case DatabaseTypes.Microsoft_Office_Access_v12:
                    case DatabaseTypes.Microsoft_SQL_Server:
                        res += $" TOP {_limitIndex} {_limitFeild.ToSqlString(tp, multiTable)}";
                        break;
                    default:
                        res += " " + fieldsSql;
                        break;
                }
            } else {
                res += " " + fieldsSql;
            }

            // 生成表列表字符串
            res += " FROM";
            for (int i = 0; i < _sqlTables.Length; i++) {
                var table = _sqlTables[i];
                if (tablesSql != "") tablesSql += ",";
                tablesSql += table.ToSqlTableString(tp);
            }
            res += " " + tablesSql;

            // 生成筛选条件
            if (!Equals(_where, null)) {
                whereSql = _where.ToSqlString(tp, multiTable);
                res += " WHERE " + whereSql;
            }

            // 生成组合条件
            if (_group != null) {
                res += " GROUP BY ";
                for (int i = 0; i < _group.Length; i++) {
                    if (groupsSql != "") groupsSql += ",";
                    groupsSql += _group[i].ToSqlString(tp, multiTable);
                }
                res += groupsSql;
            }

            // 生成排序条件
            if (_order.Count > 0) {
                res += " ORDER BY ";
                for (int i = 0; i < _order.Count; i++) {
                    if (ordersSql != "") ordersSql += ",";
                    ordersSql += _order[i].ToSqlString(tp, multiTable);
                }
                res += ordersSql;
            }

            // 游标模式
            if (!Equals(_limitFeild, null)) {
                switch (tp) {
                    case DatabaseTypes.Microsoft_Office_Access:
                    case DatabaseTypes.Microsoft_Office_Access_v12:
                    case DatabaseTypes.Microsoft_SQL_Server:
                        //具有游标的情况下，进行微软产品的子查询构建
                        #region [=====具有游标的情况下，进行微软产品的子查询构建=====]

                        //子查询
                        string szSelect2 = res;
                        string szSelect1 = "SELECT ";
                        string szTab = "Auto_Table_" + _dbc.GetNewIndex();

                        //From查询构建
                        szSelect1 += "TOP " + (_limitIndex + _limitLength);

                        //输出字段
                        if (fieldsSql != "") szSelect1 += " " + fieldsSql;

                        //添加表
                        if (tablesSql != "") szSelect1 += " FROM " + tablesSql;

                        //添加条件
                        if (whereSql != "") szSelect1 += " WHERE " + whereSql;

                        //添加聚合
                        if (groupsSql != "") szSelect1 += " GROUP BY " + groupsSql;

                        //添加排序
                        if (ordersSql != "") szSelect1 += " ORDER BY " + ordersSql;

                        //重建查询
                        res = "SELECT *";

                        //添加表
                        res += " FROM (" + szSelect1 + ") AS " + szTab;

                        //构架新的条件
                        res += " WHERE " + _limitFeild.ToSqlString(tp, false) + " NOT IN (" + szSelect2 + ")";

                        #endregion
                        break;
                    case DatabaseTypes.MySQL:
                    case DatabaseTypes.SQLite:
                    case DatabaseTypes.SQLite_3:
                        //将支持limit的数据库组织limit语句
                        res += " LIMIT " + _limitIndex + "," + _limitLength;
                        break;
                    case DatabaseTypes.PostgreSQL:
                        //将支持limit的数据库组织limit语句
                        res += " LIMIT " + _limitLength + " OFFSET " + _limitIndex;
                        break;
                    default:
                        throw new Exception("尚未支持[" + tp + "]数据库类型");
                }
            }

            return res;

        }

    }
}
