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
    public static class JsonObjectHelper {

        /// <summary>
        /// 获取字符串内容
        /// </summary>
        public static string String(this JsonObject obj, string key) {
            JsonNode node = obj[key];
            if (eggs.IsNull(node)) return "";
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
        /// 设置字符串内容
        /// </summary>
        public static JsonObject String(this JsonObject obj, string key, string value) {
            obj[key] = value;
            return obj;
        }

        /// <summary>
        /// 获取双精度内容
        /// </summary>
        public static double Double(this JsonObject obj, string key) {
            JsonNode node = obj[key];
            if (eggs.IsNull(node)) return 0;
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
        /// 设置双精度内容
        /// </summary>
        public static JsonObject Double(this JsonObject obj, string key, double value) {
            obj[key] = value;
            return obj;
        }

        /// <summary>
        /// 获取单精度内容
        /// </summary>
        public static float Float(this JsonObject obj, string key) {
            return (float)obj.Double(key);
        }

        /// <summary>
        /// 设置单精度内容
        /// </summary>
        public static JsonObject Long(this JsonObject obj, string key, long value) {
            obj[key] = value;
            return obj;
        }

        /// <summary>
        /// 获取长整型内容
        /// </summary>
        public static long Long(this JsonObject obj, string key) {
            return (long)obj.Double(key);
        }

        /// <summary>
        /// 设置长整型内容
        /// </summary>
        public static JsonObject Int(this JsonObject obj, string key, long value) {
            obj[key] = value;
            return obj;
        }

        /// <summary>
        /// 获取整型内容
        /// </summary>
        public static int Int(this JsonObject obj, string key) {
            return (int)obj.Double(key);
        }

        /// <summary>
        /// 设置整型内容
        /// </summary>
        public static JsonObject Int(this JsonObject obj, string key, int value) {
            obj[key] = value;
            return obj;
        }

        /// <summary>
        /// 获取布尔型内容
        /// </summary>
        public static bool Bool(this JsonObject obj, string key) {
            JsonNode node = obj[key];
            if (eggs.IsNull(node)) return false;
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
        /// 设置布尔型内容
        /// </summary>
        public static JsonObject Bool(this JsonObject obj, string key, bool value) {
            obj[key] = value;
            return obj;
        }

        /// <summary>
        /// 获取对象内容
        /// </summary>
        public static JsonObject Object(this JsonObject obj, string key) {
            return (JsonObject)obj[key];
        }

        /// <summary>
        /// 获取对象内容
        /// </summary>
        public static JsonObject Object(this JsonObject obj, string key, JsonObject value) {
            obj[key] = value;
            return obj;
        }

        /// <summary>
        /// 获取对象内容
        /// </summary>
        public static JsonArray Array(this JsonObject obj, string key) {
            return (JsonArray)obj[key];
        }

        /// <summary>
        /// 获取对象内容
        /// </summary>
        public static JsonObject Array(this JsonObject obj, string key, JsonArray value) {
            obj[key] = value;
            return obj;
        }

    }
}
