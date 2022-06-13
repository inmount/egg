using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Xml {

    /// <summary>
    /// 申明节点
    /// </summary>
    public class DeclarationNode : Node {


        /// <summary>
        /// 获取属性集合
        /// </summary>
        public egg.InsensitiveKeyList<string> Attr { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public DeclarationNode() : base(NodeType.Declaration) {
            this.Attr = new InsensitiveKeyList<string>();
        }

        /// <summary>
        /// 获取完整XML
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterXml() {
            StringBuilder res = new StringBuilder();
            res.Append("<?xml");
            // 拼接属性
            foreach (var key in this.Attr.Keys) {
                res.AppendFormat(" {0}=\"{1}\"", key, this.Attr[key]);
            }
            // 拼接完结尾ML
            res.Append("?>");
            return res.ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            this.Attr.Dispose();
        }

    }
}
