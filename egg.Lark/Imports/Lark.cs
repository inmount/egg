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
            MemeryUnits.Object lark = new MemeryUnits.Object();
            engine.SetVariable("lark", lark);
            // 获取对象的键值集合
            lark["getObjectKeys"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getObjectKeys";
                var obj = args["obj"];
                if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"{fnName}函数的参数'obj'不支持类型{obj.UnitType.ToString()}");
                var list = MemeryUnits.List.Create();
                var objKeys = ((MemeryUnits.Object)obj).Keys;
                foreach (var key in objKeys) {
                    list.Add(MemeryUnits.String.Create(key));
                }
                return list;
            }, egg.Strings.Create("obj"));
            // 判断对象是否存在对应键值
            lark["containsObjectKey"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.containsObjectKey";
                var obj = args["obj"];
                var key = args["key"];
                if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"{fnName}函数的参数'obj'不支持类型{obj.UnitType.ToString()}");
                if (key.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'key'不支持类型{obj.UnitType.ToString()}");
                var objKeys = ((MemeryUnits.Object)obj).Keys;
                return MemeryUnits.Boolean.Create(objKeys.Contains(key.ToString()));
            }, egg.Strings.Create("obj", "key"));
            // 获取列表的元素数量
            lark["getListCount"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getListCount";
                var list = args["list"];
                if (list.UnitType != MemeryUnits.UnitTypes.List) throw new Exception($"{fnName}函数的参数'list'不支持类型{list.UnitType.ToString()}");
                return MemeryUnits.Number.Create(((MemeryUnits.List)list).Count);
            }, egg.Strings.Create("list"));
            // 获取字符串长度
            lark["getStringLength"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getStringLength";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return MemeryUnits.Number.Create(str.ToString().Length);
            }, egg.Strings.Create("str"));
            // 截取字符串
            lark["substring"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.substring";
                var str = args["str"];
                var start = args["start"];
                var len = args["len"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                if (start.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'start'不支持类型{start.UnitType.ToString()}");
                // 无长度则直接返回剩下的所有字符串
                if (len.UnitType == MemeryUnits.UnitTypes.None) return MemeryUnits.String.Create(str.ToString().Substring((int)start.ToNumber()));
                if (len.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'len'不支持类型{len.UnitType.ToString()}");
                return MemeryUnits.String.Create(str.ToString().Substring((int)start.ToNumber(), (int)len.ToNumber()));
            }, egg.Strings.Create("str", "start", "len"));
            // 截取字符的码
            lark["getCode"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getCode";
                var str = args["str"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                return MemeryUnits.Number.Create(str.ToString()[0]);
            }, egg.Strings.Create("str"));
            // 截取字符的码
            lark["getChar"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.getChar";
                var code = args["code"];
                if (code.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'code'不支持类型{code.UnitType.ToString()}");
                return MemeryUnits.String.Create(((char)code.ToNumber()).ToString());
            }, egg.Strings.Create("code"));
            // 获取环境路径的集合
            lark["getPathes"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                //string fnName = "lark.getPathes";
                var list = MemeryUnits.List.Create();
                foreach (var path in engine.Pathes) {
                    list.Add(MemeryUnits.String.Create(path));
                }
                return list;
            });
            // 查找字符串
            lark["indexOf"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "lark.indexOf";
                var str = args["str"];
                var start = args["start"];
                var key = args["key"];
                if (str.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'str'不支持类型{str.UnitType.ToString()}");
                if (start.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'start'不支持类型{start.UnitType.ToString()}");
                if (key.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'key'不支持类型{key.UnitType.ToString()}");
                return MemeryUnits.Number.Create(str.ToString().IndexOf(key.ToString(), (int)start.ToNumber()));
            }, egg.Strings.Create("str", "start", "key"));
        }
    }
}
