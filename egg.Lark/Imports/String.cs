using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class String {
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 设置lark内置对象
            MemeryUnits.Object str = engine.MemeryPool.CreateObject().ToObject();
            engine.SetVariable("str", str);
            // 获取字符串长度
            str["getStringLength"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "string.getStringLength";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(str.ToString().Length, str.Handle).MemeryUnit;
            }, egg.Strings.Create("str"));
            // 截取字符串
            str["substring"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "str.substring";
                var str = args["str"];
                var start = args["start"];
                var len = args["len"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                if (start.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'start'不支持类型{start.UnitType.ToString()}");
                // 无长度则直接返回剩下的所有字符串
                if (len.UnitType == MemeryUnits.UnitTypes.None) return engine.MemeryPool.CreateString(str.ToString().Substring((int)start.ToNumber()), str.Handle).MemeryUnit;
                if (len.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'len'不支持类型{len.UnitType.ToString()}");
                return engine.MemeryPool.CreateString(str.ToString().Substring((int)start.ToNumber(), (int)len.ToNumber()), str.Handle).MemeryUnit;
            }, egg.Strings.Create("str", "start", "len"));
            // 查找字符串
            str["indexOf"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "str.indexOf";
                var str = args["str"];
                var start = args["start"];
                var key = args["key"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                if (start.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'start'不支持类型{start.UnitType.ToString()}");
                if (key.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'key'不支持类型{key.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(str.ToString().IndexOf(key.ToString(), (int)start.ToNumber()), str.Handle).MemeryUnit;
            }, egg.Strings.Create("str", "start", "key"));
            // 分割字符串
            str["split"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "str.split";
                var str = args["str"];
                var key = args["key"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                if (key.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'key'不支持类型{key.UnitType.ToString()}");
                return engine.MemeryPool.CreateList(str.ToString().Split(key.ToString()), str.Handle).MemeryUnit;
            }, egg.Strings.Create("str", "key"));
            // 判断字符串开头
            str["startWith"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "str.startWith";
                var str = args["str"];
                var key = args["key"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                if (key.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'key'不支持类型{key.UnitType.ToString()}");
                return engine.MemeryPool.CreateBoolean(str.ToString().StartsWith(key.ToString()), str.Handle).MemeryUnit;
            }, egg.Strings.Create("str", "key"));
            // 判断字符串开头
            str["endWith"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "str.endWith";
                var str = args["str"];
                var key = args["key"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                if (key.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'key'不支持类型{key.UnitType.ToString()}");
                return engine.MemeryPool.CreateBoolean(str.ToString().EndsWith(key.ToString()), str.Handle).MemeryUnit;
            }, egg.Strings.Create("str", "key"));
            // 判断字符串开头
            str["toLower"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "str.toLower";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return engine.MemeryPool.CreateString(str.ToString().ToLower(), str.Handle).MemeryUnit;
            }, egg.Strings.Create("str"));
            // 判断字符串开头
            str["toUpper"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "str.toUpper";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return engine.MemeryPool.CreateString(str.ToString().ToUpper(), str.Handle).MemeryUnit;
            }, egg.Strings.Create("str"));
        }
    }
}
