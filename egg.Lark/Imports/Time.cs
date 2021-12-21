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
            MemeryUnits.Object time = engine.MemeryPool.CreateObject().ToObject();
            engine.SetProcessVariable("time", time);
            time["now"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                return engine.MemeryPool.CreateNumber(egg.Time.Now.ToMillisecondsTimeStamp(), time.Handle).MemeryUnit;
            }, egg.Strings.Create());
            time["parse"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.parse";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New(str.ToString()).ToMillisecondsTimeStamp(), time.Handle).MemeryUnit;
            }, egg.Strings.Create("str"));
            time["format"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.parse";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return engine.MemeryPool.CreateString(egg.Time.New((long)ts.ToNumber(), true).ToString(str.ToString()), time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts", "str"));
            time["year"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.year";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New((long)ts.ToNumber(), true).Year, time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts"));
            time["month"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.month";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New((long)ts.ToNumber(), true).Month, time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts"));
            time["day"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.day";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New((long)ts.ToNumber(), true).Day, time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts"));
            time["hour"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.hour";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New((long)ts.ToNumber(), true).Hour, time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts"));
            time["minute"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.minute";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New((long)ts.ToNumber(), true).Minute, time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts"));
            time["second"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.second";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New((long)ts.ToNumber(), true).Second, time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts"));
            time["millisecond"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "time.millisecond";
                var ts = args["ts"];
                if (ts.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'ts'不支持类型{ts.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(egg.Time.New((long)ts.ToNumber(), true).Millisecond, time.Handle).MemeryUnit;
            }, egg.Strings.Create("ts"));
        }
    }
}
