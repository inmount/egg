using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {

    /// <summary>
    /// Json专用的空类型
    /// </summary>
    public class JNull : Object, IUnit {

        private IUnit _parent;
        private Type _type;

        /// <summary>
        /// 获取一个空类型实例
        /// </summary>
        public static JNull Create(Type tp) { return new JNull(tp); }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="tp"></param>
        public JNull(Type tp) {
            _type = tp;
        }

        /// <summary>
        /// 获取内置类型
        /// </summary>
        public Type Type { get { return _type; } }

        /// <summary>
        /// 获取父对象
        /// </summary>
        /// <returns></returns>
        public IUnit GetParent() { return _parent; }

        /// <summary>
        /// 获取单元类型
        /// </summary>
        /// <returns></returns>
        public UnitTypes GetUnitType() { return UnitTypes.None; }

        /// <summary>
        /// 设置父对象
        /// </summary>
        /// <param name="p"></param>
        public void SetParent(IUnit p) { _parent = p; }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson() {
            return "Null";
        }

        /// <summary>
        /// 创建一个同样内容的副本
        /// </summary>
        /// <returns></returns>
        public IUnit Clone() { return new JNull(_type); }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Free() { }

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
            return null;
        }

        /// <summary>
        /// 获取字符串表现形式
        /// </summary>
        /// <returns></returns>
        public string GetString() { return null; }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public double GetNumber() { return 0; }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="val"></param>
        public static implicit operator JString(JNull val) {
            return new JString(null);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="val"></param>
        public static implicit operator JNumber(JNull val) {
            return new JNumber(0);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="val"></param>
        public static implicit operator JBoolean(JNull val) {
            return new JBoolean(false);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="val"></param>
        public static implicit operator JObject(JNull val) {
            return new JObject(val._type);
        }

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="val"></param>
        public static implicit operator JArray(JNull val) {
            return new JArray(true);
        }
    }
}
