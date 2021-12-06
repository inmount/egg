using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class Json {
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 设置json内置对象
            MemeryUnits.Object json = new MemeryUnits.Object();
            engine.SetVariable("json", json);
            json["getObject"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                var s = args["s"];
                if (s.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"json.getObject函数的参数's'不支持类型{s.UnitType.ToString()}");
                return egg.Lark.Json.Parser.Parse(((MemeryUnits.String)s).Value);
            }, egg.Strings.Create("s"));
            json["getString"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                return new MemeryUnits.String(egg.Lark.Json.Parser.GetJson(args["s"]));
            }, egg.Strings.Create("s"));
        }
    }
}
