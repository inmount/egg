using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// 表格头部数据
    /// </summary>
    public class MdTableHeader : MdBasicBlock {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdTableHeader() : base(MdTypes.TableHeader) { }

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
                string align = null;
                if (mdTable.Children.Count >= 2) {
                    MdTableAlign mdTableAlign = (MdTableAlign)mdTable.Children[1];
                    if (mdTableAlign.Children.Count > i) {
                        align = mdTableAlign.Children[i].ToString();
                    }
                }
                if (align.IsEmpty()) {
                    sb.AppendFormat("<th>{0}</th>", md.ToHtml());
                } else {
                    sb.AppendFormat("<th align=\"{0}\">{1}</th>", align, md.ToHtml());
                }
            }
            sb.Append("</tr>");
            return sb.ToString();
        }

    }
}
