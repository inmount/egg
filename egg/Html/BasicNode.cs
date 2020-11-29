using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Html {

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType {

        /// <summary>
        /// 文本
        /// </summary>
        Text = 0x0,

        /// <summary>
        /// 元素节点
        /// </summary>
        Element = 0x10,

        /// <summary>
        /// 申明节点
        /// </summary>
        Declaration = 0x21,

        /// <summary>
        /// 注释
        /// </summary>
        Note = 0x22,

    }

    /// <summary>
    /// 基础节点
    /// </summary>
    public abstract class BasicNode : egg.Object {

        /// <summary>
        /// 获取节点类型
        /// </summary>
        public NodeType NodeType { get; private set; }

        /// <summary>
        /// 获取父节点
        /// </summary>
        public HtmlNode Parent { get; internal set; }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="nodeType"></param>
        public BasicNode(NodeType nodeType) {
            this.NodeType = nodeType;
            this.Parent = null;
        }

        /// <summary>
        /// 获取包含自身的完整XML字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetOuterHtml() { return ""; }

        /// <summary>
        /// 获取包含自身的完整XML字符串
        /// </summary>
        public string OuterHTML {
            get { return OnGetOuterHtml(); }
        }

        /// <summary>
        /// 获取包含的Html字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetInnerHtml() { return ""; }

        /// <summary>
        /// 获取包含的文本
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetInnerText() { return ""; }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        /// <returns></returns>
        protected virtual void OnSetInnerHtml(string xml) { throw new Exception("此节点不支持设置InnerHtml属性"); }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        public string InnerHTML {
            get { return OnGetInnerHtml(); }
            set { OnSetInnerHtml(value); }
        }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        public string InnerText {
            get { return OnGetInnerText(); }
        }

    }
}
