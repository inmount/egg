using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Mvc {
    /// <summary>
    /// API控制呈现器处理上下文
    /// </summary>
    public class ApiControllerRenderContext {

        // 表单
        private egg.KeyValues<string> _form;

        /// <summary>
        /// 获取相关控制器
        /// </summary>
        public ApiControllerBase Controller { get; private set; }

        /// <summary>
        /// 获取申请器
        /// </summary>
        public Jttp.JttpRequest Request { get; private set; }

        /// <summary>
        /// 获取应答器
        /// </summary>
        public Jttp.JttpResponse Response { get; private set; }

        /// <summary>
        /// 获取关键字集合
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetKeys() {
            return _form.Keys;
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="controller"></param>
        public ApiControllerRenderContext(ApiControllerBase controller, egg.KeyValues<string> form) {
            this.Controller = controller;
            this.Request = controller.JRequest;
            this.Response = controller.JResponse;
            _form = form;
        }

        /// <summary>
        /// 获取提交参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] { get { return _form[key]; } }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <returns></returns>
        public string Success(string msg = null) {
            this.Response.Result = 1;
            if (!msg.IsNoneOrNull()) this.Response.Message = msg;
            return this.Response.ToJsonString();
        }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <returns></returns>
        public string Success(db.Row row, string msg = null) {
            this.Response.Result = 1;
            if (!msg.IsNoneOrNull()) this.Response.Message = msg;
            this.Response.Data = row.ToJsonObject();
            return this.Response.ToJsonString();
        }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <returns></returns>
        public string Success(db.Rows rows, string msg = null) {
            this.Response.Result = 1;
            if (!msg.IsNoneOrNull()) this.Response.Message = msg;
            this.Response.Datas = rows.ToJsonArray();
            return this.Response.ToJsonString();
        }

        /// <summary>
        /// 返回失败数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string Fail(string msg = null) {
            this.Response.Result = 0;
            if (!msg.IsNoneOrNull()) this.Response.Message = msg;
            return this.Response.ToJsonString();
        }

        /// <summary>
        /// 返回错误数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public string Error(int code = 0, string msg = null, string info = null) {
            this.Response.Result = -1;
            if (!msg.IsNoneOrNull()) this.Response.Message = msg;
            if (!info.IsNoneOrNull()) this.Response.ErrorInfo = info;
            this.Response.ErrorCode = code;
            return this.Response.ToJsonString();
        }
    }
}
