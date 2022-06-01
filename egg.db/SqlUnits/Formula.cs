using System;
using System.Collections.Generic;
using System.Text;

namespace egg.db.SqlUnits {

    /// <summary>
    /// 算式
    /// </summary>
    public class Formula : egg.BasicObject, ISqlStringable {

        // 运算符左侧对象，一般带有字段性质
        private ISqlStringable _left;

        // 运算符
        private string _operator;

        // 运算符右侧对象，一般带有字段性质
        private ISqlStringable _right;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="left"></param>
        /// <param name="opt"></param>
        /// <param name="right"></param>
        public Formula(ISqlStringable left, string opt, ISqlStringable right) {
            _left = left;
            _operator = opt;
            _right = right;
            this.IsComplicated = false;
        }

        #region [=====运算符重载及常用运算处理=====]

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator &(Formula left, ISqlStringable right) {
            if (eggs.Object.IsNull(left)) {
                return (Formula)right;
            } else {
                return new Formula(left, "AND", right) { IsComplicated = true };
            }
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator |(Formula left, ISqlStringable right) {
            if (eggs.Object.IsNull(left)) {
                return (Formula)right;
            } else {
                return new Formula(left, "OR", right) { IsComplicated = true };
            }
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator +(Formula left, ISqlStringable right) {
            return new Formula(left, "+", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator +(Formula left, double right) {
            return new Formula(left, "+", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator +(Formula left, string right) {
            return new Formula(left, "+", new String(right)) { IsComplicated = true };
        }


        /// <summary>
        /// 重载-运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator -(Formula left, ISqlStringable right) {
            return new Formula(left, "-", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载-运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator -(Formula left, double right) {
            return new Formula(left, "-", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载-运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator -(Formula left, string right) {
            return new Formula(left, "-", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载*运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator *(Formula left, ISqlStringable right) {
            return new Formula(left, "*", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载*运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator *(Formula left, double right) {
            return new Formula(left, "*", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载*运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator *(Formula left, string right) {
            return new Formula(left, "*", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载/运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator /(Formula left, ISqlStringable right) {
            return new Formula(left, "/", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator /(Formula left, double right) {
            return new Formula(left, "/", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载+运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator /(Formula left, string right) {
            return new Formula(left, "/", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator %(Formula left, ISqlStringable right) {
            return new Formula(left, "%", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator %(Formula left, double right) {
            return new Formula(left, "%", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator %(Formula left, string right) {
            return new Formula(left, "%", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载==运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator ==(Formula left, ISqlStringable right) {
            return new Formula(left, "=", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载==运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator ==(Formula left, double right) {
            return new Formula(left, "=", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载==运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator ==(Formula left, string right) {
            return new Formula(left, "=", new String(right)) { IsComplicated = true };
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
        /// 重载!=运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator !=(Formula left, ISqlStringable right) {
            return new Formula(left, "<>", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载!=运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator !=(Formula left, double right) {
            return new Formula(left, "<>", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载!=运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator !=(Formula left, string right) {
            return new Formula(left, "<>", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >(Formula left, ISqlStringable right) {
            return new Formula(left, ">", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >(Formula left, double right) {
            return new Formula(left, ">", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >(Formula left, string right) {
            return new Formula(left, ">", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <(Formula left, ISqlStringable right) {
            return new Formula(left, "<", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <(Formula left, double right) {
            return new Formula(left, "<", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <(Formula left, string right) {
            return new Formula(left, "<", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >=(Formula left, ISqlStringable right) {
            return new Formula(left, ">=", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >=(Formula left, double right) {
            return new Formula(left, ">=", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator >=(Formula left, string right) {
            return new Formula(left, ">=", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <=(Formula left, ISqlStringable right) {
            return new Formula(left, "<=", right) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <=(Formula left, double right) {
            return new Formula(left, "<=", new Number(right)) { IsComplicated = true };
        }

        /// <summary>
        /// 重载运算符
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>        
        public static Formula operator <=(Formula left, string right) {
            return new Formula(left, "<=", new String(right)) { IsComplicated = true };
        }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula In(ISqlStringable right) { return new Formula(this, "IN", right) { IsComplicated = true }; }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula In(params double[] right) { return new Formula(this, "IN", new Numbers(right)) { IsComplicated = true }; }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula In(params string[] right) { return new Formula(this, "IN", new Strings(right)) { IsComplicated = true }; }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula NotIn(ISqlStringable right) { return new Formula(this, "NOT IN", right) { IsComplicated = true }; }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula NotIn(params double[] right) { return new Formula(this, "NOT IN", new Numbers(right)) { IsComplicated = true }; }

        /// <summary>
        /// IN操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula NotIn(params string[] right) { return new Formula(this, "NOT IN", new Strings(right)) { IsComplicated = true }; }

        /// <summary>
        /// IS操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula Is(string right) { return new Formula(this, "IS", new String(right)) { IsComplicated = true }; }

        /// <summary>
        /// IS NOT操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula IsNot(string right) { return new Formula(this, "IS NOT", new String(right)) { IsComplicated = true }; }

        /// <summary>
        /// LIKE操作
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Formula Like(string right) { return new Formula(this, "LIKE", new String(right)) { IsComplicated = true }; }

        #endregion

        /// <summary>
        /// 获取是否为复杂对象
        /// </summary>
        public bool IsComplicated { get; set; }

        /// <summary>
        /// 获取标准字段SQL字符串
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="multiTable"></param>
        /// <returns></returns>
        public string ToSqlString(DatabaseTypes tp, bool multiTable = false) {
            string left = _left.IsComplicated ? $"({_left.ToSqlString(tp, multiTable)})" : _left.ToSqlString(tp, multiTable);
            string right = _right.IsComplicated ? $"({_right.ToSqlString(tp, multiTable)})" : _right.ToSqlString(tp, multiTable);
            return $"{left} {_operator} {right}";
        }

    }
}
