using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net.HttpModules {

    /// <summary>
    /// Http提交管理器
    /// </summary>
    public class HttpRequest : egg.Object {

        /// <summary>
        /// 获取地址信息对象
        /// </summary>
        public egg.Net.Uri Uri { get; private set; }

        /// <summary>
        /// 头部信息
        /// </summary>
        public HttpHeader Header { get; private set; }

        /// <summary>
        /// 打印信息到字符串
        /// </summary>
        public string PrintToString() {
            string res = "";

            string path = (Uri.Path == "" ? "/" : Uri.Path) + Uri.QueryString;

            //发送HTTP头部信息
            //socket.Send(Encoding.ASCII.GetBytes($"GET {path} HTTP/1.1\r\n"));
            res += $"GET {path} HTTP/1.1\r\n";

            foreach (var item in Header) {
                //socket.Send(Encoding.ASCII.GetBytes($"{item.Key}:{item.Value}\r\n"));
                res += $"{item.Key}:{item.Value}\r\n";
            }
            //socket.Send(Encoding.ASCII.GetBytes($"\r\n"));

            return res;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public HttpRequest(string url) {
            this.Uri = new egg.Net.Uri(url);
            this.Header = new HttpHeader();
            Header["Host"] = Uri.Host + (Uri.Port != "" ? ":" : "") + Uri.Port;
            Header["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36 dpz/2.0";
            //Header["Referer"] = "http://www.dpzsoft.com/";
            Header["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            Header["Accept-Encoding"] = "gzip, deflate, br";
            Header["Accept-Language"] = "zh-CN,zh;q=0.9,zh-TW;q=0.8,en-US;q=0.7,en;q=0.6";
            Header["Upgrade-Insecure-Requests"] = "1";
        }

    }
}
