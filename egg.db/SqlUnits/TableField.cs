using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 表字段
    /// </summary>
    public class TableField : egg.BasicObject, ISqlStringable, ISqlAsable {

        // 关联表
        private Table _table;

        // 名称
        private string _name;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        public TableField(Table table, string name) {
            _table = table;
            _name = name;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public TableField(string name) {
            _table = null;
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
        /// 获取标准字段SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(egg.db.DatabaseTypes tp, bool multiTable = false) {
            // 当表不存在时，强制设置未单表模式
            if (_table == null) multiTable = false;
            // 判断是否为多表环境
            if (multiTable) {
                // 获取表SQL
                string table = _table.ToSqlString(tp);
                if (_name == "*") return $"{table}.*";
                switch (tp) {
                    case DatabaseTypes.Microsoft_Office_Access:
                    case DatabaseTypes.Microsoft_Office_Access_v12:
                    case DatabaseTypes.Microsoft_SQL_Server:
                    case DatabaseTypes.SQLite:
                    case DatabaseTypes.SQLite_3:
                        return $"{table}.[{_name}]";
                    case DatabaseTypes.MySQL:
                        return $"{table}.`{_name}`";
                    case DatabaseTypes.PostgreSQL:
                        return $"{table}.\"{_name}\"";
                    default:
                        throw new Exception($"尚未支持 {tp.ToString()} 类型数据库");
                }
            } else {
                if (_name == "*") return "*";
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
        }

        #region [=====运算符重载及常用运算处理=====]

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator +(TableField left, ISqlStringable right) {
            return new Formula(left, "+", right);
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator +(TableField left, double right) {
            return new Formula(left, "+", new Number(right));
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator +(TableField left, string right) {
            return new Formula(left, "+", new String(right));
        }


        /// <summary>
        /// 重载-运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator -(TableField left, ISqlStringable right) {
            return new Formula(left, "-", right);
        }

        /// <summary>
        /// 重载-运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator -(TableField left, double right) {
            return new Formula(left, "-", new Number(right));
        }

        /// <summary>
        /// 重载-运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator -(TableField left, string right) {
            return new Formula(left, "-", new String(right));
        }

        /// <summary>
        /// 重载*运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator *(TableField left, ISqlStringable right) {
            return new Formula(left, "*", right);
        }

        /// <summary>
        /// 重载*运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator *(TableField left, double right) {
            return new Formula(left, "*", new Number(right));
        }

        /// <summary>
        /// 重载*运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator *(TableField left, string right) {
            return new Formula(left, "*", new String(right));
        }

        /// <summary>
        /// 重载/运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator /(TableField left, ISqlStringable right) {
            return new Formula(left, "/", right);
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator /(TableField left, double right) {
            return new Formula(left, "/", new Number(right));
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator /(TableField left, string right) {
            return new Formula(left, "/", new String(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator %(TableField left, ISqlStringable right) {
            return new Formula(left, "%", right);
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator %(TableField left, double right) {
            return new Formula(left, "%", new Number(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator %(TableField left, string right) {
            return new Formula(left, "%", new String(right));
        }

        /// <summary>
        /// 重载==运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator ==(TableField left, ISqlStringable right) {
            return new Formula(left, "=", right);
        }

        /// <summary>
        /// 重载==运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator ==(TableField left, double right) {
            return new Formula(left, "=", new Number(right));
        }

        /// <summary>
        /// 重载==运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator ==(TableField left, string right) {
            return new Formula(left, "=", new String(right));
        }

        /// <summary>
        /// 重载!=运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator !=(TableField left, ISqlStringable right) {
            return new Formula(left, "<>", right);
        }

        /// <summary>
        /// 重载!=运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator !=(TableField left, double right) {
            return new Formula(left, "<>", new Number(right));
        }

        /// <summary>
        /// 重载!=运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator !=(TableField left, string right) {
            return new Formula(left, "<>", new String(right));
        }

        /// <summary>
        /// 判断是否为同一个对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) {
            return base.Equals(o);
        }

        /// <summary>
        /// 获取Hash值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >(TableField left, ISqlStringable right) {
            return new Formula(left, ">", right);
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >(TableField left, double right) {
            return new Formula(left, ">", new Number(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >(TableField left, string right) {
            return new Formula(left, ">", new String(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <(TableField left, ISqlStringable right) {
            return new Formula(left, "<", right);
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <(TableField left, double right) {
            return new Formula(left, "<", new Number(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <(TableField left, string right) {
            return new Formula(left, "<", new String(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >=(TableField left, ISqlStringable right) {
            return new Formula(left, ">=", right);
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >=(TableField left, double right) {
            return new Formula(left, ">=", new Number(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >=(TableField left, string right) {
            return new Formula(left, ">=", new String(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <=(TableField left, ISqlStringable right) {
            return new Formula(left, "<=", right);
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <=(TableField left, double right) {
            return new Formula(left, "<=", new Number(right));
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <=(TableField left, string right) {
            return new Formula(left, "<=", new String(right));
        }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula In(ISqlStringable right) { return new Formula(this, "IN", right); }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula In(params double[] right) { return new Formula(this, "IN", new Numbers(right)); }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula In(params string[] right) { return new Formula(this, "IN", new Strings(right)); }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula NotIn(ISqlStringable right) { return new Formula(this, "NOT IN", right); }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula NotIn(params double[] right) { return new Formula(this, "NOT IN", new Numbers(right)); }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula NotIn(params string[] right) { return new Formula(this, "NOT IN", new Strings(right)); }

        /// <summary>
        /// IS操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula Is(string right) { return new Formula(this, "IS", new String(right)); }

        /// <summary>
        /// IS NOT操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula IsNot(string right) { return new Formula(this, "IS NOT", new String(right)); }

        /// <summary>
        /// LIKE操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula Like(string right) { return new Formula(this, "LIKE", new String(right)); }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public As As(string name) { return new As(this, name); }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();

            _table = null;
            _name = null;
        }

    }
}
