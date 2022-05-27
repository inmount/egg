using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace egg {
    /// <summary>
    /// Json值操作扩展
    /// </summary>
    public static class JsonNodeHelper {

        /// <summary>
        /// 运算符重载
        /// </summary>
        /// <param name="str"></param>
        public static string ToString(this JsonNode node) {
            string nodeType = node.GetType().FullName;
            switch (nodeType) {
                case "System.Text.Json.Nodes.JsonObject":
                case "System.Text.Json.Nodes.JsonArray":
                    throw new Exception($"类型'{nodeType}'无法转化为字符串");
            }
            object value = node.GetValue<object>();
            switch (value.GetType().FullName) {
                case "System.Text.Json.JsonElement":
                    JsonElement ele = (JsonElement)value;
                    switch (ele.ValueKind.ToString()) {
                        case "String": return ele.GetString();
                        case "Number": return "" + ele.GetDouble();
                        case "True": return true.ToString();
                        case "False": return false.ToString();
                        default: throw new Exception($"类型'{ele.ValueKind.ToString()}'无法转化为字符串");
                    }
                case "System.String": return (string)value;
                default: return value.ToString();
            }
        }

        /// <summary>
        /// 获取双精度内容
        /// </summary>
        public static double ToDouble(this JsonNode node) {
            string nodeType = node.GetType().FullName;
            switch (nodeType) {
                case "System.Text.Json.Nodes.JsonObject":
                case "System.Text.Json.Nodes.JsonArray":
                    throw new Exception($"类型'{nodeType}'无法转化为数值");
            }
            object value = node.GetValue<object>();
            switch (value.GetType().FullName) {
                case "System.Text.Json.JsonElement":
                    JsonElement ele = (JsonElement)value;
                    switch (ele.ValueKind.ToString()) {
                        case "String": return ele.GetString().ToDouble();
                        case "Number": return ele.GetDouble();
                        case "True": return 1;
                        case "False": return 0;
                        default: throw new Exception($"类型'{ele.ValueKind.ToString()}'无法转化为数值");
                    }
                case "System.String": return ((string)value).ToDouble();
                case "System.Boolean": return (bool)value ? 1 : 0;
                default: return (double)value;
            }
        }

        /// <summary>
        /// 获取单精度内容
        /// </summary>
        public static float ToFloat(this JsonNode node) {
            return (float)node.ToDouble();
        }

        /// <summary>
        /// 获取长整型内容
        /// </summary>
        public static long ToLong(this JsonNode node) {
            return (long)node.ToDouble();
        }

        /// <summary>
        /// 获取整型内容
        /// </summary>
        public static int ToInteger(this JsonNode node) {
            return (int)node.ToDouble();
        }

        /// <summary>
        /// 获取布尔型内容
        /// </summary>
        public static bool ToBoolean(this JsonNode node) {
            string nodeType = node.GetType().FullName;
            switch (nodeType) {
                case "System.Text.Json.Nodes.JsonObject":
                case "System.Text.Json.Nodes.JsonArray":
                    throw new Exception($"类型'{nodeType}'无法转化为布尔值");
            }
            object value = node.GetValue<object>();
            switch (value.GetType().FullName) {
                case "System.Text.Json.JsonElement":
                    JsonElement ele = (JsonElement)value;
                    switch (ele.ValueKind.ToString()) {
                        case "String": return ele.GetString().ToLower() == "true";
                        case "Number": return ele.GetDouble() > 0;
                        case "True": return true;
                        case "False": return false;
                        default: throw new Exception($"类型'{ele.ValueKind.ToString()}'无法转化为布尔值");
                    }
                case "System.String": return ((string)value).ToLower() == "true";
                case "System.Int32":
                case "System.Int64":
                case "System.Single":
                case "System.Double":
                    return (double)value > 0;
                case "System.Boolean":
                    return (bool)value;
                default: throw new Exception($"类型'{nodeType}'无法转化为布尔值");
            }
        }

        /// <summary>
        /// 获取对象内容
        /// </summary>
        public static JsonObject ToObject(this JsonNode node) {
            return (JsonObject)node;
        }

        /// <summary>
        /// 获取对象内容
        /// </summary>
        public static JsonArray ToArray(this JsonNode node) {
            return (JsonArray)node;
        }

    }
}
