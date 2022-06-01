using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Jttp {

    /// <summary>
    /// Jttp访问客户端
    /// </summary>
    public class JttpClient : egg.BasicObject {

        /// <summary>
        /// 获取提交器
        /// </summary>
        public JttpRequest Request { get; private set; }

        /// <summary>
        /// 获取应答器
        /// </summary>
        public JttpResponse Response { get; private set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JttpClient() {
            this.Request = new JttpRequest();
        }

        /// <summary>
        /// 提交内容到指定地址并获取返回
        /// </summary>
        /// <param name="url"></param>
        public void Post(string url) {
            Response = Post(url, Request);
        }

        /// <summary>
        /// 提交内容到指定地址并获取返回应答器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static JttpResponse Post(string url, string args) {
            string json = eggs.Net.GetHttpContent(url, args);
            return new JttpResponse(json);
        }

        /// <summary>
        /// 提交内容到指定地址并获取返回应答器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static JttpResponse Post(string url, JttpRequest request = null) {
            string reqJson = "";
            if (!request.IsNull()) reqJson = request.ToJsonString();
            return Post(url, reqJson);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            Request.Destroy();
            Response.Destroy();
            base.OnDispose();
        }

    }
}
