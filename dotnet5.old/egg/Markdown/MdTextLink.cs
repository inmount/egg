using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 超链接
    /// </summary>
    public class MdTextLink : MdBasicContent {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdTextLink() : base(MdTypes.TextLink) { }

        /// <summary>
        /// 获取或设置链接地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            return String.Format("[{0}]({1})", Parser.Escape(base.Content), this.Url);
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return String.Format("<a href=\"{0}\">{1}</a>", this.Url, base.Content);
        }

    }
}
