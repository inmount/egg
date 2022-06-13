using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Xml {

    /// <summary>
    /// 文档对象
    /// </summary>
    public class XmlDocument : Serializable.Document {

        /// <summary>
        /// 获取子节点集合
        /// </summary>
        public List<Node> Nodes { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public XmlDocument() {
            this.Nodes = new List<Node>();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="xml"></param>
        public XmlDocument(string xml) {
            if (!xml.IsEmpty()) {
                this.Nodes = eggs.Xml.GetNodes(xml);
            } else {
                this.Nodes = new List<Node>();
            }
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

                    // 是否深度检索
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
        /// 获取包含的XML字符串
        /// </summary>
        public string InnerXml {
            get {
                StringBuilder res = new StringBuilder();
                for (int i = 0; i < this.Nodes.Count; i++) {
                    res.Append(this.Nodes[i].OuterXml);
                }
                return res.ToString();
            }
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
        /// 反序列化内容填充
        /// </summary>
        /// <param name="bytes"></param>
        protected override void OnDeserialize(Span<byte> bytes) {
            if (bytes.Length > 0) {
                this.Nodes.Clear();
                this.Nodes = eggs.Xml.GetNodes(System.Text.Encoding.UTF8.GetString(bytes));
            } else {
                this.Nodes.Clear();
                this.Nodes = new List<Node>();
            }
        }

        /// <summary>
        /// 内容序列化到字节数组
        /// </summary>
        /// <returns></returns>
        protected override byte[] OnSerializeToBytes() {
            return System.Text.Encoding.UTF8.GetBytes(this.InnerXml);
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
        }
    }
}
