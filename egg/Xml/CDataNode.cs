using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Xml {

    /// <summary>
    /// 文本节点
    /// </summary>
    public class CDataNode : BasicNode {

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public CDataNode() : base(NodeType.CData) { }

        /// <summary>
        /// 获取完整XML
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterXml() {
            return String.Format("<![CDATA[{0}]]>", this.Value);
        }

        /// <summary>
        /// 获取包含
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerXml() {
            return this.Value;
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
