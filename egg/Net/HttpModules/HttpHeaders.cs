using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net.HttpModules {

    /// <summary>
    /// Http协议头部
    /// </summary>
    public class HttpHeaders : egg.KeyStrings {

        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        public const string x_www_form_urlencoded = "application/x-www-form-urlencoded";

        /// <summary>
        /// ContentType
        /// </summary>
        public string ContentType { get { return this["content-type"]; } set { this["content-type"] = value; } }

        /// <summary>
        /// 创建一个实例化对象
        /// </summary>
        /// <returns></returns>
        public new static HttpHeaders Create() {
            return new HttpHeaders();
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
