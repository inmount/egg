using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 文本行
    /// </summary>
    public class MdTextLine : MdBasicBlock {

        /// <summary>
        /// 是否为段落
        /// </summary>
        public bool IsSection { get; set; }

        /// <summary>
        /// 实例化对象
        /// <param name="mdType"></param>
        /// </summary>
        public MdTextLine(MdTypes mdType = MdTypes.TextLine) : base(mdType) {
            this.IsSection = false;
        }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return base.Children.GetString();
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Children.GetMarkdownString());
            sb.Append("\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            StringBuilder sb = new StringBuilder();
            if (IsSection) {
                sb.Append("<p>");
            }
            sb.Append(base.Children.GetHtmlString());
            if (IsSection) {
                sb.Append("</p>");
            }
            return sb.ToString();
        }

    }
}
