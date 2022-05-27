using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 代码行
    /// </summary>
    public class MdCodeLine : MdTextLine {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdCodeLine() : base(MdTypes.CodeLine) { }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[CodeLine {0}]", base.Children.Count);
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            return String.Format("    {0}\r\n", base.Children.GetMarkdownString());
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return String.Format("<code>{0}</code>", base.Children.GetHtmlString());
        }

    }
}
