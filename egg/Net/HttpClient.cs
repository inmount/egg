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
    /// HTTP协议网络客户端
    /// </summary>
    public class HttpClient : BasicObject {

        /// <summary>
        /// 支持的方法
        /// </summary>
        public enum Methods {
            /// <summary>
            /// GET方法
            /// </summary>
            GET = 0x01,
            /// <summary>
            /// POST方法
            /// </summary>
            POST = 0x02,
            /// <summary>
            /// HEAD方法
            /// </summary>
            HEAD = 0x03,
            /// <summary>
            /// OPTIONS方法
            /// </summary>
            OPTIONS = 0x11,
            /// <summary>
            /// PUT方法
            /// </summary>
            PUT = 0x12,
            /// <summary>
            /// PATCH方法
            /// </summary>
            PATCH = 0x13,
            /// <summary>
            /// DELETE方法
            /// </summary>
            DELETE = 0x14,
            /// <summary>
            /// TRACE方法
            /// </summary>
            TRACE = 0x15,
            /// <summary>
            /// CONNECT方法
            /// </summary>
            CONNECT = 0x16
        }

        /// <summary>
        /// 下载进度函数
        /// </summary>
        /// <param name="size"></param>
        /// <param name="loaded"></param>
        public delegate void DownloadingDelegate(long size, long loaded);

        /// <summary>
        /// 获取或设置请求方式
        /// </summary>
        public Methods Method { get; set; }

        /// <summary>
        /// 获取或设置请求数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 获取相关地址信息
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// 获取头部信息定义
        /// </summary>
        public HttpModules.HttpHeaders Headers { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="url"></param>
        public HttpClient(string url = "") {
            // 初始化属性
            this.Method = Methods.GET;
            this.Uri = new Uri(url);
            this.Headers = new HttpModules.HttpHeaders();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        public HttpClient(string url, HttpModules.HttpHeaders headers) {
            // 初始化属性
            this.Method = Methods.GET;
            this.Uri = new Uri(url);
            this.Headers = headers;
        }

        /// <summary>
        /// 设置头部定义
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public HttpClient SetHeader(string key, string value) {
            this.Headers.Set(key, value);
            return this;
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <returns></returns>
        public string GetContent() {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                //Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);

            // 应答器
            HttpResponseMessage response = null;

            switch (this.Method) {
                case Methods.GET:
                    // 添加头信息
                    foreach (var h in this.Headers) {
                        if (webRequest.DefaultRequestHeaders.Contains(h.Key)) {
                            webRequest.DefaultRequestHeaders.Remove(h.Key);
                        }
                        webRequest.DefaultRequestHeaders.Add(h.Key, h.Value);
                    }
                    // 提交申请并返回应答器
                    response = webRequest.GetAsync(this.Uri.ToString(), HttpCompletionOption.ResponseHeadersRead).Result;
                    break;
                case Methods.POST:
                    if (this.Headers.ContentType.IsEmpty()) this.Headers.ContentType = HttpModules.HttpHeaders.x_www_form_urlencoded;
                    // 建立传输内容
                    HttpContent content = new StringContent(this.Data);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(this.Headers.ContentType);
                    // 添加头信息
                    foreach (var h in this.Headers) {
                        if (h.Key != HttpModules.HttpHeaders.content_type) {
                            if (content.Headers.Contains(h.Key)) {
                                content.Headers.Remove(h.Key);
                            }
                            content.Headers.Add(h.Key, h.Value);
                        }
                    }
                    // 提交申请并返回应答器
                    response = webRequest.PostAsync(this.Uri.ToString(), content).Result;
                    break;
                default:
                    throw new Exception("不支持的请求方式");
            }
            // 判断状态并抛出异常
            response.EnsureSuccessStatusCode();
            // 返回数据结果
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 上传文件并获取数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string UploadFile(string path) {
            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                //Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);
            // 建立传输内容
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(path)), "file", System.IO.Path.GetFileName(path));
            // 添加头信息
            foreach (var h in this.Headers) {
                if (content.Headers.Contains(h.Key)) {
                    content.Headers.Remove(h.Key);
                }
                content.Headers.Add(h.Key, h.Value);
            }
            // 提交申请并返回应答器
            HttpResponseMessage response = webRequest.PostAsync(this.Uri.ToString(), content).Result;
            // 判断状态并抛出异常
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;

        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="downloading"></param>
        public void Download(string path, DownloadingDelegate downloading = null) {

            // 新建一个Handler
            var handler = new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.None,
                AllowAutoRedirect = true,
                UseProxy = false,
                //Proxy = null,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            // 新建一个HttpClient
            var webRequest = new System.Net.Http.HttpClient(handler);

            // 应答器
            HttpResponseMessage response = null;

            switch (this.Method) {
                case Methods.GET:
                    // 添加头信息
                    foreach (var h in this.Headers) {
                        if (webRequest.DefaultRequestHeaders.Contains(h.Key)) {
                            webRequest.DefaultRequestHeaders.Remove(h.Key);
                        }
                        webRequest.DefaultRequestHeaders.Add(h.Key, h.Value);
                    }
                    // 提交申请并返回应答器
                    response = webRequest.GetAsync(this.Uri.ToString(), HttpCompletionOption.ResponseHeadersRead).Result;
                    break;
                case Methods.POST:
                    if (this.Headers.ContentType.IsEmpty()) this.Headers.ContentType = HttpModules.HttpHeaders.x_www_form_urlencoded;
                    // 建立传输内容
                    HttpContent content = new StringContent(this.Data);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(this.Headers.ContentType);
                    // 添加头信息
                    foreach (var h in this.Headers) {
                        if (h.Key != HttpModules.HttpHeaders.content_type) {
                            if (content.Headers.Contains(h.Key)) {
                                content.Headers.Remove(h.Key);
                            }
                            content.Headers.Add(h.Key, h.Value);
                        }
                    }
                    // 提交申请并返回应答器
                    response = webRequest.PostAsync(this.Uri.ToString(), content).Result;
                    break;
                default:
                    throw new Exception("不支持的请求方式");
            }

            // 判断状态并抛出异常
            response.EnsureSuccessStatusCode();

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
                            task.Wait();
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
