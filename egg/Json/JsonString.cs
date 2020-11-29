using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 字符串单元
    /// </summary>
    public class JsonString : JsonUnit {

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JsonString(JsonUnit parent) : base(UnitType.String, parent) {
            this.Value = null;
        }

        /// <summary>  
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        private static string String2Unicode(string source) {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2) {
                stringBuilder.AppendFormat("\\u{0:x2}{1:x2}", bytes[i + 1], bytes[i]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>  
        /// 字符串转为UniCode码字符串  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string StringToUnicode(string s) {
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++) {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(System.String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }

        /// <summary>  
        /// Unicode字符串转为正常字符串  
        /// </summary>  
        /// <param name="srcText"></param>  
        /// <returns></returns>  
        private static string UnicodeToString(string srcText) {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;
            for (int i = 0; i <= len - 1; i++) {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetString() {
            return this.Value;
        }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected override double OnGetNumber() {
            if (!this.Value.IsDouble()) throw new Exception($"字符串\"{this.Value}\"无法有效的转化为数值内容");
            return this.Value.ToDouble();
        }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <param name="value"></param>
        protected override void OnSetNumber(double value) {
            this.Value = "" + value;
        }

        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="value"></param>
        protected override void OnSetString(string value) {
            this.Value = value;
        }

        /// <summary>
        /// 获取标准化的Json字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetJsonString(string value) {
            if (value == null) {
                return "null";
            } else {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                for (int i = 0; i < value.Length; i++) {
                    char c = value[i];
                    int code = (int)c;
                    if (code < 128) {
                        switch (c) {
                            case '\"':
                                sb.Append("\\\""); break;
                            case '\\':
                                sb.Append("\\\\"); break;
                            //case '/':
                            //sb.Append("\\/"); break;
                            case '\b':
                                sb.Append("\\b"); break;
                            case '\f':
                                sb.Append("\\f"); break;
                            case '\n':
                                sb.Append("\\n"); break;
                            case '\r':
                                sb.Append("\\r"); break;
                            case '\t':
                                sb.Append("\\t"); break;
                            default:
                                if (c > 31 && c < 127) {
                                    sb.Append(c);
                                }
                                break;
                        }
                    } else {
                        if (Parser.EnforceUnicode) {
                            // 转化为Unicode处理
                            sb.AppendFormat("\\u{0:x2}{1:x2}", code / 256, code % 256);
                        } else {
                            sb.Append(c);
                        }

                    }
                }
                sb.Append("\"");
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetJsonString() {
            return GetJsonString(this.Value);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            this.Value = null;

            base.OnDispose();
        }

    }
}
