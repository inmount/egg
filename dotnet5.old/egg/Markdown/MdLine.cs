using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 文本对象
    /// </summary>
    public class MdLine : MdBasic {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdLine() : base(MdTypes.Line) { }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return "[Line]";
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            return "------\r\n";
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return "<hr />";
        }

    }
}
