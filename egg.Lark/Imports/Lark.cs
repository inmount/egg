using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class Lark {
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 设置lark内置对象
            MemeryUnits.Object lark = engine.MemeryPool.CreateObject().ToObject();
            engine.SetVariable("lark", lark);
            // 获取对象的键值集合
            lark["getObjectKeys"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getObjectKeys";
                var obj = args["obj"];
                if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"{fnName}函数的参数'obj'不支持类型{obj.UnitType.ToString()}");
                var list = engine.MemeryPool.CreateList(lark.Handle).ToList();
                var objKeys = ((MemeryUnits.Object)obj).Keys;
                foreach (var key in objKeys) {
                    list.Add(engine.MemeryPool.CreateString(key, lark.Handle).MemeryUnit);
                }
                return list;
            }, egg.Strings.Create("obj"));
            // 判断对象是否存在对应键值
            lark["containsObjectKey"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.containsObjectKey";
                var obj = args["obj"];
                var key = args["key"];
                if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"{fnName}函数的参数'obj'不支持类型{obj.UnitType.ToString()}");
                if (key.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'key'不支持类型{obj.UnitType.ToString()}");
                var objKeys = ((MemeryUnits.Object)obj).Keys;
                return engine.MemeryPool.CreateBoolean(objKeys.Contains(key.ToString()), lark.Handle).MemeryUnit;
            }, egg.Strings.Create("obj", "key"));
            // 获取列表的元素数量
            lark["getListCount"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getListCount";
                var list = args["list"];
                if (list.UnitType != MemeryUnits.UnitTypes.List) throw new Exception($"{fnName}函数的参数'list'不支持类型{list.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(((MemeryUnits.List)list).Count, lark.Handle).MemeryUnit;
            }, egg.Strings.Create("list"));
            // 截取字符的码
            lark["getCode"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getCode";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(str.ToString()[0], lark.Handle).MemeryUnit;
            }, egg.Strings.Create("str"));
            // 截取字符的码
            lark["getChar"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getChar";
                var code = args["code"];
                if (code.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'code'不支持类型{code.UnitType.ToString()}");
                return engine.MemeryPool.CreateString(((char)code.ToNumber()).ToString(), lark.Handle).MemeryUnit;
            }, egg.Strings.Create("code"));
            // 获取环境路径的集合
            lark["getPathes"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                //string fnName = "lark.getPathes";
                var list = engine.MemeryPool.CreateList(lark.Handle).ToList();
                foreach (var path in engine.Pathes) {
                    list.Add(engine.MemeryPool.CreateString(path, lark.Handle).MemeryUnit);
                }
                return list;
            });
            // 获取环境路径的集合
            lark["getVarNames"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                return engine.MemeryPool.CreateList(engine.GetProcessVariables(), lark.Handle).MemeryUnit;
            });
        }
    }
}
