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
            MemeryUnits.Object json = engine.MemeryPool.CreateObject().ToObject();
            engine.SetProcessVariable("json", json);
            json["getObject"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                string fnName = "json.getObject";
                var s = args["s"];
                if (s.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数's'不支持类型{s.UnitType.ToString()}");
                return egg.Lark.Json.Parser.Parse(engine.MemeryPool.None, s.ToString());
            }, egg.Strings.Create("s"));
            json["getString"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                return engine.MemeryPool.CreateString(egg.Lark.Json.Parser.GetString(args["s"]), json.Handle).MemeryUnit;
            }, egg.Strings.Create("s"));
        }
    }
}
