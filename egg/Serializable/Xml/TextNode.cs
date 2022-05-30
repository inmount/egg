using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Xml {

    /// <summary>
    /// 文本节点
    /// </summary>
    public class TextNode : Node {

        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 设置带编码的值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public void SetEncodeValue(string val) {
            this.Value = eggs.Xml.EscapeDecode(val);
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public TextNode() : base(NodeType.Text) { }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterXml() {
            return eggs.Xml.EscapeEncode(this.Value);
        }

        /// <summary>
        /// 获取内置文本
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerText() {
            return this.Value;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            this.Value = null;

            base.OnDispose();
        }

    }
}
