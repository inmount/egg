using egg;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace egg.Mvc {

    /// <summary>
    /// WebApi基础类
    /// </summary>
    public abstract class ApiControllerBase : Controller {

        /// <summary>
        /// 默认数据处理模式
        /// </summary>
        public static ApiControllerMode DefaultMode = ApiControllerMode.Native;

        // 私有全局变量
        private bool _is_request_json;
        private bool _is_request_form;

        /// <summary>
        /// 输出对象
        /// </summary>
        public egg.Jttp.JttpResponse JResponse { get; private set; }

        /// <summary>
        /// 获取格式化内容后的对象
        /// </summary>
        public egg.Jttp.JttpRequest JRequest { get; private set; }

        /// <summary>
        /// 获取表单内容对象
        /// </summary>
        protected egg.KeyValues<string> Form { get; private set; }

        #region [=====集中处理字段集合=====]

        private List<ApiControllerFieldSetting> _fields;

        /// <summary>
        ///  清理字段设定
        /// </summary>
        protected void FieldsClear() { _fields.Clear(); }

        /// <summary>
        /// 添加字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void AddField(string name) { _fields.Add(new ApiControllerFieldSetting() { Name = name }); }

        /// <summary>
        /// 添加字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void AddField(string name, bool isMust, bool isEnable = true) { _fields.Add(new ApiControllerFieldSetting() { Name = name, IsMust = isMust, Enabled = isEnable }); }

        /// <summary>
        /// 添加字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void AddField(string name, ApiControllerFieldSetting.DataTypes types, bool isMust = false, bool isEnable = true) { _fields.Add(new ApiControllerFieldSetting() { Name = name, DataType = types, IsMust = isMust, Enabled = isEnable }); }

        // 查找字段设定
        private ApiControllerFieldSetting FindFieldSetting(string name) {
            for (int i = 0; i < _fields.Count; i++) {
                if (_fields[i].Name == name) return _fields[i];
            }
            return null;
        }

        // 获取字段设定
        private ApiControllerFieldSetting GetFieldSetting(string name) {
            ApiControllerFieldSetting field = FindFieldSetting(name);
            if (eggs.IsNull(field)) field = FindFieldSetting("*");
            return null;
        }

        /// <summary>
        /// 设置字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void SetField(string name) {
            ApiControllerFieldSetting field = FindFieldSetting(name);
            if (eggs.IsNull(field)) AddField(name);
        }

        /// <summary>
        /// 设置字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void SetField(string name, bool isMust, bool isEnable = true) {
            ApiControllerFieldSetting field = FindFieldSetting(name);
            if (eggs.IsNull(field)) {
                AddField(name, isMust, isEnable);
            } else {
                field.IsMust = isMust;
                field.Enabled = isEnable;
            }
        }

        /// <summary>
        /// 设置字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void SetField(string name, ApiControllerFieldSetting.DataTypes types, bool isMust = false, bool isEnable = true) {
            ApiControllerFieldSetting field = FindFieldSetting(name);
            if (eggs.IsNull(field)) {
                AddField(name, types, isMust, isEnable);
            } else {
                field.DataType = types;
                field.IsMust = isEnable;
                field.Enabled = isEnable;
            }
        }

        #endregion

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

            // 建立Jttp应答器
            this.JResponse = new Jttp.JttpResponse();

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
                this.JRequest = (egg.Jttp.JttpRequest)eggs.ParseJson(content, typeof(egg.Jttp.JttpRequest));

                // 检测所有的必要参数
                foreach (var field in _fields) {
                    if (field.IsMust) {
                        if (!JRequest.Form.ContainsKey(field.Name)) {
                            JResponse.SetFail($"缺少必要的 '{field.Name}' 参数");
                            return JResponse.ToJson();
                        }
                    }
                }

                // 填充表单数据
                this.Form = new KeyValues<string>();
                foreach (string key in JRequest.Form.GetNames()) {
                    var field = GetFieldSetting(key);
                    if (!eggs.IsNull(field)) {
                        if (field.Enabled) {
                            switch (field.DataType) {
                                case ApiControllerFieldSetting.DataTypes.Decimal: this.Form[key] = JRequest.Form[key].GetNumber().ToString(); break;
                                case ApiControllerFieldSetting.DataTypes.Long: this.Form[key] = ((long)JRequest.Form[key].GetNumber()).ToString(); break;
                                case ApiControllerFieldSetting.DataTypes.Integer: this.Form[key] = ((int)JRequest.Form[key].GetNumber()).ToString(); break;
                                default: this.Form[key] = "" + JRequest.Form[key].GetString(); break;
                            }
                        }
                    }
                }
            }

            // 表单数据模式
            if (_is_request_form) {
                // 检测所有的必要参数
                foreach (var field in _fields) {
                    if (field.IsMust) {
                        if (!Request.Form.ContainsKey(field.Name)) {
                            JResponse.SetFail($"缺少必要的 '{field.Name}' 参数");
                            return JResponse.ToJson();
                        }
                    }
                }
                // 填充表单数据
                this.Form = new KeyValues<string>();
                foreach (string key in Request.Form.Keys) {
                    var field = GetFieldSetting(key);
                    if (!eggs.IsNull(field)) {
                        if (field.Enabled) {
                            switch (field.DataType) {
                                case ApiControllerFieldSetting.DataTypes.Decimal: this.Form[key] = ((string)Request.Form[key]).ToDouble().ToString(); break;
                                case ApiControllerFieldSetting.DataTypes.Long: this.Form[key] = ((string)Request.Form[key]).ToLong().ToString(); break;
                                case ApiControllerFieldSetting.DataTypes.Integer: this.Form[key] = ((string)Request.Form[key]).ToInteger().ToString(); break;
                                default: this.Form[key] = Request.Form[key]; break;
                            }
                        }
                    }
                }
            }

            // 返回初始化重载事件
            return this.OnInit();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public ApiControllerBase() {
            this.ClearRequestMode();
            if (DefaultMode == ApiControllerMode.Form) {
                this.SetRequestFormMode();
            }
            if (DefaultMode == ApiControllerMode.Jttp) {
                this.SetRequestJsonMode();
            }
            _fields = new List<ApiControllerFieldSetting>();
            SetField("*", ApiControllerFieldSetting.DataTypes.String, false, true);
            //this.Initialize();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public ApiControllerBase(ApiControllerMode mode) {
            this.ClearRequestMode();
            if (mode == ApiControllerMode.Form) {
                this.SetRequestFormMode();
            }
            if (mode == ApiControllerMode.Jttp) {
                this.SetRequestJsonMode();
            }
            //this.Initialize();
        }

        /// <summary>
        /// 返回函数运行结果
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected string Func(Action<Jttp.JttpRequest, Jttp.JttpResponse> action) {
            try {
                Initialize();
                action(this.JRequest, this.JResponse);
                return this.JResponse.ToJson();
            } catch (Exception ex) {
                return Error(0, ex.Message);
            }
        }

        /// <summary>
        /// 创建上下文
        /// </summary>
        /// <returns></returns>
        protected virtual ApiControllerRenderContext OnCreateRenderContext() { return new ApiControllerRenderContext(this, Form); }

        /// <summary>
        /// 呈现之前事件
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnBeforeRender(ApiControllerRenderContext context) { return true; }

        /// <summary>
        /// 呈现后事件
        /// </summary>
        /// <returns></returns>
        protected virtual void OnAfterRender(ApiControllerRenderContext context) { }

        /// <summary>
        /// 呈现器
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected string Render(Action<ApiControllerRenderContext> action) {
            try {
                Initialize();
                var context = this.OnCreateRenderContext();
                if (this.OnBeforeRender(context)) {
                    action(context);
                }
                this.OnAfterRender(context);
                return context.Response.ToJson();
            } catch (Exception ex) {
                return Error(0, ex.Message);
            }
        }

        #region [=====直接操作模式=====]

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <returns></returns>
        protected string Success(string msg = null) {
            JResponse.Result = 1;
            if (!msg.IsNoneOrNull()) JResponse.Message = msg;
            // 重载
            this.OnRender();
            return JResponse.ToJson();
        }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <returns></returns>
        protected string Success(db.Row row, string msg = null) {
            JResponse.Result = 1;
            if (!msg.IsNoneOrNull()) JResponse.Message = msg;
            JResponse.Data = row.ToJsonObject();
            // 重载
            this.OnRender();
            return JResponse.ToJson();
        }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <returns></returns>
        protected string Success(db.Rows rows, string msg = null) {
            JResponse.Result = 1;
            if (!msg.IsNoneOrNull()) JResponse.Message = msg;
            JResponse.Datas = rows.ToJsonArray();
            // 重载
            this.OnRender();
            return JResponse.ToJson();
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
            return JResponse.ToJson();
        }

        /// <summary>
        /// 返回错误数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        protected string Error(int code = 0, string msg = null) {
            JResponse.Result = -1;
            if (!msg.IsNoneOrNull()) JResponse.Message = msg;
            JResponse.ErrorCode = 0;
            // 重载
            this.OnRender();
            return JResponse.ToJson();
        }

        #endregion

    }
}
