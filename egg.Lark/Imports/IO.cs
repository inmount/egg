using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class IO {
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 设置file内置对象
            MemeryUnits.Object io = new MemeryUnits.Object();
            engine.SetProcessVariable("io", io);
            // 目录存在性判断
            io["folderExists"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.folderExists";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                return MemeryUnits.Boolean.Create(eggs.CheckFolderExists(path.ToString()));
            }, egg.Strings.Create("path"));
            // 目录创建
            io["folderCreate"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.folderCreate";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                System.IO.Directory.CreateDirectory(path.ToString());
                return MemeryUnits.None.Create();
            }, egg.Strings.Create("path"));
            // 目录删除
            io["folderDelete"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.folderDelete";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                System.IO.Directory.Delete(path.ToString(), true);
                return MemeryUnits.None.Create();
            }, egg.Strings.Create("path"));
            // 目录文件存在性判断
            io["fileExists"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.fileExists";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                return MemeryUnits.Boolean.Create(eggs.CheckFileExists(path.ToString()));
            }, egg.Strings.Create("path"));
            // 文件创建
            io["fileCreate"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.fileCreate";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                System.IO.File.Create(path.ToString());
                return MemeryUnits.None.Create();
            }, egg.Strings.Create("path"));
            // 文件删除
            io["fileDelete"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.folderDelete";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                System.IO.File.Delete(path.ToString());
                return MemeryUnits.None.Create();
            }, egg.Strings.Create("path"));
            // 文件复制
            io["fileCopy"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.fileCopy";
                var pathSource = args["pathSource"];
                if (pathSource.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{pathSource.UnitType.ToString()}");
                var pathTarget = args["pathTarget"];
                if (pathTarget.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{pathTarget.UnitType.ToString()}");
                System.IO.File.Copy(pathSource.ToString(), pathTarget.ToString(), true);
                return MemeryUnits.None.Create();
            }, egg.Strings.Create("pathSource", "pathTarget"));
            // 文件复制
            io["fileMove"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.fileMove";
                var pathSource = args["pathSource"];
                if (pathSource.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{pathSource.UnitType.ToString()}");
                var pathTarget = args["pathTarget"];
                if (pathTarget.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{pathTarget.UnitType.ToString()}");
                System.IO.File.Move(pathSource.ToString(), pathTarget.ToString(), true);
                return MemeryUnits.None.Create();
            }, egg.Strings.Create("pathSource", "pathTarget"));
            // 读取全部文本
            io["readAllText"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.readAllText";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                var encoding = args["encoding"];
                if (encoding.UnitType == MemeryUnits.UnitTypes.None) encoding = new MemeryUnits.String("utf-8");
                if (encoding.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'encoding'不支持类型{path.UnitType.ToString()}");
                var encodingCode = System.Text.Encoding.GetEncoding(((MemeryUnits.String)encoding).Value.ToLower());
                using (egg.File.BinaryFile file1 = new egg.File.BinaryFile(((MemeryUnits.String)path).Value, System.IO.FileMode.OpenOrCreate)) {
                    byte[] bytes = file1.Read(0, (int)file1.Length);
                    return new MemeryUnits.String(encodingCode.GetString(bytes));
                }
            }, egg.Strings.Create("path", "encoding"));
            // 写入全部文本
            io["writeAllText"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.writeAllText";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                var encoding = args["encoding"];
                if (encoding.UnitType == MemeryUnits.UnitTypes.None) encoding = new MemeryUnits.String("utf-8");
                if (encoding.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'encoding'不支持类型{path.UnitType.ToString()}");
                var encodingCode = System.Text.Encoding.GetEncoding(((MemeryUnits.String)encoding).Value.ToLower());
                var content = args["content"];
                if (content.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'content'不支持类型{path.UnitType.ToString()}");
                byte[] bytes = encodingCode.GetBytes(content.ToString());
                using (egg.File.BinaryFile file1 = new egg.File.BinaryFile(((MemeryUnits.String)path).Value, System.IO.FileMode.Create)) {
                    file1.Write(0, bytes);
                }
                return MemeryUnits.None.Create();
            }, egg.Strings.Create("path", "content", "encoding"));
            // 获取子目录
            io["getFolders"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.getFolders";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                var pattern = args["pattern"];
                string[] pathes;
                switch (pattern.UnitType) {
                    case MemeryUnits.UnitTypes.None:
                        pathes = System.IO.Directory.GetDirectories(path.ToString());
                        break;
                    case MemeryUnits.UnitTypes.String:
                        pathes = System.IO.Directory.GetDirectories(path.ToString(), pattern.ToString());
                        break;
                    default:
                        throw new Exception($"{fnName}函数的参数'pattern'不支持类型{pattern.UnitType.ToString()}");
                }
                var list = MemeryUnits.List.Create();
                for (int i = 0; i < pathes.Length; i++) {
                    list.Add(MemeryUnits.String.Create(pathes[i]));
                }
                return list;
            }, egg.Strings.Create("path", "pattern"));
            // 获取文件
            io["getFiles"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.getFiles";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                var pattern = args["pattern"];
                string[] pathes;
                switch (pattern.UnitType) {
                    case MemeryUnits.UnitTypes.None:
                        pathes = System.IO.Directory.GetFiles(path.ToString());
                        break;
                    case MemeryUnits.UnitTypes.String:
                        pathes = System.IO.Directory.GetFiles(path.ToString(), pattern.ToString());
                        break;
                    default:
                        throw new Exception($"{fnName}函数的参数'pattern'不支持类型{pattern.UnitType.ToString()}");
                }
                var list = MemeryUnits.List.Create();
                for (int i = 0; i < pathes.Length; i++) {
                    list.Add(MemeryUnits.String.Create(pathes[i]));
                }
                return list;
            }, egg.Strings.Create("path", "pattern"));
            // 获取文件名
            io["getFileName"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.getFileName";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                return MemeryUnits.String.Create(System.IO.Path.GetFileName(path.ToString()));
            }, egg.Strings.Create("path"));
            // 获取父目录
            io["getParentFolder"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "io.getParentFolder";
                var path = args["path"];
                if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception($"{fnName}函数的参数'path'不支持类型{path.UnitType.ToString()}");
                return MemeryUnits.String.Create(System.IO.Path.GetDirectoryName(path.ToString()));
            }, egg.Strings.Create("path"));
            // 获取驱动器列表
            io["getDrives"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                //string fnName = "io.getDrives";
                string[] drives= System.IO.Directory.GetLogicalDrives();
                var list = MemeryUnits.List.Create();
                for (int i = 0; i < drives.Length; i++) {
                    list.Add(MemeryUnits.String.Create(drives[i]));
                }
                return list;
            }, egg.Strings.Create());
        }
    }
}
