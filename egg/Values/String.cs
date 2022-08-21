using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Values {

    /// <summary>
    /// 字符串数据
    /// </summary>
    public class String : Value {

        /// <summary>
        /// 获取值
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="value"></param>
        public String(string value) {
            Value = value;
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <returns></returns>
        protected override bool OnCheckDouble() { return this.Value.IsDouble(); }

        /// <summary>
        /// 检查是否为空
        /// </summary>
        /// <returns></returns>
        protected override bool OnCheckEmpty() {
            return this.Value.IsEmpty();
        }

        /// <summary>
        /// 转化为布尔型
        /// </summary>
        /// <returns></returns>
        protected override bool OnParseBealoon() {
            return this.Value.IsTrue();
        }

        /// <summary>
        /// 转化为字节型
        /// </summary>
        /// <returns></returns>
        protected override byte OnParseByte() {
            return this.Value.ToByte();
        }

        /// <summary>
        /// 转化为整型
        /// </summary>
        /// <returns></returns>
        protected override int OnParseInteger() {
            return this.Value.ToInteger();
        }

        /// <summary>
        /// 转化为长整型
        /// </summary>
        /// <returns></returns>
        protected override long OnParseLong() {
            return this.Value.ToLong();
        }

        /// <summary>
        /// 转化为单精度
        /// </summary>
        /// <returns></returns>
        protected override float OnParseFloat() {
            return this.Value.ToFloat();
        }

        /// <summary>
        /// 转化为双精度
        /// </summary>
        /// <returns></returns>
        protected override double OnParseDouble() {
            return this.Value.ToDouble();
        }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return "" + this.Value;
        }
    }
}
