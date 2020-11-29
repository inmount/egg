using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 表格
    /// </summary>
    public class MdTable : MdBasicBlock {

        /// <summary>
        /// 获取或设置序号
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdTable() : base(MdTypes.Table) {
            this.Row = 0;
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[Table {0}]", this.Children.Count);
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            foreach (var md in this.Children) {
                sb.Append(md.ToHtml());
            }
            sb.Append("</table>");
            return sb.ToString();
        }

    }
}
