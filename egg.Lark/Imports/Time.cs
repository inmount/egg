using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class Time {
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 设置file内置对象
            MemeryUnits.Object time = new MemeryUnits.Object();
            engine.SetProcessVariable("time", time);
            time["now"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                return MemeryUnits.Number.Create(egg.Time.Now.ToMillisecondsTimeStamp());
            }, egg.Strings.Create());
            time["parse"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.parse";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New(str.ToString()).ToMillisecondsTimeStamp());
            }, egg.Strings.Create("str"));
            time["format"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.parse";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return MemeryUnits.String.Create(egg.Time.New((long)ts.ToNumber(), true).ToString(str.ToString()));
            }, egg.Strings.Create("ts", "str"));
            time["year"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.year";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New((long)ts.ToNumber(), true).Year);
            }, egg.Strings.Create("ts"));
            time["month"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.month";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New((long)ts.ToNumber(), true).Month);
            }, egg.Strings.Create("ts"));
            time["day"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.day";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New((long)ts.ToNumber(), true).Day);
            }, egg.Strings.Create("ts"));
            time["hour"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.hour";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New((long)ts.ToNumber(), true).Hour);
            }, egg.Strings.Create("ts"));
            time["minute"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.minute";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New((long)ts.ToNumber(), true).Minute);
            }, egg.Strings.Create("ts"));
            time["second"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.second";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New((long)ts.ToNumber(), true).Second);
            }, egg.Strings.Create("ts"));
            time["millisecond"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.millisecond";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return MemeryUnits.Number.Create(egg.Time.New((long)ts.ToNumber(), true).Millisecond);
            }, egg.Strings.Create("ts"));
        }
    }
}
