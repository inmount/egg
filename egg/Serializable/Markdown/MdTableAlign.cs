using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Serializable.Markdown {

    /// <summary>
    /// 表格头部数据
    /// </summary>
    public class MdTableAlign : MdTextLine {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public MdTableAlign() : base(MdTypes.TableAlign) { }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("[MdTableAlign {0}]", this.Children.Count);
        }

        /// <summary>
        /// 获取标准字符串表示
        /// </summary>
        /// <returns></returns>
        protected override string OnGetMarkdownString() {
            StringBuilder sb = new StringBuilder();
            foreach (var md in this.Children) {
                sb.Append("| ");
                switch (md.ToString()) {
                    case "left": sb.Append(":-----"); break;
                    case "center": sb.Append(":----:"); break;
                    case "right": sb.Append("-----:"); break;
                    default: sb.Append("------"); break;
                }
                sb.Append(" ");
            }
            sb.Append("|");
            return sb.ToString();

        }

        /// <summary>
        /// 获取标准的HTML字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetHtmlString() {
            return "";
        }

    }
}
