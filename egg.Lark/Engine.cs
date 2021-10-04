using System;
using System.Collections.Generic;

namespace egg.Lark {

    /// <summary>
    /// 百灵鸟脚本引擎
    /// </summary>
    public class Engine : IDisposable {

        public delegate MemeryUnits.Unit Function(List<MemeryUnits.Unit> args);

        // 主函数
        private MemeryUnits.Function main;
        private egg.KeyValues<MemeryUnits.Unit> vars;
        private egg.KeyValues<Function> funs;
        private List<string> pathes;
        private List<Engine> libs;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public Engine(bool isLibrary = false) {
            main = new MemeryUnits.Function(this, null, "step");
            vars = new KeyValues<MemeryUnits.Unit>();
            funs = new KeyValues<Function>();
            pathes = new List<string>();
            if (!isLibrary) {
                libs = new List<Engine>();
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
        /// 获取变量值
        /// </summary>
        /// <param name="name"></param>
        public MemeryUnits.Unit GetVariable(string name) {
            if (vars.ContainsKey(name)) return vars[name];
            if (eggs.IsNull(this.libs)) return new MemeryUnits.None();
            for (int i = 0; i < libs.Count; i++) {
                if (libs[i].CheckVariable(name)) return libs[i].GetVariable(name);
            }
            return new MemeryUnits.None();
        }

        /// <summary>
        /// 检测变量值
        /// </summary>
        /// <param name="name"></param>
        public bool CheckVariable(string name) {
            if (vars.ContainsKey(name)) return true;
            if (eggs.IsNull(this.libs)) return false;
            for (int i = 0; i < libs.Count; i++) {
                if (libs[i].CheckVariable(name)) return true;
            }
            return false;
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void RegFunction(string name, Function fun) {
            funs[name] = fun;
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
                    Engine lib = new Engine(true);
                    lib.ExecuteFile(path);
                    this.libs.Add(lib);
                    return;
                }
            }
            throw new Exception($"未找到'{name}'库文件");
        }

        // 获取函数
        internal bool CheckFunction(string name) {
            if (funs.ContainsKey(name)) return true;
            return false;
        }

        // 获取函数
        internal Function GetFunction(string name) {
            if (funs.ContainsKey(name)) return funs[name];
            return null;
        }

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
