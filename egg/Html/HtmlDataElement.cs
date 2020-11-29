using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Html {

    /// <summary>
    /// 数据元素
    /// </summary>
    public class HtmlDataElement : HtmlElement {

        /// <summary>
        /// 获取或设置关联数据
        /// </summary>
        public string Data { get; set; }

        // 隐藏子对象
        private new HtmlElementCollection Children { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="name"></param>
        public HtmlDataElement(string name) : base(name) { }

        /// <summary>
        /// 获取包含的Html代码
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerHtml() {
            return this.Data;
        }

        /// <summary>
        /// 获取包含的文本
        /// </summary>
        /// <returns></returns>
        protected override string OnGetInnerText() {
            return this.Data;
        }

        /// <summary>
        /// 设置包含的HTML代码
        /// </summary>
        /// <param name="xml"></param>
        protected override void OnSetInnerHtml(string xml) {
            this.Data = xml;
        }

    }
}
