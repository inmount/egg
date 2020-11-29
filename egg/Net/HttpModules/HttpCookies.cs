using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net.HttpModules {

    /// <summary>
    /// 专用于超文本传输协议的会话本地存储信息
    /// </summary>
    public class HttpCookies : egg.KeyList<string> {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public HttpCookies() { }


        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="str"></param>
        public HttpCookies(string str) {

            if (str != "") {
                string[] szStrs = str.Split(';');
                for (int i = 0; i < szStrs.Length; i++) {
                    int nDeng = szStrs[i].IndexOf("=");
                    if (nDeng <= 0) {
                        //base[szStrs[i]].Value = "";
                    } else {
                        string szKey = szStrs[i].Substring(0, nDeng).Trim();
                        string szValue = szStrs[i].Substring(nDeng + 1).Trim();
                        if (szKey != "path")
                            base[szKey] = System.Web.HttpUtility.UrlDecode(szValue);
                    }

                }
            }

        }

        /// <summary>
        /// 获取标准的Cookie字符串
        /// </summary>
        /// <returns></returns>
        public new string ToString() {
            string res = "";
            foreach (var item in this) {
                if (res != "") res += ",";
                if (item.Value == "") {
                    res += item.Key;
                } else {
                    res += item.Key + "=" + System.Web.HttpUtility.UrlEncode(item.Key);
                }
            }
            return res;
        }

    }
}
