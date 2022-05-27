using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Console {

    /// <summary>
    /// E格式参数集合
    /// </summary>
    public class EParams : egg.KeyStrings {

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="args"></param>
        /// <param name="tags"></param>
        public EParams(string[] args = null) {
            // 获取参数
            for (int i = 0; i < args.Length; i++) {
                string str = args[i];
                // 双横线开头为定义开始
                if (str.StartsWith("--")) {
                    // 冒号为内容定义
                    int idxSign = str.IndexOf(':');
                    if (idxSign > 0) {
                        string sign = str.Substring(2, idxSign - 2);
                        string value = str.Substring(idxSign + 1);
                        if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length > 1) value = value.Substring(1, value.Length - 2);
                        this[sign] = value;
                    } else {
                        string sign = str.Substring(2);
                        this[sign] = "";
                    }
                }
            }
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this) {
                if (sb.Length > 0) sb.Append(' ');
                if (item.Value.IsEmpty()) {
                    sb.Append("--");
                    sb.Append(item.Key);
                } else {
                    sb.Append("--");
                    sb.Append(item.Key);
                    sb.Append(":\"");
                    sb.Append(item.Value);
                    sb.Append("\"");
                }
            }
            return sb.ToString();
        }

    }
}
