using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Html {

    /// <summary>
    /// 文本节点
    /// </summary>
    public class HtmlNode : Node {

        /// <summary>
        /// 获取子节点集合
        /// </summary>
        public HtmlNodeCollection Nodes { get; protected set; }

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
            this.Attr[key] = eggs.Html.EscapeDecode(val);
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
        public HtmlNode(string name) : base(NodeType.Element) {
            this.TagName = name;
            this.Nodes = new HtmlNodeCollection(this);
            this.Attr = new InsensitiveKeyList<string>();
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
        /// 获取所有标签名节点
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="searchChildNodes"></param>
        /// <returns></returns>
        public List<HtmlNode> GetNodesByTagName(string tagName, bool searchChildNodes = true) {
            List<HtmlNode> nodes = new List<HtmlNode>();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if ((this.Nodes[i].NodeType & NodeType.Element) == NodeType.Element) {
                    var node = (HtmlNode)this.Nodes[i];
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
        public List<HtmlNode> GetNodesByAttr(string attrName, string attrValue, bool searchChildNodes = true) {
            List<HtmlNode> nodes = new List<HtmlNode>();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if ((this.Nodes[i].NodeType & NodeType.Element) == NodeType.Element) {
                    var node = (HtmlNode)this.Nodes[i];
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
        public HtmlNode GetNodeByAttr(string attrName, string attrValue, bool searchChildNodes = true) {
            List<HtmlNode> nodes = new List<HtmlNode>();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if ((this.Nodes[i].NodeType & NodeType.Element) == NodeType.Element) {
                    var node = (HtmlNode)this.Nodes[i];
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
