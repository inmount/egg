using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class Hash {
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 设置hash内置对象
            MemeryUnits.Object hash = engine.MemeryPool.CreateObject().ToObject();
            engine.SetProcessVariable("hash", hash);
            hash["md5"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "hash.md5";
                var content = args["content"];
                if (content.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'content'不支持类型'{content.UnitType.ToString()}'");
                return engine.MemeryPool.CreateString(content.ToString().GetMD5()).MemeryUnit;
            }, egg.Strings.Create("content"));
            hash["getFileMD5"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "hash.getFileMD5";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'content'不支持类型'{path.UnitType.ToString()}'");
                return engine.MemeryPool.CreateString(egg.File.BinaryFile.GetMD5(path.ToString())).MemeryUnit;
            }, egg.Strings.Create("path"));
            hash["sha1"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "hash.sha1";
                var content = args["content"];
                if (content.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'content'不支持类型'{content.UnitType.ToString()}'");
                return engine.MemeryPool.CreateString(content.ToString().GetSha1()).MemeryUnit;
            }, egg.Strings.Create("content"));
            hash["sha256"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "hash.sha256";
                var content = args["content"];
                if (content.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'content'不支持类型'{content.UnitType.ToString()}'");
                return engine.MemeryPool.CreateString(content.ToString().GetSha256()).MemeryUnit;
            }, egg.Strings.Create("content"));
            hash["sha512"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "hash.sha512";
                var content = args["content"];
                if (content.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'content'不支持类型'{content.UnitType.ToString()}'");
                return engine.MemeryPool.CreateString(content.ToString().GetSha512()).MemeryUnit;
            }, egg.Strings.Create("content"));
        }
    }
}
