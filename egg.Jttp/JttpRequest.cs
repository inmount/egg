using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace egg.Jttp {

    /// <summary>
    /// Jttp提交器
    /// </summary>
    public class JttpRequest : egg.Object {

        private JsonNode _node;

        /// <summary>
        /// 获取或设置交互令牌
        /// </summary>
        public string Token {
            get {
                if (eggs.IsNull(_node)) return null;
                return (string)_node["Token"];
            }
            set {
                _node["Token"] = value;
            }
        }

        /// <summary>
        /// 获取或设置时间戳
        /// </summary>
        public string Timestamp {
            get {
                if (eggs.IsNull(_node)) return null;
                return (string)_node["Timestamp"];
            }
            set {
                _node["Timestamp"] = value;
            }
        }

        /// <summary>
        /// 获取或设置验证签名
        /// </summary>
        public string Signature {
            get {
                if (eggs.IsNull(_node)) return null;
                return (string)_node["Signature"];
            }
            set {
                _node["Signature"] = value;
            }
        }

        /// <summary>
        /// 获取表单设置
        /// </summary>
        public JsonObject Form {
            get {
                if (eggs.IsNull(_node)) return null;
                if (eggs.IsNull(_node["Form"])) _node["Form"] = new JsonObject();
                return (JsonObject)_node["Form"];
            }
        }

        public JsonNode this[string key] {
            get {
                if (eggs.IsNull(_node)) return null;
                return _node[key];
            }
            set { _node[key] = value; }
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="json"></param>
        public JttpRequest() {
            _node = new JsonObject();
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="json"></param>
        public JttpRequest(string json) {
            _node = JsonNode.Parse(json);
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
            return _node.ToJsonString();
        }

        protected override void OnDispose() {
            _node = null;
            //throw new NotImplementedException();
        }
    }
}
