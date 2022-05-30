using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Html {

    /// <summary>
    /// 网页元素集合
    /// </summary>
    public class HtmlElementCollection : List<HtmlElement> {

        private HtmlElement parentElement;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="parent"></param>
        public HtmlElementCollection(HtmlElement parent = null) {
            parentElement = parent;
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="element"></param>
        public new void Add(HtmlElement element) {
            if (parentElement != null) {
                element.Parent = parentElement;
                parentElement.Nodes.Add(element);
            }
            base.Add(element);
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="addNode"></param>
        public void Add(HtmlElement element, bool addNode) {
            if (parentElement != null) {
                element.Parent = parentElement;
                if (addNode) parentElement.Nodes.Add(element);
            }
            base.Add(element);
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="element"></param>
        public new void Insert(int index, HtmlElement element) {
            if (parentElement != null) {
                element.Parent = parentElement;

                // 查找元素节点的位置并执行节点插入
                var ele = parentElement.Children[index];
                for (int i = 0; i < parentElement.Nodes.Count; i++) {
                    if (parentElement.Nodes[i] == ele) {
                        parentElement.Nodes.Insert(i, element);
                        break;
                    }
                }

            }
            base.Insert(index, element);
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="element"></param>
        /// <param name="insertNode"></param>
        public void Insert(int index, HtmlElement element, bool insertNode) {
            if (parentElement != null) {
                element.Parent = parentElement;

                if (insertNode) {
                    // 查找元素节点的位置并执行节点插入
                    var ele = parentElement.Children[index];
                    for (int i = 0; i < parentElement.Nodes.Count; i++) {
                        if (parentElement.Nodes[i] == ele) {
                            parentElement.Nodes.Insert(i, element);
                            break;
                        }
                    }
                }

            }
            base.Insert(index, element);
        }

    }
}
