using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class Http {
        private static egg.Net.HttpModules.HttpHeader GetHeaderFromObject(MemeryUnits.Unit ob) {
            var header = new egg.Net.HttpModules.HttpHeader();
            switch (ob.UnitType) {
                case MemeryUnits.UnitTypes.Object:
                    MemeryUnits.Object obj = (MemeryUnits.Object)ob;
                    foreach (var key in obj.Keys) {
                        header[key] = obj[key].ToString();
                    }
                    break;
                case MemeryUnits.UnitTypes.None: break;
                default:
                    throw new Exception($"不支持的转化为'HttpHeader'的类型'{ob.UnitType.ToString()}'");
            }
            return header;
        }
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 设置json内置对象
            MemeryUnits.Object http = new MemeryUnits.Object();
            engine.SetProcessVariable("http", http);
            http["get"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "http.get";
                var url = args["url"];
                var header = args["header"];
                if (url.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'url'不支持类型'{url.UnitType.ToString()}'");
                return MemeryUnits.String.Create(egg.Net.HttpClient.Get(url.ToString(), GetHeaderFromObject(header)));
            }, egg.Strings.Create("url", "header"));
            http["post"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "http.post";
                var url = args["url"];
                var arg = args["arg"];
                var header = args["header"];
                if (url.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'url'不支持类型'{url.UnitType.ToString()}'");
                if (arg.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'arg'不支持类型'{arg.UnitType.ToString()}'");
                return MemeryUnits.String.Create(egg.Net.HttpClient.Post(url.ToString(), arg.ToString(), GetHeaderFromObject(header)));
            }, egg.Strings.Create("url", "aargrgs", "header"));
        }
    }
}
