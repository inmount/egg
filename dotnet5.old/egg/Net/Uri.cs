using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net {

    /// <summary>
    /// 统一资源定位符
    /// </summary>
    public class Uri : egg.Object {

        /// <summary>
        /// 协议信息
        /// </summary>
        public string Procotol { get; set; } = "";

        /// <summary>
        /// 主机信息
        /// </summary>
        public string Host { get; set; } = "";

        /// <summary>
        /// 端口信息
        /// </summary>
        public string Port { get; private set; } = "";

        /// <summary>
        /// 页面信息
        /// </summary>
        public string Path { get; private set; } = "";

        /// <summary>
        /// 页面信息
        /// </summary>
        public string FileName { get; private set; } = "";


        /// <summary>
        /// 获取参数查询器
        /// </summary>
        public UriQuery Query { get; private set; }

        /// <summary>
        /// 获取参数字符串
        /// </summary>
        public string QueryString { get { return this.Query.ToString(); } }

        /// <summary>
        /// 新建对象实例
        /// </summary>
        /// <param name="url"></param>
        public Uri(string url = "") {
            //gszUrl = url;

            //初始化参数列表
            if (url.IsNone()) {
                this.Query = new UriQuery();
                return;
            }

            //清除两边多于的空格
            url = url.Trim();

            int nQIdx = url.IndexOf("?");

            //获取Url的地址部分
            string pathAll = "";
            string argStr = "";

            if (nQIdx >= 0) {

                //获取Url的地址部分
                pathAll = url.Substring(0, nQIdx);
                //获取Url的参数部分
                argStr = url.Substring(nQIdx + 1);

            } else {
                pathAll = url;
            }

            //分析地址
            string[] pathParts = pathAll.Split('/');
            if (pathParts.Length < 3) throw new Exception("无效地址");
            if (!pathParts[1].IsNone()) throw new Exception("无效协议定义");
            this.Procotol = pathParts[0];

            //分析主机名及端口
            string host = pathParts[2];
            int portIdx = host.IndexOf(":");
            if (portIdx <= 0) {
                this.Host = host;
            } else {
                this.Host = host.Substring(0, portIdx);
                string port = host.Substring(portIdx + 1);
                if (!port.IsInteger()) throw new Exception("无效的端口信息");
                this.Port = port;
            }

            //重新阻止页面地址
            string pathPage = "";
            for (var i = 3; i < pathParts.Length; i++) {
                pathPage += "/" + pathParts[i];
            }
            this.Path = pathPage;
            if (pathParts.Length > 3) {
                this.FileName = pathParts[pathParts.Length - 1];
            }

            //分析参数列表
            this.Query = new UriQuery(argStr);

        }

        /// <summary>
        /// 获取或设置参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name] {
            get {
                return this.Query[name];
            }
            set {
                this.Query[name] = value;
            }
        }

        /// <summary>
        /// 获取字符串显示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            string port = this.Port == "" ? "" : ":" + this.Port;
            string qs = this.Query.ToString();
            if (qs.IsNoneOrNull()) {
                return $"{this.Procotol}//{this.Host}{port}{this.Path}";
            } else {
                return $"{this.Procotol}//{this.Host}{port}{this.Path}?{qs}";
            }
        }


    }
}
