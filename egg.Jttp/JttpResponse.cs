﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Jttp {

    /// <summary>
    /// Jttp获取器
    /// </summary>
    public class JttpResponse : egg.BasicObject {

        private Serializable.Json.Object obj;

        /// <summary>
        /// 获取或设置结果
        /// </summary>
        public int Result {
            get {
                if (obj.IsNull()) return 0;
                return (int)obj["Result"];
            }
            set {
                obj["Result"] = value;
            }
        }

        /// <summary>
        /// 判断是否为成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess() { return this.Result > 0; }

        /// <summary>
        /// 判断是否为失败，失败包含错误
        /// </summary>
        /// <returns></returns>
        public bool IsFail() { return this.Result <= 0; }

        /// <summary>
        /// 判断是否为错误
        /// </summary>
        /// <returns></returns>
        public bool IsError() { return this.Result < 0; }

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
        public void SetSuccess(Serializable.Json.Object data, string msg = null) {
            this.Result = 1;
            this.Data = data;
            if (!msg.IsEmpty()) this.Message = msg;
        }

        /// <summary>
        /// 设置为成功
        /// </summary>
        public void SetSuccess(Serializable.Json.List datas, string msg = null) {
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
        public void SetError(int code = 0, string msg = null, string info = null) {
            this.Result = -1;
            this.ErrorCode = code;
            if (!msg.IsEmpty()) this.Message = msg;
            if (!info.IsEmpty()) this.ErrorInfo = info;
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
        public static JttpResponse Success(Serializable.Json.Object data, string msg = null) {
            JttpResponse res = new JttpResponse();
            res.SetSuccess(data, msg);
            return res;
        }

        /// <summary>
        /// 创建一个成功获取器
        /// </summary>
        /// <param name="msg"></param>
        public static JttpResponse Success(Serializable.Json.List datas, string msg = null) {
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
        public static JttpResponse Error(int code = 0, string msg = null, string info = null) {
            JttpResponse res = new JttpResponse();
            res.SetError(code, msg, info);
            return res;
        }

        /// <summary>
        /// 获取或设置交互令牌
        /// </summary>
        public string Token {
            get {
                if (obj.IsNull()) return null;
                return (string)obj["Token"];
            }
            set {
                obj["Token"] = value;
            }
        }

        /// <summary>
        /// 获取或设置时间戳
        /// </summary>
        public string Timestamp {
            get {
                if (obj.IsNull()) return null;
                return (string)obj["Timestamp"];
            }
            set {
                obj["Timestamp"] = value;
            }
        }

        /// <summary>
        /// 获取或设置验证签名
        /// </summary>
        public string Signature {
            get {
                if (obj.IsNull()) return null;
                return (string)obj["Signature"];
            }
            set {
                obj["Signature"] = value;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public Serializable.Json.Object Data {
            get {
                if (obj.IsNull()) return null;
                return (Serializable.Json.Object)obj["Data"];
            }
            set {
                obj["Data"] = value;
            }
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        public Serializable.Json.List Datas {
            get {
                if (obj.IsNull()) return null;
                return (Serializable.Json.List)obj["Datas"];
            }
            set {
                obj["Datas"] = value;
            }
        }

        /// <summary>
        /// 获取或设置消息
        /// </summary>
        public string Message {
            get {
                if (obj.IsNull()) return null;
                return (string)obj["Message"];
            }
            set {
                obj["Message"] = value;
            }
        }

        /// <summary>
        /// 获取或设置错误号
        /// </summary>
        public int ErrorCode {
            get {
                if (obj.IsNull()) return 0;
                return (int)obj["ErrorCode"];
            }
            set {
                obj["ErrorCode"] = value;
            }
        }

        /// <summary>
        /// 获取或设置错误信息
        /// </summary>
        public string ErrorInfo {
            get {
                if (obj.IsNull()) return null;
                return (string)obj["ErrorInfo"];
            }
            set {
                obj["ErrorInfo"] = value;
            }
        }

        public JttpResponse() {
            obj = new Serializable.Json.Object();
        }

        public JttpResponse(string json) {
            obj = (Serializable.Json.Object)eggs.Json.Parse(json);
        }

        /// <summary>
        /// 从Json字符串中创建对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JttpResponse Create(string json) {
            return new JttpResponse(json);
        }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() {
            return obj.SerializeToString();
        }

    }
}