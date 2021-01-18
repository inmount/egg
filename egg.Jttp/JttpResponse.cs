using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg.JsonBean;

namespace egg.Jttp {

    /// <summary>
    /// Jttp获取器
    /// </summary>
    public class JttpResponse : egg.JsonBean.JObject {

        /// <summary>
        /// 获取或设置结果
        /// </summary>
        public JNumber Result { get; set; }

        /// <summary>
        /// 判断是否为成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess() { return this.Result.GetNumber() > 0; }

        /// <summary>
        /// 判断是否为失败，失败包含错误
        /// </summary>
        /// <returns></returns>
        public bool IsFail() { return this.Result.GetNumber() <= 0; }

        /// <summary>
        /// 判断是否为错误
        /// </summary>
        /// <returns></returns>
        public bool IsError() { return this.Result.GetNumber() < 0; }

        /// <summary>
        /// 设置为成功
        /// </summary>
        public void SetSuccess(string msg = null) {
            this.Result = 1;
            if (!msg.IsEmpty()) this.Message = msg;
        }

        /// <summary>
        /// 设置为成功
        /// </summary>
        public void SetSuccess(JObject data, string msg = null) {
            this.Result = 1;
            this.Data = data;
            if (!msg.IsEmpty()) this.Message = msg;
        }

        /// <summary>
        /// 设置为成功
        /// </summary>
        public void SetSuccess(JArray datas, string msg = null) {
            this.Result = 1;
            this.Datas = datas;
            if (!msg.IsEmpty()) this.Message = msg;
        }

        /// <summary>
        /// 设置为失败
        /// </summary>
        public void SetFail(string msg = null) {
            this.Result = 0;
            if (!msg.IsEmpty()) this.Message = msg;
        }

        /// <summary>
        /// 设置为失败
        /// </summary>
        public void SetError(int code = 0, string msg = null) {
            this.Result = -1;
            this.ErrorCode = code;
            if (!msg.IsEmpty()) this.Message = msg;
        }

        /// <summary>
        /// 创建一个成功获取器
        /// </summary>
        /// <param name="msg"></param>
        public static JttpResponse Success(string msg = null) {
            JttpResponse res = new JttpResponse();
            res.SetSuccess(msg);
            return res;
        }

        /// <summary>
        /// 创建一个成功获取器
        /// </summary>
        /// <param name="msg"></param>
        public static JttpResponse Success(JObject data, string msg = null) {
            JttpResponse res = new JttpResponse();
            res.SetSuccess(data, msg);
            return res;
        }

        /// <summary>
        /// 创建一个成功获取器
        /// </summary>
        /// <param name="msg"></param>
        public static JttpResponse Success(JArray datas, string msg = null) {
            JttpResponse res = new JttpResponse();
            res.SetSuccess(datas, msg);
            return res;
        }

        /// <summary>
        /// 创建一个失败获取器
        /// </summary>
        /// <param name="msg"></param>
        public static JttpResponse Fail(string msg = null) {
            JttpResponse res = new JttpResponse();
            res.SetFail(msg);
            return res;
        }

        /// <summary>
        /// 创建一个失败获取器
        /// </summary>
        /// <param name="msg"></param>
        public static JttpResponse Error(int code = 0, string msg = null) {
            JttpResponse res = new JttpResponse();
            res.SetError(code, msg);
            return res;
        }

        /// <summary>
        /// 获取或设置交互令牌
        /// </summary>
        [JsonOptional] public JString Token { get; set; }

        /// <summary>
        /// 获取或设置时间戳
        /// </summary>
        [JsonOptional] public JString Timestamp { get; set; }

        /// <summary>
        /// 获取或设置验证签名
        /// </summary>
        [JsonOptional] public JString Signature { get; set; }

        /// <summary>
        /// 获取数据
        /// </summary>
        [JsonOptional] public JObject Data { get; set; }

        /// <summary>
        /// 获取数据集
        /// </summary>
        [JsonOptional] public JArray Datas { get; set; }

        /// <summary>
        /// 获取或设置消息
        /// </summary>
        [JsonOptional] public JString Message { get; set; }

        /// <summary>
        /// 获取或设置错误号
        /// </summary>
        [JsonOptional] public JNumber ErrorCode { get; set; }

        public JttpResponse() { }

        /// <summary>
        /// 从Json字符串中创建对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JttpResponse Create(string json) {
            return (JttpResponse)eggs.ParseJson(json, typeof(JttpResponse));
        }

    }
}
