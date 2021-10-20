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
        /// 注册器
        /// </summary>
        /// <param name="engine"></param>
        public static void Reg(ScriptEngine engine) {
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

            // 设置file内置对象
            MemeryUnits.Object file = new MemeryUnits.Object();
            engine.SetVariable("file", file);
            json["readAllText"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
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
    }
}
