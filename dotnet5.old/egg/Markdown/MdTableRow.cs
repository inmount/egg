using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 表格行
    /// </summary>
    public class MdTableRow : MdBasicBlock {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdTableRow() : base(MdTypes.TableRow) { }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            StringBuilder sb = new StringBuilder();
            foreach (var md in this.Children) {
                sb.Append("| ");
                sb.Append(md.ToMarkdown());
                sb.Append(" ");
            }
            sb.Append("|");
            return sb.ToString();
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            StringBuilder sb = new StringBuilder();
            MdTable mdTable = (MdTable)this.ParentBlock;
            sb.Append("<tr>");
            for (int i = 0; i < this.Children.Count; i++) {
                var md = this.Children[i];
                sb.AppendFormat("<td>{0}</td>", md.ToHtml());
            }
            sb.Append("</tr>");
            return sb.ToString();
        }

    }
}
