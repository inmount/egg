using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Xml {

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum NodeType {
        /// <summary>
        /// 文本
        /// </summary>
        Text = 0x0,

        /// <summary>
        /// CDATA
        /// </summary>
        CData = 0x1,

        /// <summary>
        /// 标准节点
        /// </summary>
        Normal = 0x11,

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
        public XmlNode Parent { get; internal set; }

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
        protected virtual string OnGetOuterXml() { return ""; }

        /// <summary>
        /// 获取包含自身的完整XML字符串
        /// </summary>
        public string OuterXml {
            get { return OnGetOuterXml(); }
        }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetInnerXml() { return ""; }

        /// <summary>
        /// 获取包含的文本
        /// </summary>
        /// <returns></returns>
        protected virtual string OnGetInnerText() { return ""; }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        /// <returns></returns>
        protected virtual void OnSetInnerXml(string xml) { throw new Exception("此节点不支持设置InnerXml属性"); }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        public string InnerXml {
            get { return OnGetInnerXml(); }
            set { OnSetInnerXml(value); }
        }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        public string InnerText {
            get { return OnGetInnerText(); }
        }

    }
}
