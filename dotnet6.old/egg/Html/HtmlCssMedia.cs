using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace egg.Html {

    /// <summary>
    /// 响应式片段
    /// </summary>
    public class HtmlCssMedia : HtmlCssUnit {

        /// <summary>
        /// 键名称集合
        /// </summary>
        public List<string> Keys { get; private set; }

        /// <summary>
        /// 单元集合
        /// </summary>
        public HtmlCssUnitCollection Items { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="names"></param>
        public HtmlCssMedia(string names = null) {
            this.Keys = new List<string>();
            this.Items = new HtmlCssUnitCollection();

            if (!names.IsNoneOrNull()) {
                string[] ks = names.Split(',');
                for (int i = 0; i < ks.Length; i++) {
                    this.Keys.Add(ks[i].Trim());
                }
            }
        }

        /// <summary>
        /// 获取标准字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            StringBuilder sb = new StringBuilder();
            // 生成键
            for (int i = 0; i < this.Keys.Count; i++) {
                if (i > 0) sb.Append(", ");
                sb.AppendFormat(this.Keys[i]);
            }
            sb.Append(" {\r\n");
            // 生成配置内容
            for (int i = 0; i < this.Items.Count; i++) {
                sb.AppendFormat("    {0}\r\n", this.Items[i].ToString());
            }
            sb.Append("}\r\n");
            return sb.ToString();
        }

    }
}
