using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Html {

    /// <summary>
    /// 申明节点
    /// </summary>
    public class DeclarationNode : Node {


        /// <summary>
        /// 获取或设置
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public DeclarationNode() : base(NodeType.Declaration) { }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        protected override string OnGetOuterHtml() {
            if (this.Content.IsEmpty()) {
                return "<!DOCTYPE>";
            }
            return String.Format("<!DOCTYPE {0}>", this.Content);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            this.Content = null;

            base.OnDispose();
        }

    }
}
