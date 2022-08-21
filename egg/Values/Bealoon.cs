using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Values {

    /// <summary>
    /// 布尔型数据
    /// </summary>
    public class Bealoon : Value {

        /// <summary>
        /// 获取值
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="value"></param>
        public Bealoon(bool value) {
            Value = value;
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <returns></returns>
        protected override bool OnCheckDouble() { return true; }

        /// <summary>
        /// 转化为布尔型
        /// </summary>
        /// <returns></returns>
        protected override bool OnParseBealoon() {
            return this.Value;
        }

        /// <summary>
        /// 转化为字节型
        /// </summary>
        /// <returns></returns>
        protected override byte OnParseByte() {
            return (byte)(this.Value ? 1 : 0);
        }

        /// <summary>
        /// 转化为整型
        /// </summary>
        /// <returns></returns>
        protected override int OnParseInteger() {
            return this.Value ? 1 : 0;
        }

        /// <summary>
        /// 转化为长整型
        /// </summary>
        /// <returns></returns>
        protected override long OnParseLong() {
            return this.Value ? 1 : 0;
        }

        /// <summary>
        /// 转化为单精度
        /// </summary>
        /// <returns></returns>
        protected override float OnParseFloat() {
            return this.Value ? 1 : 0;
        }

        /// <summary>
        /// 转化为双精度
        /// </summary>
        /// <returns></returns>
        protected override double OnParseDouble() {
            return this.Value ? 1 : 0;
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return this.Value ? "true" : "false";
        }
    }
}
