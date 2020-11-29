using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net.HttpModules {

    /// <summary>
    /// Http协议头部
    /// </summary>
    public class HttpHeader : egg.KeyList<string> {

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
