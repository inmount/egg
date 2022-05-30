using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Html {

    /// <summary>
    /// 文档对象
    /// </summary>
    public class HtmlDocument : Serializable.Document {

        /// <summary>
        /// 获取Head元素
        /// </summary>
        public HtmlElement Head { get; private set; }

        /// <summary>
        /// 获取Body元素
        /// </summary>
        public HtmlElement Body { get; private set; }

        /// <summary>
        /// 获取子节点集合
        /// </summary>
        public HtmlNodeCollection Nodes { get; private set; }

        // 填充默认节点
        private void FillDefaultNode() {
            // 获取所有html标签
            var htmls = this.GetElementsByTagName("html", false);
            HtmlElement html;
            if (htmls.Count > 0) {
                html = htmls[0];
            } else {
                html = new HtmlElement("html");
                this.Nodes.Add(html);
            }
            // 填充head
            var heads = html.GetElementsByTagName("head", false);
            if (heads.Count > 0) {
                this.Head = heads[0];
            } else {
                this.Head = new HtmlElement("head");
                html.Children.Add(this.Head);
            }
            // 填充body
            var bodys = html.GetElementsByTagName("body", false);
            if (bodys.Count > 0) {
                this.Body = bodys[0];
            } else {
                this.Body = new HtmlElement("body");
                html.Children.Add(this.Body);
            }
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="html"></param>
        public HtmlDocument() {
            this.Nodes = new HtmlNodeCollection();
            // 填充默认节点
            this.FillDefaultNode();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="html"></param>
        public HtmlDocument(string html = null) {
            if (!html.IsEmpty()) {
                this.Nodes = eggs.Html.GetNodes(html);
            } else {
                this.Nodes = new HtmlNodeCollection();
            }

            // 填充默认节点
            this.FillDefaultNode();

        }

        /// <summary>
        /// 建立新元素
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public HtmlElement CreateElement(string tagName) {
            string tagNameLower = tagName.ToLower();
            if (tagNameLower == "style" || tagNameLower == "script") {
                return new HtmlDataElement(tagName);
            } else {
                return new HtmlElement(tagName);
            }
        }

        /// <summary>
        /// 获取所有标签名节点
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="searchChildNodes"></param>
        /// <returns></returns>
        public HtmlElementCollection GetElementsByTagName(string tagName, bool searchChildNodes = true) {
            HtmlElementCollection nodes = new HtmlElementCollection();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if (this.Nodes[i].NodeType == NodeType.Element) {
                    var node = (HtmlElement)this.Nodes[i];
                    if (node.TagName.ToLower() == tagName) {
                        nodes.Add(node);
                    }

                    // 深度查询
                    if (searchChildNodes) {
                        if (node.Nodes.Count > 0) {
                            // 获取子节点的查询结果并应用到结果中
                            var childNodes = node.GetElementsByTagName(tagName);
                            for (int j = 0; j < childNodes.Count; j++) {
                                nodes.Add(childNodes[j]);
                            }
                            childNodes.Clear();
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
        public HtmlElementCollection GetElementsByAttr(string attrName, string attrValue, bool searchChildNodes = true) {
            HtmlElementCollection nodes = new HtmlElementCollection();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if (this.Nodes[i].NodeType == NodeType.Element) {
                    var node = (HtmlElement)this.Nodes[i];
                    if (node.Attr[attrName] == attrValue) {
                        nodes.Add(node);
                    }

                    // 深度查询
                    if (searchChildNodes) {
                        if (node.Nodes.Count > 0) {
                            // 获取子节点的查询结果并应用到结果中
                            var childNodes = node.GetElementsByAttr(attrName, attrValue);
                            for (int j = 0; j < childNodes.Count; j++) {
                                nodes.Add(childNodes[j]);
                            }
                            childNodes.Clear();
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
        public HtmlElement GetElementByAttr(string attrName, string attrValue, bool searchChildNodes = true) {
            for (int i = 0; i < this.Nodes.Count; i++) {
                if (this.Nodes[i].NodeType == NodeType.Element) {
                    var node = (HtmlElement)this.Nodes[i];
                    if (node.Attr[attrName] == attrValue) {
                        return node;
                    }

                    // 深度查找
                    if (searchChildNodes) {
                        if (node.Nodes.Count > 0) {
                            // 获取子节点的查询结果并应用到结果中
                            var childNode = node.GetElementByAttr(attrName, attrValue);
                            if (childNode != null) return childNode;
                        }
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// 根据id查找元素
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HtmlElement GetElementById(string id) {
            return GetElementByAttr("id", id);
        }

        /// <summary>
        /// 根据id查找元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HtmlElementCollection GetElementsByClassName(string name) {

            string nameStart = $"{name} ";
            string nameIn = $" {name} ";
            string nameEnd = $" {name}";

            HtmlElementCollection nodes = new HtmlElementCollection();
            for (int i = 0; i < this.Nodes.Count; i++) {
                if (this.Nodes[i].NodeType == NodeType.Element) {
                    var node = (HtmlElement)this.Nodes[i];
                    string className = node.Attr["class"];
                    if (className == name || className.StartsWith(nameStart) || className.EndsWith(nameEnd) || className.IndexOf(nameIn) > 0) {
                        nodes.Add(node);
                    }

                    // 深度查询
                    if (node.Nodes.Count > 0) {
                        // 获取子节点的查询结果并应用到结果中
                        var childNodes = node.GetElementsByClassName(name);
                        for (int j = 0; j < childNodes.Count; j++) {
                            nodes.Add(childNodes[j]);
                        }
                        childNodes.Clear();
                    }
                }
            }
            return nodes;
        }

        /// <summary>
        /// 获取包含的XML字符串
        /// </summary>
        public string InnerHTML {
            get {
                StringBuilder res = new StringBuilder();
                for (int i = 0; i < this.Nodes.Count; i++) {
                    res.Append(this.Nodes[i].OuterHTML);
                }
                return res.ToString();
            }
        }

        // 反序列化内容填充
        protected override void OnDeserialize(Span<byte> bytes) {
            if (bytes.Length > 0) {
                this.Nodes.Clear();
                this.Nodes = eggs.Html.GetNodes(System.Text.Encoding.UTF8.GetString(bytes));
            } else {
                this.Nodes.Clear();
                this.Nodes = new HtmlNodeCollection();
            }
            // 填充默认节点
            this.FillDefaultNode();
        }

        // 内容序列化到字节数组
        protected override byte[] OnSerializeToBytes() {
            return System.Text.Encoding.UTF8.GetBytes(this.InnerHTML);
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
