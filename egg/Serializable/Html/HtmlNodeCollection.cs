using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Html {

    /// <summary>
    /// 节点集合
    /// </summary>
    public class HtmlNodeCollection : List<Node> {

        /// <summary>
        /// 获取父对象
        /// </summary>
        public HtmlNode Parent { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="parent"></param>
        public HtmlNodeCollection(HtmlNode parent = null) {
            this.Parent = parent;
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="node"></param>
        public new void Add(Node node) {
            egg.Debug.WriteLine("Add");
            if (this.Parent != null) {
                egg.Debug.WriteLine("Add -> Parent is On");
                node.Parent = this.Parent;
            }
            base.Add(node);
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        public new void Insert(int index, Node node) {
            if (this.Parent != null) {
                node.Parent = this.Parent;
            }
            base.Insert(index, node);
        }

    }
}
