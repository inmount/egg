using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg.Jttp;
using Microsoft.AspNetCore.Http;

namespace eggs {
    /// <summary>
    /// Mvc快速工具类
    /// </summary>
    public static class Mvc {

        /// <summary>
        /// 创建默认的配置文件
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDefaultKestrelConfigFile(string path) {
            using (var f = eggs.IO.OpenConfigDocument(path)) {
                var doc = f.Document;
                // 设定服务组
                var server = doc["server"];
                server["Enable"] = "yes";
                server["Configs"] = "http;https";
                // HTTP组
                var http = doc["http"];
                http["Enable"] = "yes";
                http["IP"] = "*";
                http["Port"] = "80";
                http["Certs"] = "";
                // HTTPS组
                var https = doc["https"];
                https["Enable"] = "no";
                https["IP"] = "*";
                https["Port"] = "443";
                https["Certs"] = "cert";
                // cert组
                var cert = doc["cert"];
                cert["Enable"] = "no";
                cert["Path"] = "";
                cert["Password"] = "";
                f.Save();
            }
        }

        /// <summary>
        /// 获取提交数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestData(HttpRequest request) {
            byte[] buffer = new byte[4096];
            List<byte> ls = new List<byte>();
            int res = 0;
            do {
                res = request.Body.Read(buffer, 0, buffer.Length);
                if (res > 0) {
                    ls.AddRange(new ArraySegment<byte>(buffer, 0, res));
                }
            } while (res > 0);
            return System.Text.Encoding.UTF8.GetString(ls.ToArray());
        }
        /// <summary>
        /// 获取提交Jttp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static JttpRequest GetRequestJttp(HttpRequest request) {
            return new JttpRequest(GetRequestData(request));
        }
    }
}
