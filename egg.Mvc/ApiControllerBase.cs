using egg;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using egg.JsonBean;
using Microsoft.AspNetCore.Hosting;

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

        /// <summary>
        /// 获取配置信息集合
        /// </summary>
        protected List<ApiControllerFieldSetting> Configs { get; private set; }

        /// <summary>
        ///  清理字段设定
        /// </summary>
        protected void ClearConfigs() { this.Configs.Clear(); }

        /// <summary>
        /// 添加字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void AddConfig(string name, ApiControllerFieldSetting.DataTypes types, bool isMust = false, bool isEnable = true) { this.Configs.Add(new ApiControllerFieldSetting() { Name = name, DataType = types, IsMust = isMust, Enabled = isEnable }); }

        /// <summary>
        /// 添加字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void AddConfig(string name) { AddConfig(name, ApiControllerFieldSetting.DataTypes.String, false, true); }

        /// <summary>
        /// 添加字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void AddConfig(string name, bool isMust, bool isEnable = true) { AddConfig(name, ApiControllerFieldSetting.DataTypes.String, isMust, isEnable); }

        // 查找字段设定
        private ApiControllerFieldSetting FindConfig(string name) {
            foreach (var cfg in this.Configs) {
                if (cfg.Name == name) return cfg;
            }
            return null;
        }

        // 获取字段设定
        private ApiControllerFieldSetting GetConfig(string name) {
            ApiControllerFieldSetting field = FindConfig(name);
            if (eggs.IsNull(field)) field = FindConfig("*");
            return field;
        }

        /// <summary>
        /// 设置字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void SetConfig(string name) {
            ApiControllerFieldSetting field = FindConfig(name);
            if (eggs.IsNull(field)) AddConfig(name);
        }

        /// <summary>
        /// 设置字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void SetConfig(string name, bool isMust, bool isEnable = true) {
            ApiControllerFieldSetting cfg = FindConfig(name);
            if (eggs.IsNull(cfg)) {
                AddConfig(name, isMust, isEnable);
            } else {
                cfg.IsMust = isMust;
                cfg.Enabled = isEnable;
            }
        }

        /// <summary>
        /// 设置字段设定
        /// </summary>
        /// <param name="name"></param>
        protected void SetConfig(string name, ApiControllerFieldSetting.DataTypes types, bool isMust = false, bool isEnable = true) {
            ApiControllerFieldSetting cfg = FindConfig(name);
            if (eggs.IsNull(cfg)) {
                AddConfig(name, types, isMust, isEnable);
            } else {
                cfg.DataType = types;
                cfg.IsMust = isEnable;
                cfg.Enabled = isEnable;
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
                // 获取提交的数据
                string content = Utilities.GetRequestData(base.Request);
                this.JRequestText = content;

                // 解析获取到的数据
                this.JRequest = new egg.Jttp.JttpRequest(content);

                // 检测所有的必要参数
                foreach (var cfg in this.Configs) {
                    if (cfg.IsMust) {
                        if (!JRequest.Form.ContainsKey(cfg.Name)) {
                            JResponse.SetFail($"缺少必要的 '{cfg.Name}' 参数");
                            return JResponse.ToJsonString();
                        }
                    }
                }

                // 填充表单数据
                this.Form = new KeyValues<string>();
                foreach (var item in JRequest.Form) {
                    string key = item.Key;
                    var field = GetConfig(key);
                    if (!eggs.IsNull(field)) {
                        if (field.Enabled) {
                            switch (field.DataType) {
                                case ApiControllerFieldSetting.DataTypes.Decimal: this.Form[key] = JRequest.Form[key].ToDouble().ToString(); break;
                                case ApiControllerFieldSetting.DataTypes.Long: this.Form[key] = JRequest.Form[key].ToLong().ToString(); break;
                                case ApiControllerFieldSetting.DataTypes.Integer: this.Form[key] = JRequest.Form[key].ToInteger().ToString(); break;
                                default: this.Form[key] = JRequest.Form.String(key); break;
                            }
                        }
                    }
                }
            }

            // 表单数据模式
            if (_is_request_form) {
                // 检测所有的必要参数
                foreach (var cfg in this.Configs) {
                    if (cfg.IsMust) {
                        if (!Request.Form.ContainsKey(cfg.Name)) {
                            JResponse.SetFail($"缺少必要的 '{cfg.Name}' 参数");
                            return JResponse.ToJsonString();
                        }
                    }
                }
                // 填充表单数据
                this.Form = new KeyValues<string>();
                foreach (string key in Request.Form.Keys) {
                    var field = GetConfig(key);
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
            this.Configs = new List<ApiControllerFieldSetting>();
            SetConfig("*", ApiControllerFieldSetting.DataTypes.String, false, true);
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
                return this.JResponse.ToJsonString();
            } catch (Exception ex) {
                return Error(0, ex.Message, ex.ToString());
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
                string res = Initialize();
                if (!res.IsNoneOrNull()) return res;
                var context = this.OnCreateRenderContext();
                if (this.OnBeforeRender(context)) {
                    action(context);
                }
                this.OnAfterRender(context);
                return context.Response.ToJsonString();
            } catch (Exception ex) {
                return Error(0, ex.Message, ex.ToString());
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
            return JResponse.ToJsonString();
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
            return JResponse.ToJsonString();
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
            return JResponse.ToJsonString();
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
            return JResponse.ToJsonString();
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
            if (!info.IsNoneOrNull()) JResponse.ErrorInfo = info;
            JResponse.ErrorCode = 0;
            // 重载
            this.OnRender();
            return JResponse.ToJsonString();
        }

        #endregion

    }
}
