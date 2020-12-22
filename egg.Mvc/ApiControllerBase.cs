using egg;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace egg.Mvc {

    /// <summary>
    /// WebApi基础类
    /// </summary>
    public abstract class ApiControllerBase : Controller {

        // 私有全局变量
        private bool _is_request_json;
        private bool _is_request_form;

        /// <summary>
        /// 输出对象
        /// </summary>
        protected egg.Jttp.Object JResponse { get; private set; }

        /// <summary>
        /// 获取格式化内容后的对象
        /// </summary>
        protected egg.Json.JsonObject JRequest { get; private set; }

        /// <summary>
        /// 获取表单内容对象
        /// </summary>
        protected egg.KeyValues<string> Form { get; private set; }

        /// <summary>
        /// 获取文本内容
        /// </summary>
        protected string JRequestText { get; private set; }

        /// <summary>
        /// 设置为表单数据模式
        /// </summary>
        protected void SetRequestFormMode() {
            _is_request_form = true;
            _is_request_json = false;
        }

        /// <summary>
        /// 设置为Json数据模式
        /// </summary>
        protected void SetRequestJsonMode() {
            _is_request_form = false;
            _is_request_json = true;
        }

        /// <summary>
        /// 清楚数据模式
        /// </summary>
        protected void ClearRequestMode() {
            _is_request_form = false;
            _is_request_json = false;
        }

        /// <summary>
        /// 可重载的初始化时间
        /// </summary>
        /// <returns></returns>
        protected virtual string OnInit() { return null; }

        /// <summary>
        /// 可重载的呈现时间
        /// </summary>
        /// <returns></returns>
        protected virtual void OnRender() { }

        /// <summary>
        /// 操作初始化
        /// </summary>
        /// <returns></returns>
        protected string Initialize() {

            // Json数据模式
            if (_is_request_json) {
                byte[] buffer = new byte[4096];
                List<byte> ls = new List<byte>();
                int res = 0;
                do {
                    res = base.Request.Body.Read(buffer, 0, buffer.Length);
                    if (res > 0) {
                        ls.AddRange(new ArraySegment<byte>(buffer, 0, res));
                    }
                } while (res > 0);
                //var reader = new System.IO.StreamReader(Request.Body);
                string content = System.Text.Encoding.UTF8.GetString(ls.ToArray());
                this.JRequestText = content;

                // 解析获取到的数据
                this.JRequest = (egg.Json.JsonObject)egg.Json.Parser.ParseJson(content);
            }

            // 表单数据模式
            if (_is_request_json) {
                this.Form = new KeyValues<string>();
                foreach (string key in Request.Form.Keys) {
                    this.Form[key] = Request.Form[key];
                }
            }

            // 建立Jttp应答器
            this.JResponse = new Jttp.Object();

            // 返回初始化重载事件
            return this.OnInit();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public ApiControllerBase() {
            this.ClearRequestMode();
        }

        /// <summary>
        /// 将行数据填充到Json对象中
        /// </summary>
        /// <param name="row"></param>
        /// <param name="obj"></param>
        protected void RenderData(egg.db.Row row, egg.Json.JsonObject obj = null) {
            if (eggs.IsNull(obj)) obj = JResponse.Data;
            foreach (var item in row) {
                obj.String(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 将行集合填充到Json对象中
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="array"></param>
        protected void RenderList(egg.db.Rows rows, egg.Json.JsonArray array = null) {
            if (eggs.IsNull(array)) array = JResponse.List;
            foreach (var row in rows) {
                RenderData(row, array.Object(array.Count));
            }
        }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <returns></returns>
        protected string Success(string msg = null) {
            JResponse.Result = 1;
            if (!msg.IsNoneOrNull()) JResponse.Message = msg;
            // 重载
            this.OnRender();
            return JResponse.ToString();
        }

        /// <summary>
        /// 返回失败数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected string Fail(string msg = null) {
            JResponse.Result = 0;
            if (!msg.IsNoneOrNull()) JResponse.Message = msg;
            // 重载
            this.OnRender();
            return JResponse.ToString();
        }

        /// <summary>
        /// 返回错误数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected string Error(int code = 0, string msg = null, string info = null) {
            JResponse.Result = -1;
            if (!msg.IsNoneOrNull()) JResponse.Message = msg;
            JResponse.Error.Code = 0;
            if (info != null) JResponse.Error.Info = info;
            // 重载
            this.OnRender();
            return JResponse.ToString();
        }

    }
}
