using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Xml {

    /// <summary>
    /// 文本节点
    /// </summary>
    public class NoteNode : Node {

        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public NoteNode() : base(NodeType.Note) { }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterXml() {
            return String.Format("<!--{0}-->", this.Note);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            this.Note = null;

            base.OnDispose();
        }

    }
}
