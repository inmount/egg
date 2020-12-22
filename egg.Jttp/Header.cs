using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Jttp {

    /// <summary>
    /// 头部定义
    /// </summary>
    public class Header {

        // 头部对象
        private egg.Json.JsonObject _obj;

        /// <summary>
        /// 获取或设置版本信息
        /// </summary>
        public string Ver { get { return _obj.String("Ver"); } set { _obj.String("Ver", value); } }

        /// <summary>
        /// 获取或设置交互类型
        /// </summary>
        public string Type { get { return _obj.String("Type"); } set { _obj.String("Type", value); } }

        /// <summary>
        /// 获取或设置交互信息
        /// </summary>
        public string SessionID { get { return _obj.String("SessionID"); } set { _obj.String("SessionID", value); } }

        /// <summary>
        /// 获取或设置时间戳信息
        /// </summary>
        public long Time { get { return (long)_obj.Number("Time"); } set { _obj.Number("Time", value); } }

        /// <summary>
        /// 获取或设置验证加盐
        /// </summary>
        public string VerifySalt { get { return _obj.String("VerifySalt"); } set { _obj.String("VerifySalt", value); } }

        /// <summary>
        /// 获取或设置验证加盐
        /// </summary>
        public string VerifyType { get { return _obj.String("VerifyType"); } set { _obj.String("VerifyType", value); } }

        /// <summary>
        /// 获取或设置验证签名
        /// </summary>
        public string VerifySign { get { return _obj.String("VerifySign"); } set { _obj.String("VerifySign", value); } }

        /// <summary>
        /// 获取或设置状态信息
        /// </summary>
        public int Status { get { return (int)_obj.Number("Status"); } set { _obj.Number("Status", value); } }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(Header header = null) {
            if (!eggs.IsNull(header)) {
                this.Ver = header.Ver;
                this.Type = header.Type;
                this.SessionID = header.SessionID;
            } else {
                this.Ver = "1.0";
            }
            this.Time = egg.Time.Now.ToTimeStamp();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="obj"></param>
        public Header(egg.Json.JsonObject obj) {
            _obj = obj;
        }

    }
}
