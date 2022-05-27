using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 区块
    /// </summary>
    public class MdBlock : MdBasicBlock {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdBlock() : base(MdTypes.Block, "> ") { }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[Block {0}]", base.Children.Count);
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("<blockquote>");
            foreach (var md in base.Children) {
                sb.Append(md.ToHtml());
            }
            sb.Append("</blockquote>");
            return sb.ToString();
        }

    }
}
