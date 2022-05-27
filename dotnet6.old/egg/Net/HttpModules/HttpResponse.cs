using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Net.HttpModules {

    /// <summary>
    /// Http接收管理器
    /// </summary>
    public class HttpResponse : egg.Object {

        /// <summary>
        /// 获取协议版本
        /// </summary>
        public string Protocol { get; private set; }

        /// <summary>
        /// 获取状态码
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// 头部信息
        /// </summary>
        public HttpHeader Headers { get; private set; }

        /// <summary>
        /// 头部信息
        /// </summary>
        public byte[] ContentBytes { get; private set; }

        /// <summary>
        /// 获取内容
        /// </summary>
        public string GetContent(Encoding encoding) {
            return encoding.GetString(this.ContentBytes);
        }

        /// <summary>
        /// 获取UTF8内容
        /// </summary>
        public string GetUtf8Content() {
            return GetContent(Encoding.UTF8);
        }

        /// <summary>
        /// 内容长度
        /// </summary>
        public int ContentLength { get; internal set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public HttpResponse(string protocol, int status, HttpHeader headers, byte[] body) {
            this.Protocol = protocol;
            this.Status = status;
            this.Headers = headers;
            //this.HeaderString = Encoding.ASCII.GetString(head);
            this.ContentBytes = body;

            string cLen = Headers["Content-Length"];
            if (cLen.IsInteger()) {
                this.ContentLength = cLen.ToInteger();

                //根据长度修正数据
                if (body.Length != this.ContentLength) {
                    var bs = new byte[this.ContentLength];
                    if (body.Length < this.ContentLength) {
                        Array.Copy(body, 0, bs, 0, body.Length);
                    } else {
                        Array.Copy(body, 0, bs, 0, this.ContentLength);
                    }
                    this.ContentBytes = bs;
                }
            } else {
                this.ContentLength = body.Length;
            }

        }

    }
}
