using System;
using egg;

/// <summary>
/// Egg 开发套件 快速操作入口
/// </summary>
public partial class eggs {

    /// <summary>
    /// 网络相关函数
    /// </summary>
    public static class Net {

        /// <summary>
        /// 将域名解析为对应的IP地址
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string GetIPv4Address(string host) {
            if (host.IsIPv4()) return host;

            System.Net.IPAddress[] iPs = System.Net.Dns.GetHostEntry(host).AddressList;
            for (int i = 0; i < iPs.Length; i++) {
                string ip = iPs[i].ToString();
                if (ip.IsIPv4())
                    return ip;
            }

            throw new Exception("解析失败");

        }

        /// <summary>
        /// 根据URL获取超文本内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpContent(string url) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url)) {
                return http.GetContent();
            }
        }

        /// <summary>
        /// 根据URL获取超文本内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpContent(string url, egg.Net.HttpModules.HttpHeaders headers) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url, headers)) {
                return http.GetContent();
            }
        }

        /// <summary>
        /// 根据URL提交数据并获取超文本内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpContent(string url, string postData) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url)) {
                http.Method = egg.Net.HttpClient.Methods.POST;
                http.Data = postData;
                return http.GetContent();
            }
        }

        /// <summary>
        /// 根据URL提交数据并获取超文本内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpContent(string url, string postData, egg.Net.HttpModules.HttpHeaders headers) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url, headers)) {
                http.Method = egg.Net.HttpClient.Methods.POST;
                http.Data = postData;
                return http.GetContent();
            }
        }

        /// <summary>
        /// 上传文件并获取超文本内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UploadFile(string path, string url) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url)) {
                return http.UploadFile(path);
            }
        }

        /// <summary>
        /// 上传文件并获取超文本内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UploadFile(string path, string url, egg.Net.HttpModules.HttpHeaders headers) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url, headers)) {
                return http.UploadFile(path);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void DownloadFile(string path, string url) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url)) {
                http.Download(path);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void DownloadFile(string path, string url, egg.Net.HttpModules.HttpHeaders headers) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url, headers)) {
                http.Download(path);
            }
        }

        /// <summary>
        /// 提交数据并下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void DownloadFile(string path, string url, string postData) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url)) {
                http.Method = egg.Net.HttpClient.Methods.POST;
                http.Data = postData;
                http.Download(path);
            }
        }

        /// <summary>
        /// 提交数据并下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void DownloadFile(string path, string url, string postData, egg.Net.HttpModules.HttpHeaders headers) {
            using (egg.Net.HttpClient http = new egg.Net.HttpClient(url, headers)) {
                http.Method = egg.Net.HttpClient.Methods.POST;
                http.Data = postData;
                http.Download(path);
            }
        }

        /// <summary>
        /// 根据URL获取Json对象
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static egg.Serializable.Json.Node GetHttpJson(string url) {
            string json = GetHttpContent(url);
            return eggs.Json.Parse(json);
        }

        ///// <summary>
        ///// 根据URL获取HTML对象
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public static egg.Html.HtmlDocument GetHttpHtmlDocument(string url) {
        //    string html = egg.Net.HttpClient.Get(url);
        //    return egg.Html.Parser.GetDocument(html);
        //}

    }

}
