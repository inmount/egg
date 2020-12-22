using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Mvc {

    /// <summary>
    /// 支持ssr.SessionServer的WebApi基础类
    /// </summary>
    public abstract class ApiControllerSecuritySessionBase : ApiControllerSessionBase {

        /// <summary>
        /// 验证符号
        /// </summary>
        protected string VerifyToken { get; set; }

        /// <summary>
        /// 验证加盐
        /// </summary>
        protected string VerifyType { get; set; }

        /// <summary>
        /// 验证密钥
        /// </summary>
        protected string VerifyKey { get; set; }

        // 获取签名
        private string GetSign(string token, string salt, long timetamp, string key, string type = "md5", string attach = null) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // 添加加签类型
            sb.AppendFormat("$type={0}", type);

            // 添加盐
            sb.AppendFormat("$salt={0}", salt);

            // 添加时间戳
            sb.AppendFormat("$time={0}", timetamp);

            // 添加交互标识
            sb.AppendFormat("$token={0}", token);

            // 添加附加信息
            if (!attach.IsNoneOrNull()) sb.AppendFormat("$attach={0}", attach);

            // 添加加签密钥
            sb.AppendFormat("$key={0}", key);

            switch (type) {
                case "md5": return sb.ToString().GetMD5();
                case "sha1": return sb.ToString().GetSha1();
                case "sha256": return sb.ToString().GetSha256();
                case "sha512": return sb.ToString().GetSha512();
                default: throw new Exception("不支持的加签算法");
            }
        }

        // 头部信息加签
        private void SignUp() {
            // 输出头部信息
            string salt = Guid.NewGuid().ToString().Replace("-", "");
            long ts =egg.Time.Now.ToTimeStamp();

            // Verify_Token
            if (Response.Headers.ContainsKey("Verify_Token")) {
                Response.Headers["Verify_Token"] = this.VerifyToken;
            } else {
                Response.Headers.Add("Verify_Token", this.VerifyToken);
            }

            // Verify_Salt
            if (Response.Headers.ContainsKey("Verify_Salt")) {
                Response.Headers["Verify_Salt"] = salt;
            } else {
                Response.Headers.Add("Verify_Salt", salt);
            }

            // Verify_Type
            if (this.VerifyType.IsNoneOrNull()) this.VerifyType = "md5";
            if (Response.Headers.ContainsKey("Verify_Type")) {
                Response.Headers["Verify_Type"] = this.VerifyType;
            } else {
                Response.Headers.Add("Verify_Type", this.VerifyType);
            }


            // Verify_Time
            if (Response.Headers.ContainsKey("Verify_Time")) {
                Response.Headers["Verify_Time"] = $"{ts}";
            } else {
                Response.Headers.Add("Verify_Time", $"{ts}");
            }

            // Verify_Sign
            string sign = GetSign(this.VerifyToken, salt, ts, this.VerifyKey, this.VerifyType);
            if (Response.Headers.ContainsKey("Verify_Sign")) {
                Response.Headers["Verify_Sign"] = sign;
            } else {
                Response.Headers.Add("Verify_Sign", sign);
            }
        }

        /// <summary>
        /// 呈现内容
        /// </summary>
        /// <returns></returns>
        protected override void OnRender() {
            // 呈现时进行加签
            this.SignUp();
        }

        /// <summary>
        /// 检查签名有效性
        /// </summary>
        /// <returns></returns>
        protected bool CheckSign(string attach = null) {

            // 获取交互标识
            string token = "";
            if (Request.Headers.ContainsKey("Verify_Token")) {
                token = Request.Headers["Verify_Token"].ToString();
            }

            // 获取加签盐
            string salt = "";
            if (Request.Headers.ContainsKey("Verify_Salt")) {
                salt = Request.Headers["Verify_Salt"].ToString();
            }

            // 获取验证时间戳
            long ts = 0;
            if (Request.Headers.ContainsKey("Verify_Time")) {
                ts = Request.Headers["Verify_Time"].ToString().ToLong();
            }

            // 获取加签类型
            string type = "";
            if (Request.Headers.ContainsKey("Verify_Type")) {
                type = Request.Headers["Verify_Type"].ToString();
            }

            // 获取签名
            string sign = "";
            if (Request.Headers.ContainsKey("Verify_Sign")) {
                sign = Request.Headers["Verify_Sign"].ToString();
            }

            // 计算签名
            string signNew = GetSign(token, salt, ts, this.VerifyKey, type, attach);

            // 返回对比结果
            return signNew.Equals(sign);
        }

        /// <summary>
        /// 事件初始化重载
        /// </summary>
        /// <returns></returns>
        protected override string OnInit() {

            // 获取交互标识
            this.VerifyToken = "";
            if (Request.Headers.ContainsKey("Verify_Token")) {
                this.VerifyToken = Request.Headers["Verify_Token"].ToString();
            }

            return base.OnInit();
        }

    }
}
