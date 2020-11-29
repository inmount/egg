using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Markdown {

    /// <summary>
    /// 一级标题
    /// </summary>
    public class MdTitle : MdBasicBlock {

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 对象实例化
        /// <param name="lv"></param>
        /// </summary>
        public MdTitle(int lv) : base(MdTypes.Title) {
            this.Level = lv;
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[Title({0}) {1}]", this.Level, this.Children.Count);
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Level; i++) {
                sb.Append('#');
            }
            sb.Append(' ');
            sb.Append(this.Children.GetMarkdownString());
            sb.Append("\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// 获取HTML表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return String.Format("<h{1}>{0}</h{1}>", base.OnGetHtmlString(), this.Level);
        }

    }
}
