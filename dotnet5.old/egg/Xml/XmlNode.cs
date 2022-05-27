using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Xml {

    /// <summary>
    /// 文本节点
    /// </summary>
    public class XmlNode : BasicNode {

        /// <summary>
        /// 获取子节点集合
        /// </summary>
        public XmlNodeCollection Nodes { get; private set; }

        /// <summary>
        /// 获取属性集合
        /// </summary>
        public egg.InsensitiveKeyList<string> Attr { get; private set; }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetEncodeAttr(string key, string val) {
            this.Attr[key] = Parser.EscapeDecode(val);
        }

        /// <summary>
        /// 获取或设置是否为独立标签
        /// </summary>
        public bool IsSingle { get; set; }

        /// <summary>
        /// 获取名称
        /// </summary>
        public string TagName { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public XmlNode(string name) : base(NodeType.Normal) {
            this.TagName = name;
            this.Nodes = new XmlNodeCollection(this);
            this.Attr = new InsensitiveKeyList<string>();
        }

        // 获取包含的XML
        private string GetInnerXml() {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < this.Nodes.Count; i++) {
                res.Append(this.Nodes[i].OuterXml);
            }
            return res.ToString();
        }

        /// <summary>
        /// 获取完整XML
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterXml() {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("<{0}", this.TagName);
            // 拼接属性
            foreach (var key in this.Attr.Keys) {
                res.AppendFormat(" {0}=\"{1}\"", key, Parser.EscapeEncode(this.Attr[key]));
            }
            // 拼接完整XML
            if (this.IsSingle) {
                res.Append("/>");
            } else {
                res.AppendFormat(">{0}</{1}>", GetInnerXml(), this.TagName);
            }
            return res.ToString();
        }

        /// <summary>
        /// 获取包含
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerXml() {
            return GetInnerXml();
        }

        /// <summary>
        /// 设置包含XML
        /// </summary>
        /// <param name="xml"></param>
        protected override void OnSetInnerXml(string xml) {
            // 解析对象
            var nodes = Parser.GetNodes(xml, this);
            // 先释放资源
            for (int i = 0; i < this.Nodes.Count; i++) {
                this.Nodes[i].Dispose();
            }
            this.Nodes.Clear();
            // 重新设定子节点集合
            this.Nodes = nodes;
        }

        /// <summary>
        /// 获取内置文本
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerText() {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < this.Nodes.Count; i++) {
                res.Append(this.Nodes[i].InnerText);
            }
            return res.ToString();
        }

        /// <summary>
        /// 获取一个子节点
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public XmlNode this[string tagName] {
            get {
                tagName = tagName.ToLower();
                for (int i = 0; i < this.Nodes.Count; i++) {
                    if (this.Nodes[i].NodeType == NodeType.Normal) {
                        var node = (XmlNode)this.Nodes[i];
                        if (node.TagName.ToLower() == tagName) {
                            return node;
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取所有标签名节点
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="searchChildNodes"></param>
        /// <returns></returns>
        public List<XmlNode> GetNodesByTagName(string tagName, bool searchChildNodes = true) {
            List<XmlNode> nodes = new List<XmlNode>();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if (this.Nodes[i].NodeType == NodeType.Normal) {
                    var node = (XmlNode)this.Nodes[i];
                    if (node.TagName.ToLower() == tagName) {
                        nodes.Add(node);
                    }

                    // 深度查询
                    if (searchChildNodes) {
                        if (node.Nodes.Count > 0) {
                            // 获取子节点的查询结果并应用到结果中
                            var childNodes = node.GetNodesByTagName(tagName);
                            for (int j = 0; j < childNodes.Count; j++) {
                                nodes.Add(childNodes[j]);
                            }
                        }
                    }
                }
            }
            return nodes;
        }

        /// <summary>
        /// 获取所有满足属性限定的节点
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        /// <param name="searchChildNodes"></param>
        /// <returns></returns>
        public List<XmlNode> GetNodesByAttr(string attrName, string attrValue, bool searchChildNodes = true) {
            List<XmlNode> nodes = new List<XmlNode>();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if (this.Nodes[i].NodeType == NodeType.Normal) {
                    var node = (XmlNode)this.Nodes[i];
                    if (node.Attr[attrName] == attrValue) {
                        nodes.Add(node);
                    }

                    // 深度查询
                    if (searchChildNodes) {
                        if (node.Nodes.Count > 0) {
                            // 获取子节点的查询结果并应用到结果中
                            var childNodes = node.GetNodesByAttr(attrName, attrValue);
                            for (int j = 0; j < childNodes.Count; j++) {
                                nodes.Add(childNodes[j]);
                            }
                        }
                    }
                }
            }
            return nodes;
        }

        /// <summary>
        /// 获取第一个满足属性限定的节点
        /// </summary>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        /// <param name="searchChildNodes"></param>
        /// <returns></returns>
        public XmlNode GetNodeByAttr(string attrName, string attrValue, bool searchChildNodes = true) {
            List<XmlNode> nodes = new List<XmlNode>();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if (this.Nodes[i].NodeType == NodeType.Normal) {
                    var node = (XmlNode)this.Nodes[i];
                    if (node.Attr[attrName] == attrValue) {
                        return node;
                    }

                    // 深度查找
                    if (searchChildNodes) {
                        if (node.Nodes.Count > 0) {
                            // 获取子节点的查询结果并应用到结果中
                            var childNode = node.GetNodeByAttr(attrName, attrValue);
                            if (childNode != null) return childNode;
                        }
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// 添加一个新的标准节点
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public XmlNode AddNode(string tagName) {
            XmlNode node = new XmlNode(tagName);
            this.Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// 添加一个注释
        /// </summary>
        /// <returns></returns>
        public NoteNode AddNote() {
            NoteNode node = new NoteNode();
            this.Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();

            for (int i = 0; i < this.Nodes.Count; i++) {
                this.Nodes[i].Dispose();
            }

            this.Nodes.Clear();
            this.Attr.Dispose();

        }

    }
}
