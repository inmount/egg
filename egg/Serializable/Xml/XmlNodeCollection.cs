using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Xml {

    /// <summary>
    /// 节点集合
    /// </summary>
    public class XmlNodeCollection : List<Node>, IDisposable {

        private BasicObjectsMnanger objects;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="parent"></param>
        public XmlNodeCollection(XmlNode parent = null) {
            objects = new BasicObjectsMnanger(parent);
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="node"></param>
        public new void Add(Node node) {
            objects.Add(node);
            base.Add(node);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            this.Clear();
            objects.Dispose();
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        public new void Insert(int index, Node node) {
            objects.Add(node);
            base.Insert(index, node);
        }

    }
}
