using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 无序列表项
    /// </summary>
    public class MdListItem : MdBasicBlock {

        /// <summary>
        /// 获取或设置序号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 对象初始化
        /// </summary>
        public MdListItem() : base(MdTypes.ListItem) {
            this.SerialNumber = 1;
        }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[ListItem {0}]\r\n", base.Children.Count);
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            MdList mdList = (MdList)this.ParentBlock;
            StringBuilder sb = new StringBuilder();
            if (mdList.IsOrdered) {
                sb.AppendFormat("{0}. ", this.SerialNumber);
            } else {
                sb.Append("+ ");
            }
            sb.Append(base.Children.GetMarkdownString());
            return sb.ToString();
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return String.Format("<li>{0}</li>", base.Children.GetHtmlString());
        }

    }
}
