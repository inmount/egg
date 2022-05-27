using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net.HttpModules {

    /// <summary>
    /// Http协议头部
    /// </summary>
    public class HttpHeader : egg.KeyList<string> {

        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        public const string x_www_form_urlencoded = "application/x-www-form-urlencoded";

        /// <summary>
        /// ContentType
        /// </summary>
        public string ContentType { get { return this["content-type"]; } set { this["content-type"] = value; } }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpHeader Set(string key, string value) {
            this[key] = value;
            return this;
        }

        /// <summary>
        /// 创建一个实例化对象
        /// </summary>
        /// <returns></returns>
        public static HttpHeader Create() {
            return new HttpHeader();
        }

        /// <summary>
        /// 获取标准的字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {

            string res = "";

            foreach (var item in this) {
                //socket.Send(Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n"));
                res += $"{item.Key}:{item.Value}\r\n";
            }

            return res;
        }

    }
}
