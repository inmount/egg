using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 有序列表
    /// </summary>
    public class MdList : MdBasicBlock {

        /// <summary>
        /// 获取或设置是否为有序列表
        /// </summary>
        public bool IsOrdered { get; set; }

        /// <summary>
        /// 获取或设置序号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 对象初始化
        /// </summary>
        public MdList() : base(MdTypes.List, "    ") {
            this.SerialNumber = 1;
            this.IsOrdered = false;
        }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[List({0}) {1}]", this.IsOrdered, base.Children.Count);
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            StringBuilder sb = new StringBuilder();
            if (this.IsOrdered) {
                sb.AppendFormat("<ol start=\"{0}\">", this.SerialNumber);
            } else {
                sb.Append("<ul>");
            }
            sb.Append(base.Children.GetHtmlString());
            if (this.IsOrdered) {
                sb.Append("</ol>");
            } else {
                sb.Append("</ul>");
            }
            return sb.ToString();
        }

    }
}
