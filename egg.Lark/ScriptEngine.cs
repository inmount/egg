using System;
using System.Collections.Generic;

namespace egg.Lark {

    /// <summary>
    /// 百灵鸟脚本引擎
    /// </summary>
    public class ScriptEngine : IDisposable {

        #region [=====静态功能=====]

        /// <summary>
        /// 获取是否为可执行函数
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool IsFunction(MemeryUnits.Unit unit) { return (unit.UnitType == MemeryUnits.UnitTypes.Function || unit.UnitType == MemeryUnits.UnitTypes.NativeFunction); }

        /// <summary>
        /// 执行函数并返回结果
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static MemeryUnits.Unit ExecuteFunction(MemeryUnits.Function parent, MemeryUnits.Unit fn) {
            if (!IsFunction(fn)) throw new Exception("执行对象并非函数");
            if (fn.UnitType == MemeryUnits.UnitTypes.NativeFunction) {
                // 外部函数
                MemeryUnits.NativeFunction func = (MemeryUnits.NativeFunction)fn;
                egg.KeyValues<MemeryUnits.Unit> args = new KeyValues<MemeryUnits.Unit>();
                if (!eggs.IsNull(func.Keys)) {
                    if (parent.Params.Count > func.Keys.Count) throw new Exception($"函数'{parent.Name}'只允许{func.Keys.Count}个参数");
                    for (int i = 0; i < func.Keys.Count; i++) {
                        var valSource = parent.Params[i].GetMemeryUnit();
                        if (valSource.UnitType == MemeryUnits.UnitTypes.Function) valSource = ((MemeryUnits.Function)valSource).Execute();
                        args[func.Keys[i]] = valSource;
                    }
                }
                return ((MemeryUnits.NativeFunction)fn).Execute(args);
            } else {
                // 自定义函数
                MemeryUnits.Function func = (MemeryUnits.Function)fn;
                if (parent.Params.Count >= func.Params.Count) throw new Exception($"函数'{parent.Name}'只允许{func.Params.Count - 1}个参数");
                egg.KeyValues<MemeryUnits.Unit> args = new KeyValues<MemeryUnits.Unit>();
                for (int i = 0; i < func.Params.Count - 1; i++) {
                    var valSource = parent.Params[i].GetMemeryUnit();
                    if (valSource.UnitType == MemeryUnits.UnitTypes.Function) valSource = ((MemeryUnits.Function)valSource).Execute();
                    args[((ProcessUnits.Define)func.Params[i]).Name] = valSource;
                }
                return ((MemeryUnits.Function)func.Params[func.Params.Count - 1].GetMemeryUnit()).Execute(args);
            }
        }

        #endregion

        public delegate MemeryUnits.Unit Function(egg.KeyValues<MemeryUnits.Unit> args);

        // 主函数
        private MemeryUnits.Function main;
        private egg.KeyValues<MemeryUnits.Unit> vars;
        //private egg.KeyValues<Function> funs;
        private List<string> pathes;
        private List<ScriptEngine> libs;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public ScriptEngine(bool isLibrary = false) {
            main = new MemeryUnits.Function(this, null, "step");
            vars = new KeyValues<MemeryUnits.Unit>();
            //funs = new KeyValues<Function>();
            pathes = new List<string>();
            if (!isLibrary) {
                libs = new List<ScriptEngine>();
                Functions.Reg(this);
            }
        }

        /// <summary>
        /// 添加函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public MemeryUnits.Function AddFunction(string name, Params args = null) {
            ProcessUnits.Pointer res = main.Params.AddFunction(name, args);
            MemeryUnits.Function fn = (MemeryUnits.Function)res.GetMemeryUnit();
            return fn;
        }

        /// <summary>
        /// 设置初始化变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetVariable(string name, MemeryUnits.Unit value) {
            vars[name] = value;
        }

        /// <summary>
        /// 设置初始化变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProcessVariable(string name, MemeryUnits.Unit value) {
            main.SetVarValue(name, value, true);
        }

        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <param name="name"></param>
        public MemeryUnits.Unit GetProcessVariable(string name) {
            if (main.CheckVar(name)) return main.GetVarValue(name);
            if (eggs.IsNull(this.libs)) return new MemeryUnits.None();
            for (int i = 0; i < libs.Count; i++) {
                if (libs[i].CheckProcessVariable(name)) return libs[i].GetProcessVariable(name);
            }
            return new MemeryUnits.None();
        }

        /// <summary>
        /// 检测变量值
        /// </summary>
        /// <param name="name"></param>
        public bool CheckProcessVariable(string name) {
            if (main.CheckVar(name)) return true;
            if (eggs.IsNull(this.libs)) return false;
            for (int i = 0; i < libs.Count; i++) {
                if (libs[i].CheckProcessVariable(name)) return true;
            }
            return false;
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="keys"></param>
        /// <param name="fun"></param>
        public void RegFunction(string name, Function fun, egg.Strings keys = null) {
            //funs[name] = fun;
            this.SetVariable(name, new MemeryUnits.NativeFunction(fun, keys));
        }

        /// <summary>
        /// 添加路径
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddPath(string path) {
            path = path.Replace("\\", "/");
            if (!path.EndsWith("/")) path += "/";
            pathes.Add(path);
        }

        // 引入文件
        internal void Include(string name) {
            for (int i = 0; i < pathes.Count; i++) {
                string path = $"{pathes[i]}{name}.lark";
                if (eggs.CheckFileExists(path)) {
                    ScriptEngine lib = new ScriptEngine(true);
                    lib.ExecuteFile(path);
                    this.libs.Add(lib);
                    return;
                }
            }
            throw new Exception($"未找到'{name}'库文件");
        }

        //// 获取函数
        //internal bool CheckFunction(string name) {
        //    if (funs.ContainsKey(name)) return true;
        //    return false;
        //}

        //// 获取函数
        //internal Function GetFunction(string name) {
        //    if (funs.ContainsKey(name)) return funs[name];
        //    return null;
        //}

        /// <summary>
        /// 执行代码
        /// </summary>
        public void Execute() {
            main.Execute(vars);
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        public void Execute(string script) {
            Parser.Parse(this, script);
            main.Execute(vars);
        }

        /// <summary>
        /// 执行文件
        /// </summary>
        public void ExecuteFile(string path) {
            string script = egg.File.UTF8File.ReadAllText(path);
            Parser.Parse(this, script);
            main.Execute(vars);
        }

        public void Dispose() {
            main.Dispose();
        }
    }
}
