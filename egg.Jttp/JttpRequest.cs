using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg.JsonBean;

namespace egg.Jttp {

    /// <summary>
    /// Jttp提交器
    /// </summary>
    public class JttpRequest : egg.JsonBean.JObject {

        /// <summary>
        /// 获取或设置交互令牌
        /// </summary>
        public JString Token { get; set; }

        /// <summary>
        /// 获取或设置时间戳
        /// </summary>
        [JsonOptional] public JString Timestamp { get; set; }

        /// <summary>
        /// 获取或设置验证签名
        /// </summary>
        [JsonOptional] public JString Signature { get; set; }

        /// <summary>
        /// 获取表单设置
        /// </summary>
        public JObject Form { get; private set; }

        public JttpRequest() {
            this.Form = new JObject();
        }

        /// <summary>
        /// 从Json字符串中创建对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JttpRequest Create(string json) {
            return (JttpRequest)eggs.ParseJson(json, typeof(JttpRequest));
        }

    }
}
