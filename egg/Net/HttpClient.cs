using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace egg.Net {

    /// <summary>
    /// 网络客户端
    /// </summary>
    public class HttpClient {

        /// <summary>
        /// 下载进度函数
        /// </summary>
        /// <param name="size"></param>
        /// <param name="loaded"></param>
        public delegate void DownloadingDelegate(long size, long loaded);

        /// <summary>
        /// 以Get方式获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url) {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);
            return webRequest.GetStringAsync(url).GetAwaiter().GetResult();

        }

        /// <summary>
        /// 以Get方式获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string Get(string url, egg.KeyList<string> headers) {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);
            // 添加头信息
            foreach (var h in headers) {
                webRequest.DefaultRequestHeaders.Add(h.Key, h.Value);
            }
            return webRequest.GetStringAsync(url).GetAwaiter().GetResult();

        }

        /// <summary>
        /// 以Post方式获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Post(string url, string args, string contentType = "application/x-www-form-urlencoded") {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);

            // 建立传输内容
            HttpContent content = new StringContent(args);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            HttpResponseMessage response = webRequest.PostAsync(url, content).Result;

            // 判断状态并抛出异常
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }

        /// <summary>
        /// 以Post方式获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string Post(string url, string args, egg.KeyList<string> headers, string contentType = "application/x-www-form-urlencoded") {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);

            // 建立传输内容
            HttpContent content = new StringContent(args);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            // 添加头信息
            foreach (var h in headers) {
                content.Headers.Add(h.Key, h.Value);
            }
            HttpResponseMessage response = webRequest.PostAsync(url, content).Result;

            // 判断状态并抛出异常
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }

        /// <summary>
        /// 以Post方式获取数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string UploadFile(string path, string url, egg.KeyList<string> headers) {
            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);
            // 建立传输内容
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(path)), "file", System.IO.Path.GetFileName(path));
            // 添加头信息
            foreach (var h in headers) {
                content.Headers.Add(h.Key, h.Value);
            }

            HttpResponseMessage response = webRequest.PostAsync(url, content).Result;

            // 判断状态并抛出异常
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <param name="downloading"></param>
        public static void Download(string url, string path, DownloadingDelegate downloading = null) {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);
            HttpResponseMessage response = webRequest.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;

            // 创建文件操作流
            using (FileStream fs = System.IO.File.Open(path, FileMode.Create)) {

                var task = response.Content.CopyToAsync(fs);
                var contentLength = response.Content.Headers.ContentLength.GetValueOrDefault();
                bool isDone = false;

                // 判断是否需要进行回调
                if (downloading != null) {
                    new Task(() => {
                        while (!task.IsCompleted) {
                            downloading(contentLength, fs.Length);
                            if (fs.Length >= contentLength && contentLength > 0) isDone = true;
                            System.Threading.Thread.Sleep(500);
                        }
                    }).Start();
                }

                task.Wait();

                // 判断是否需要再回调一次进度更新
                if (downloading != null && contentLength > 0 && !isDone) downloading(contentLength, fs.Length);
            }

        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <param name="downloading"></param>
        public static void Download(string url, string path, egg.KeyList<string> headers, DownloadingDelegate downloading = null) {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);
            // 添加头信息
            foreach (var h in headers) {
                webRequest.DefaultRequestHeaders.Add(h.Key, h.Value);
            }
            HttpResponseMessage response = webRequest.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;

            // 创建文件操作流
            using (FileStream fs = System.IO.File.Open(path, FileMode.Create)) {

                var task = response.Content.CopyToAsync(fs);
                var contentLength = response.Content.Headers.ContentLength.GetValueOrDefault();
                bool isDone = false;

                // 判断是否需要进行回调
                if (downloading != null) {
                    new Task(() => {
                        while (!task.IsCompleted) {
                            downloading(contentLength, fs.Length);
                            if (fs.Length >= contentLength && contentLength > 0) isDone = true;
                            System.Threading.Thread.Sleep(500);
                        }
                    }).Start();
                }

                task.Wait();

                // 判断是否需要再回调一次进度更新
                if (downloading != null && contentLength > 0 && !isDone) downloading(contentLength, fs.Length);
            }

        }

    }
}
