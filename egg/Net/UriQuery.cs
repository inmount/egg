using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net {

    /// <summary>
    /// 统一资源定位符参数查询器
    /// </summary>
    public class UriQuery : egg.BasicObject {

        private KeyList<string> _args;//参数链表

        /// <summary>
        /// 新建对象实例
        /// </summary>
        /// <param name="qs"></param>
        public UriQuery(string qs = "") {

            //初始化参数列表
            _args = new KeyList<string>();
            if (qs.IsEmpty()) return;

            //分析参数列表
            string[] args = qs.Split('&');

            for (int i = 0; i < args.Length; i++) {
                int nDIdx = args[i].IndexOf("=");
                string szName = args[i].Substring(0, nDIdx);
                string szValue = args[i].Substring(nDIdx + 1);
                //gArgs.Add(szName, System.Web.HttpUtility.UrlDecode(szValue));
                this[szName] = System.Web.HttpUtility.UrlDecode(szValue);
            }

        }

        /// <summary>
        /// 获取或设置参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name] {
            get {
                return _args[name];
            }
            set {
                _args[name] = value;
            }
        }

        /// <summary>
        /// 获取字符串显示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            StringBuilder sb = new StringBuilder();
            foreach (var arg in _args) {
                if (arg.Value != "") {
                    if (sb.Length > 0) sb.Append("&");
                    sb.AppendFormat("{0}={1}", arg.Key, System.Web.HttpUtility.UrlEncode(arg.Value));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取Json形式表示的参数
        /// </summary>
        public string ToJson() {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var arg in _args) {
                if (arg.Value != "") {
                    if (sb.Length > 0) sb.Append(",");
                    sb.AppendFormat("\"{0}\":\"{1}\"", arg.Key, arg.Value.Replace("\"", "\\\""));
                }
            }
            sb.Append("}");
            return sb.ToString();
        }

    }
}
