using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Html {

    /// <summary>
    /// 文本节点
    /// </summary>
    public class HtmlElement : HtmlNode {

        /// <summary>
        /// 获取子节点集合
        /// </summary>
        public HtmlElementCollection Children { get; private set; }

        /// <summary>
        /// 获取父节点
        /// </summary>
        public new HtmlElement Parent { get { return (HtmlElement)base.Parent; } internal set { base.Parent = value; } }

        /// <summary>
        /// 获取或设置名称属性
        /// </summary>
        public string Name { get { return base.Attr["name"]; } set { base.Attr["name"] = value; } }

        /// <summary>
        /// 获取或设置id属性
        /// </summary>
        public string Id { get { return base.Attr["id"]; } set { base.Attr["id"] = value; } }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public HtmlElement(string name) : base(name) {
            //base.Name = name;
            this.Parent = null;
            this.Children = new HtmlElementCollection(this);
        }

        // 获取包含的XML
        private string GetInnerHtml() {
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < this.Nodes.Count; i++) {
                res.Append(this.Nodes[i].OuterHTML);
            }
            return res.ToString();
        }

        /// <summary>
        /// 获取完整XML
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterHtml() {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("<{0}", this.TagName);
            // 拼接属性
            foreach (var key in this.Attr.Keys) {
                string value = this.Attr[key];
                if (value.IsNull()) {
                    res.AppendFormat(" {0}", key);
                } else {
                    res.AppendFormat(" {0}=\"{1}\"", key, value);
                }
            }
            // 拼接完整XML
            if (this.IsSingle) {
                res.Append("/>");
            } else {
                res.AppendFormat(">{0}</{1}>", GetInnerHtml(), this.TagName);
            }
            return res.ToString();
        }

        /// <summary>
        /// 获取包含
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerHtml() {
            return GetInnerHtml();
        }

        /// <summary>
        /// 设置包含XML
        /// </summary>
        /// <param name="html"></param>
        protected override void OnSetInnerHtml(string html) {
            // 解析对象
            var nodes = eggs.Html.GetNodes(html, this);
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
