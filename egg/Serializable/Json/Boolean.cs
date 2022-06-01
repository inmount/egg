using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 布尔节点
    /// </summary>
    public class Boolean : ValueNode {

        /// <summary>
        /// 获取或设置内容
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// 实例化一个空节点
        /// </summary>
        public Boolean() : base(NodeTypes.Boolean) { }

        /// <summary>
        /// 转化为布尔型内容
        /// </summary>
        /// <returns></returns>
        protected override bool OnParseBoolean() {
            return this.Value;
        }

        /// <summary>
        /// 获取数值内容
        /// </summary>
        /// <returns></returns>
        protected override double OnParseNumber() {
            return this.Value ? 1 : 0;
        }

        /// <summary>
        /// 获取字符串内容
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return this.Value ? "true" : "false";
        }

        /// <summary>
        /// 获取序列化内容
        /// </summary>
        /// <returns></returns>
        protected override string OnSerialize() {
            return this.Value ? "true" : "false";
        }
    }
}
