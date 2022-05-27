using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Xml {

    /// <summary>
    /// 节点集合
    /// </summary>
    public class XmlNodeCollection : List<BasicNode> {

        private XmlNode parentNode;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="parent"></param>
        public XmlNodeCollection(XmlNode parent = null) {
            parentNode = parent;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="node"></param>
        public new void Add(BasicNode node) {
            if (parentNode != null) {
                node.Parent = parentNode;
            }
            base.Add(node);
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        public new void Insert(int index, BasicNode node) {
            if (parentNode != null) {
                node.Parent = parentNode;
            }
            base.Insert(index, node);
        }

    }
}
