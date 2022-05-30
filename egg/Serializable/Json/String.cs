using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Json {

    /// <summary>
    /// 字符串节点
    /// </summary>
    public class String : ValueNode {

        /// <summary>
        /// 获取或设置内容
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 实例化一个空节点
        /// </summary>
        public String() : base(NodeTypes.String) { }

        // 转化为布尔型内容
        protected override bool OnParseBoolean() {
            if (this.Value.IsDouble()) return this.Value.ToDouble() > 0;
            string sz = this.Value.ToLower();
            if (sz.Equals("true")) return true;
            if (sz.Equals("false")) return false;
            return base.OnParseBoolean();
        }

        // 获取字符串内容
        protected override double OnParseNumber() {
            if (this.Value.IsDouble()) return this.Value.ToDouble();
            return base.OnParseNumber();
        }

        // 获取字符串内容
        protected override string OnParseString() { return this.Value; }

        // 获取序列化内容
        protected override string OnSerialize() {
            return eggs.Json.GetJsonString(this.Value);
        }
    }
}
