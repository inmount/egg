using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// 文本对象
    /// </summary>
    public class MdText : MdBasicContent {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdText() : base(MdTypes.Text) { }

        /// <summary>
        /// 获取或设置是否为代码
        /// </summary>
        public bool IsCode { get; set; }

        /// <summary>
        /// 获取或设置是否为粗体
        /// </summary>
        public bool IsBold { get; set; }

        /// <summary>
        /// 获取或设置是否为斜体
        /// </summary>
        public bool IsItalic { get; set; }

        /// <summary>
        /// 获取或设置是否带删除线
        /// </summary>
        public bool IsStrikethrough { get; set; }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            if (this.ParentBlock.Type == MdTypes.CodeLine) {
                return base.Content;
            } else {
                StringBuilder sb = new StringBuilder();
                if (IsStrikethrough) sb.Append("~~");
                if (IsBold) sb.Append("**");
                if (IsItalic) sb.Append("*");
                if (IsCode) sb.Append("`");
                sb.Append(eggs.Markdown.Escape(base.Content));
                if (IsCode) sb.Append("`");
                if (IsItalic) sb.Append("*");
                if (IsBold) sb.Append("**");
                if (IsStrikethrough) sb.Append("~~");
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            if (this.ParentBlock.Type == MdTypes.CodeLine) {
                return base.Content;
            } else {
                StringBuilder sb = new StringBuilder();
                if (IsStrikethrough) sb.Append("<del>");
                if (IsBold) sb.Append("<b>");
                if (IsItalic) sb.Append("<i>");
                if (IsCode) sb.Append("<code>");
                if (!(IsBold || IsStrikethrough || IsItalic || IsCode)) sb.Append("<span>");
                sb.Append(base.Content);
                if (!(IsBold || IsStrikethrough || IsItalic || IsCode)) sb.Append("</span>");
                if (IsCode) sb.Append("</code>");
                if (IsItalic) sb.Append("</i>");
                if (IsBold) sb.Append("</b>");
                if (IsStrikethrough) sb.Append("</del>");
                return sb.ToString();
            }
        }

    }
}
