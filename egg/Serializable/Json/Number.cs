using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 数字节点
    /// </summary>
    public class Number : ValueNode {

        /// <summary>
        /// 获取或设置内容
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 实例化一个空节点
        /// </summary>
        public Number() : base(NodeTypes.Number) { }

        /// <summary>
        /// 转化为布尔型内容
        /// </summary>
        /// <returns></returns>
        protected override bool OnParseBoolean() {
            return this.Value > 0;
        }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        protected override double OnParseNumber() {
            return this.Value;
        }

        /// <summary>
        /// 获取字符串内容
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return "" + this.Value;
        }

        /// <summary>
        /// 获取序列化内容
        /// </summary>
        /// <returns></returns>
        protected override string OnSerialize() {
            return "" + this.Value;
        }
    }
}
