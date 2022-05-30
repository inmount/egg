using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Html {

    /// <summary>
    /// Css管理器
    /// </summary>
    public class HtmlCss : egg.BasicObject {

        /// <summary>
        /// 单元集合
        /// </summary>
        public HtmlCssUnitCollection Items { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public HtmlCss() : base() {
            this.Items = new HtmlCssUnitCollection();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public HtmlCss(string css) : base() {
            this.Items = new HtmlCssUnitCollection();
            eggs.Html.FillCss(this, css);
        }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.Items.Count; i++) {
                sb.AppendFormat("{0}\r\n", this.Items[i].ToString());
            }
            return sb.ToString();
        }

    }
}
