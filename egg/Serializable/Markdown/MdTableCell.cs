using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// 表格行
    /// </summary>
    public class MdTableCell : MdBasicContent {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdTableCell() : base(MdTypes.TableCell) { }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            return String.Format("| {0} ", eggs.Markdown.Escape(base.Content));
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return String.Format("<td>{0}</td>", base.Content);
        }

    }
}
