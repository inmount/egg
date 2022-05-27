using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Html {

    /// <summary>
    /// Css设置单元
    /// </summary>
    public class HtmlCssItem : HtmlCssUnit {

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public HtmlCssItem() {
            this.Name = null;
            this.Content = null;
        }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            if (this.Name.IsNoneOrNull()) throw new Exception("名称不允许为空");
            if (this.Name == "/") {
                return $"/{this.Content}/";
            } else {
                return $"{this.Name}:{this.Content};";
            }
            //return base.OnParseString();
        }

    }
}
