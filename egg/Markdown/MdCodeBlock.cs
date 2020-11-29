using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 代码块
    /// </summary>
    public class MdCodeBlock : MdBasicBlock {

        /// <summary>
        /// 获取或设置语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdCodeBlock() : base(MdTypes.CodeBlock) {
            this.Language = null;
        }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[CodeBlock {0}]", base.Children.Count);
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            return String.Format("```{0}\r\n{1}```\r\n", this.Language, base.Children.GetMarkdownString());
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"code-block\">");
            sb.Append("<ol start=\"1\">");
            foreach (var md in this.Children) {
                sb.Append("<li>");
                sb.Append(md.ToHtml());
                sb.Append("</li>");
            }
            sb.Append("</ol>");
            sb.Append("</div>");
            return sb.ToString();
        }

    }
}
