using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {

    /// <summary>
    /// Json专用字符串
    /// </summary>
    public class JString : Object, IUnit {

        private string value;

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
        public UnitTypes GetUnitType() { return value.IsNull() ? UnitTypes.None : UnitTypes.String; }

        #endregion

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="val"></param>
        public JString(string val = null) {
            value = val;
        }

        /// <summary>
        /// 快速创建一个字符串对象
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static JString Create(string val) { return new JString(val); }

        /// <summary>
        /// 获取一个空字符串
        /// </summary>
        public static JNull Null { get { return new JNull(typeof(JString)); } }

        /// <summary>
        /// 获取Json表示形式
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            if (value == null) return "NULL";
            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            sb.Append(value.Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n"));
            sb.Append('"');
            return sb.ToString();
        }

        /// <summary>
        /// 创建一个同样内容的副本
        /// </summary>
        /// <returns></returns>
        public IUnit Clone() { return new JString(value); }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Free() { value = null; }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            this.Free();
            base.OnDispose();
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return value;
        }

        /// <summary>
        /// 获取字符串表现形式
        /// </summary>
        /// <returns></returns>
        public string GetString() { return value; }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public double GetNumber() { return value.ToDouble(); }

        #region [=====重载运算符=====]

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="str"></param>
        public static implicit operator JString(string str) {
            return new JString(str);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="str"></param>
        public static implicit operator string(JString str) {
            if (eggs.IsNull(str)) return null;
            return str.value;
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static JString operator +(JString str1, JString str2) {
            StringBuilder sb = new StringBuilder();
            sb.Append(str1);
            sb.Append(str2);
            return new JString(sb.ToString());
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static JString operator +(JString str1, string str2) {
            StringBuilder sb = new StringBuilder();
            sb.Append(str1);
            sb.Append(str2);
            return new JString(sb.ToString());
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static JString operator +(string str1, JString str2) {
            StringBuilder sb = new StringBuilder();
            sb.Append(str1);
            sb.Append(str2);
            return new JString(sb.ToString());
        }

        #endregion

    }
}
