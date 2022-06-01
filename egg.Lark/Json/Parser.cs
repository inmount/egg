using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg;

namespace egg.Lark.Json {

    /// <summary>
    /// Json解析器
    /// </summary>
    public static class Parser {

        // 将Json节点转化为Unit节点
        private static MemeryUnits.Unit GetUnitFromJsonNode(MemeryUnits.Unit unit, Serializable.Json.Node node) {
            switch (node.NodeType) {
                case Serializable.Json.NodeTypes.List: // 数组
                    Serializable.Json.List arr = (Serializable.Json.List)node;
                    var list = unit.MemeryPool.CreateList(unit.Handle).ToList();
                    for (int i = 0; i < arr.Count; i++) {
                        list.Add(GetUnitFromJsonNode(list, arr[i]));
                    }
                    return list;
                case Serializable.Json.NodeTypes.Object: // 对象
                    Serializable.Json.Object ob = (Serializable.Json.Object)node;
                    var obj = unit.MemeryPool.CreateObject(unit.Handle).ToObject();
                    foreach (var key in ob.Keys) {
                        obj[key] = GetUnitFromJsonNode(obj, ob[key]);
                    }
                    return obj;
                case Serializable.Json.NodeTypes.Null: // 空
                    return null;
                case Serializable.Json.NodeTypes.Number: // 数值
                    return unit.MemeryPool.CreateNumber((double)node, unit.Handle).MemeryUnit;
                case Serializable.Json.NodeTypes.Boolean: // 布尔
                    return unit.MemeryPool.CreateBoolean((bool)node, unit.Handle).MemeryUnit;
                default: // 默认按照字符串处理 
                    return unit.MemeryPool.CreateString((string)node, unit.Handle).MemeryUnit;
            }
        }



        /// <summary>
        /// 从字符串中直接解析对象
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static MemeryUnits.Unit Parse(MemeryUnits.Unit unit, string json) {
            Serializable.Json.Node node = eggs.Json.Parse(json);
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
