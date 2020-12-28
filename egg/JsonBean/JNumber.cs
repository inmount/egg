using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {

    /// <summary>
    /// Json专用数字
    /// </summary>
    public class JNumber : IUnit {

        #region [=====接口实现====]

        private IUnit _parent;

        /// <summary>
        /// 设置父对象
        /// </summary>
        /// <param name="p"></param>
        public void SetParent(IUnit p) { _parent = p; }

        /// <summary>
        /// 获取父对象
        /// </summary>
        public IUnit GetParent() { return _parent; }

        /// <summary>
        /// 获取单元类型
        /// </summary>
        public UnitTypes GetUnitType() { return _null ? UnitTypes.None : UnitTypes.Number; }

        #endregion

        private double value;
        private bool _null;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public JNumber() {
            value = 0;
            _null = true;
        }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="val"></param>
        public JNumber(double val = 0) {
            value = val;
            _null = false;
        }

        /// <summary>
        /// 快速创建一个数值对象
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static JNumber Create(double val) { return new JNumber(val); }

        /// <summary>
        /// 转化为双精度数据
        /// </summary>
        /// <returns></returns>
        public double ToDouble() { return value; }

        /// <summary>
        /// 获取Json表示形式
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            return value.ToString();
        }

        /// <summary>
        /// 创建一个同样内容的副本
        /// </summary>
        /// <returns></returns>
        public IUnit Clone() { return new JNumber(value); }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Free() {
            value = 0;
            _null = true;
        }

        #region [=====重载运算符=====]

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num"></param>
        public static implicit operator JNumber(double num) {
            return new JNumber(num);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num"></param>
        public static implicit operator JNumber(int num) {
            return new JNumber(num);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num"></param>
        public static implicit operator JNumber(long num) {
            return new JNumber(num);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num"></param>
        public static implicit operator JNumber(float num) {
            return new JNumber(num);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="str"></param>
        public static implicit operator double(JNumber str) {
            return str.ToDouble();
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator +(JNumber num1, JNumber num2) { return new JNumber(num1.ToDouble() + num2.ToDouble()); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator +(JNumber num1, double num2) { return new JNumber(num1.ToDouble() + num2); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator +(double num1, JNumber num2) { return new JNumber(num1 + num2.ToDouble()); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator -(JNumber num1, JNumber num2) { return new JNumber(num1.ToDouble() - num2.ToDouble()); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator -(JNumber num1, double num2) { return new JNumber(num1.ToDouble() - num2); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator -(double num1, JNumber num2) { return new JNumber(num1 - num2.ToDouble()); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator *(JNumber num1, JNumber num2) { return new JNumber(num1.ToDouble() * num2.ToDouble()); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator *(JNumber num1, double num2) { return new JNumber(num1.ToDouble() * num2); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator *(double num1, JNumber num2) { return new JNumber(num1 * num2.ToDouble()); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator /(JNumber num1, JNumber num2) { return new JNumber(num1.ToDouble() / num2.ToDouble()); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator /(JNumber num1, double num2) { return new JNumber(num1.ToDouble() / num2); }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static JNumber operator /(double num1, JNumber num2) { return new JNumber(num1 / num2.ToDouble()); }

        #endregion

    }
}
