using System;
using System.Collections.Generic;
using System.Text;
using egg;

namespace egg.Jttp {

    /// <summary>
    /// 签名器
    /// </summary>
    public static class Signer {

        /// <summary>
        /// 加签运算
        /// </summary>
        public static string SignUp(string tp, string token, long timestamp, string salt, string key, string attach = null) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // 添加加签类型
            sb.AppendFormat("$type={0}", tp);

            // 添加盐
            sb.AppendFormat("$salt={0}", salt);

            // 添加时间戳
            sb.AppendFormat("$time={0}", timestamp);

            // 添加交互标识
            sb.AppendFormat("$session={0}", token);

            // 添加附加信息
            if (!attach.IsEmpty()) sb.AppendFormat("$attach={0}", attach);

            // 添加加签密钥
            sb.AppendFormat("$key={0}", key);

            // 计算签名
            switch (tp) {
                case "md5": return sb.ToString().GetMD5();
                case "sha1": return sb.ToString().GetSha1();
                case "sha256": return sb.ToString().GetSha256();
                case "sha512": return sb.ToString().GetSha512();
                default: throw new Exception("不支持的加签算法");
            }

        }

        /// <summary>
        /// 加签运算
        /// </summary>
        public static string SignUp(string tp, string token, string timestamp, string key) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // 添加时间戳
            sb.AppendFormat("$time={0}", timestamp);

            // 添加交互标识
            sb.AppendFormat("$token={0}", token);

            // 添加加签类型
            sb.AppendFormat("$type={0}", token);

            // 添加加签密钥
            sb.AppendFormat("$key={0}", key);

            // 计算签名
            switch (tp) {
                case "md5": return sb.ToString().GetMD5();
                case "sha1": return sb.ToString().GetSha1();
                case "sha256": return sb.ToString().GetSha256();
                case "sha512": return sb.ToString().GetSha512();
                default: throw new Exception("不支持的加签算法");
            }

        }

        /// <summary>
        /// MD5加签运算
        /// </summary>
        public static string SignUpMD5(string token, string timestamp, string key) { return SignUp("md5", token, timestamp, key); }

        /// <summary>
        /// sha1加签运算
        /// </summary>
        public static string SignUpSha1(string token, string timestamp, string key) { return SignUp("sha1", token, timestamp, key); }

        /// <summary>
        /// sha256加签运算
        /// </summary>
        public static string SignUpSha256(string token, string timestamp, string key) { return SignUp("sha256", token, timestamp, key); }

        /// <summary>
        /// sha512加签运算
        /// </summary>
        public static string SignUpSha512(string token, string timestamp, string key) { return SignUp("sha512", token, timestamp, key); }

    }
}
