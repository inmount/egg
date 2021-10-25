using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {
    /// <summary>
    /// 内置函数集合
    /// </summary>
    public static class Functions {

        /// <summary>
        /// 注册lark对象
        /// </summary>
        /// <param name="engine"></param>
        public static void RegLark(ScriptEngine engine) {
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
        }

        /// <summary>
        /// 注册json对象
        /// </summary>
        /// <param name="engine"></param>
        public static void RegJson(ScriptEngine engine) {
            // 设置json内置对象
            MemeryUnits.Object json = new MemeryUnits.Object();
            engine.SetVariable("json", json);
            json["getObject"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                var s = args["s"];
                if (s.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"json.getObject函数的参数's'不支持类型{s.UnitType.ToString()}");
                return Json.Parser.Parse(((MemeryUnits.String)s).Value);
            }, egg.Strings.Create("s"));
            json["getString"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                return new MemeryUnits.String(Json.Parser.GetJson(args["s"]));
            }, egg.Strings.Create("s"));
        }

        /// <summary>
        /// 注册file对象
        /// </summary>
        /// <param name="engine"></param>
        public static void RegFile(ScriptEngine engine) {
            // 设置file内置对象
            MemeryUnits.Object file = new MemeryUnits.Object();
            engine.SetVariable("file", file);
            file["readAllText"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"file.readAllText函数的参数'path'不支持类型{path.UnitType.ToString()}");
                var encoding = args["encoding"];
                if (encoding.UnitType == MemeryUnits.UnitTypes.None) encoding = new MemeryUnits.String("utf-8");
                if (encoding.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"file.readAllText函数的参数'encoding'不支持类型{path.UnitType.ToString()}");
                var encodingCode = System.Text.Encoding.GetEncoding(((MemeryUnits.String)encoding).Value.ToLower());
                using (egg.File.BinaryFile file1 = new File.BinaryFile(((MemeryUnits.String)path).Value, System.IO.FileMode.OpenOrCreate)) {
                    byte[] bytes = file1.Read(0, (int)file1.Length);
                    return new MemeryUnits.String(encodingCode.GetString(bytes));
                }
            }, egg.Strings.Create("path", "encoding"));
        }

        /// <summary>
        /// 注册time对象
        /// </summary>
        /// <param name="engine"></param>
        public static void RegTime(ScriptEngine engine) {
            // 设置file内置对象
            MemeryUnits.Object time = new MemeryUnits.Object();
            engine.SetVariable("time", time);
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
