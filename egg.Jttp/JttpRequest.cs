using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg;

namespace egg.Jttp {

    /// <summary>
    /// Jttp提交器
    /// </summary>
    public class JttpRequest : egg.BasicObject {

        private Serializable.Json.Object obj;

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
        /// 获取表单设置
        /// </summary>
        public Serializable.Json.Object Form {
            get {
                if (obj.IsNull()) return null;
                if (!obj.Keys.Contains("Form")) obj["Form"] = new Serializable.Json.Object();
                return (Serializable.Json.Object)obj["Form"];
            }
            set {
                obj["Form"] = value;
            }
        }

        /// <summary>
        /// 获取或设置所属对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Serializable.Json.Node this[string key] {
            get {
                if (obj.IsNull()) return null;
                return obj[key];
            }
            set { obj[key] = value; }
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JttpRequest() {
            obj = new Serializable.Json.Object();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="json"></param>
        public JttpRequest(string json) {
            obj = (Serializable.Json.Object)eggs.Json.Parse(json);
        }

        /// <summary>
        /// 从Json字符串中创建对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JttpRequest Create(string json) {
            return new JttpRequest(json);
        }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJsonString() {
            return obj.SerializeToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            obj.Destroy();
            base.OnDispose();
        }
    }
}
