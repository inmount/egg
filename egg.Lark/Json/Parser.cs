using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace egg.Lark.Json {

    /// <summary>
    /// Json解析器
    /// </summary>
    public static class Parser {

        // 将Json节点转化为Unit节点
        private static MemeryUnits.Unit GetUnitFromJsonNode(MemeryUnits.Unit unit, JsonNode node) {
            string tpName = node.GetType().Name;
            switch (tpName) {
                case "JsonArray": // 数组
                    JsonArray arr = (JsonArray)node;
                    var list = unit.MemeryPool.CreateList(unit.Handle).ToList();
                    for (int i = 0; i < arr.Count; i++) {
                        list.Add(GetUnitFromJsonNode(list, arr[i]));
                    }
                    return list;
                case "JsonObject": // 对象
                    JsonObject ob = (JsonObject)node;
                    var obj = unit.MemeryPool.CreateObject(unit.Handle).ToObject();
                    foreach (var item in ob) {
                        obj[item.Key] = GetUnitFromJsonNode(obj, item.Value);
                    }
                    return obj;
                default: // 值
                    JsonElement ele = node.GetValue<JsonElement>();
                    switch (ele.ValueKind.ToString()) {
                        case "True": return unit.MemeryPool.CreateBoolean(true, unit.Handle).MemeryUnit;
                        case "False": return unit.MemeryPool.CreateBoolean(false, unit.Handle).MemeryUnit;
                        case "Number": return unit.MemeryPool.CreateNumber((double)node, unit.Handle).MemeryUnit;
                        default: return unit.MemeryPool.CreateString((string)node, unit.Handle).MemeryUnit;
                    }
            }
        }



        /// <summary>
        /// 从字符串中直接解析对象
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static MemeryUnits.Unit Parse(MemeryUnits.Unit unit, string json) {
            JsonNode node = JsonNode.Parse(json);
            return GetUnitFromJsonNode(unit, node);
        }

        /// <summary>
        /// 从字符串中直接解析对象
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string GetString(MemeryUnits.Unit unit) {
            StringBuilder sb = new StringBuilder();
            ScriptMemeryItem item = unit.MemeryPool.GetMemeryByHandle(unit.Handle);
            if (item.IsObject()) {
                var obj = item.ToObject();
                sb.Append("{");
                foreach (var key in obj.Keys) {
                    if (sb.Length > 1) sb.Append(",");
                    sb.Append("\"");
                    sb.Append(key);
                    sb.Append("\":");
                    sb.Append(GetString(obj[key]));
                }
                sb.Append("}");
            } else if (item.IsList()) {
                var list = item.ToList();
                sb.Append("[");
                for (int i = 0; i < list.Count; i++) {
                    if (sb.Length > 1) sb.Append(",");
                    sb.Append(GetString(list[i]));
                }
                sb.Append("]");
            } else if (item.IsString()) {
                sb.Append("\"");
                sb.Append(item.ToString());
                sb.Append("\"");
            } else {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }

    }
}
