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
                ScriptEngine.FunctionArgs args = new ScriptEngine.FunctionArgs();
                if (!eggs.IsNull(func.Keys)) {
                    if (parent.Params.Count > func.Keys.Count) throw new Exception($"函数'{parent.Name}'只允许{func.Keys.Count}个参数");
                    for (int i = 0; i < func.Keys.Count; i++) {
                        if (parent.Params.Count > i) {
                            var valSource = parent.Params[i].GetMemeryUnit();
                            if (valSource.UnitType == MemeryUnits.UnitTypes.Function) valSource = ((MemeryUnits.Function)valSource).Execute();
                            args[func.Keys[i]] = valSource;
                        } else {
                            args[func.Keys[i]] = parent.Engine.MemeryPool.None;
                        }
                    }
                }
                return ((MemeryUnits.NativeFunction)fn).Execute(args);
            } else {
                // 自定义函数
                MemeryUnits.Function func = (MemeryUnits.Function)fn;
                if (parent.Params.Count >= func.Params.Count) throw new Exception($"函数'{parent.Name}'只允许{func.Params.Count - 1}个参数");
                ScriptEngine.FunctionArgs args = new ScriptEngine.FunctionArgs();
                for (int i = 0; i < func.Params.Count - 1; i++) {
                    if (parent.Params.Count > i) {
                        var valSource = parent.Params[i].GetMemeryUnit();
                        if (valSource.UnitType == MemeryUnits.UnitTypes.Function) valSource = ((MemeryUnits.Function)valSource).Execute();
                        args[((ProcessUnits.Define)func.Params[i]).Name] = valSource;
                    } else {
                        args[((ProcessUnits.Define)func.Params[i]).Name] = parent.Engine.MemeryPool.None;
                    }
                }
                return ((MemeryUnits.Function)func.Params[func.Params.Count - 1].GetMemeryUnit()).Execute(args);
            }
        }

        #endregion

        /// <summary>
        /// 函数参数
        /// </summary>
        public class FunctionArgs : egg.KeyValues<MemeryUnits.Unit> {

            /// <summary>
            /// 获取字符串内容
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public string GetString(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.String) return res.ToString();
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Number) return res.ToString();
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Boolean) return res.ToString();
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取字符串内容
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool GetBoolean(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.String) return res.ToBoolean();
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Number) return res.ToBoolean();
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Boolean) return res.ToBoolean();
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取双精度内容
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public double GetDouble(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.String) return res.ToNumber();
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Number) return res.ToNumber();
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Boolean) return res.ToNumber();
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取双精度内容
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public float GetSingle(string name) { return (float)GetDouble(name); }

            /// <summary>
            /// 获取长整型内容
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public long GetLong(string name) { return (long)GetDouble(name); }

            /// <summary>
            /// 获取整型内容
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public int GetInteger(string name) { return (int)GetDouble(name); }

            /// <summary>
            /// 获取字符串参数
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public MemeryUnits.String String(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.String) return (MemeryUnits.String)res;
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取数值参数
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public MemeryUnits.Number Number(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Number) return (MemeryUnits.Number)res;
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取布尔参数
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public MemeryUnits.Boolean Boolean(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Boolean) return (MemeryUnits.Boolean)res;
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取对象参数
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public MemeryUnits.Object Object(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Object) return (MemeryUnits.Object)res;
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取列表参数
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public MemeryUnits.List List(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.List) return (MemeryUnits.List)res;
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取对象参数
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public MemeryUnits.NativeObject NativeObject(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.NativeObject) return (MemeryUnits.NativeObject)res;
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }

            /// <summary>
            /// 获取函数参数
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public MemeryUnits.Function Function(string name) {
                var res = this[name];
                if (res.UnitType == egg.Lark.MemeryUnits.UnitTypes.Function) return (MemeryUnits.Function)res;
                throw new Exception($"函数的'{name}'参数不支持的类型'{res.UnitType.ToString()}'。");
            }
        }

        /// <summary>
        /// 函数定义接口
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public delegate MemeryUnits.Unit Function(ScriptMemeryPool pool, FunctionArgs args);

        /// <summary>
        /// 存储池
        /// </summary>
        public ScriptMemeryPool MemeryPool { get; private set; }

        /// <summary>
        /// 获取地址集合
        /// </summary>
        internal List<string> Pathes { get; private set; }

        // 主函数
        private MemeryUnits.Function main;
        private ScriptEngine.FunctionArgs vars;
        private List<ScriptEngine> libs;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public ScriptEngine(ScriptMemeryPool pool, bool isLibrary = false) {
            this.MemeryPool = pool;
            main = new MemeryUnits.Function(this, pool, null, "step");
            vars = new ScriptEngine.FunctionArgs();
            this.Pathes = new List<string>();
            if (!isLibrary) {
                libs = new List<ScriptEngine>();
            }
            Imports.Lark.Reg(this);
            Imports.String.Reg(this);
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
        /// 设置主程序变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProcessVariable(string name, MemeryUnits.Unit value) {
            main.SetVariableValue(name, value, true);
        }

        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <param name="name"></param>
        public MemeryUnits.Unit GetProcessVariable(string name) {
            // 检测主程序变量
            if (main.CheckVariable(name)) return main.GetVariableValue(name);
            if (eggs.IsNull(this.libs)) return this.MemeryPool.None;
            for (int i = 0; i < libs.Count; i++) {
                if (libs[i].CheckProcessVariable(name)) return libs[i].GetProcessVariable(name);
            }
            return this.MemeryPool.None;
        }

        /// <summary>
        /// 获取变量列表
        /// </summary>
        public egg.Strings GetProcessVariables() {
            return main.GetVariables();
        }

        /// <summary>
        /// 检测变量值
        /// </summary>
        /// <param name="name"></param>
        public bool CheckProcessVariable(string name) {
            if (main.CheckVariable(name)) return true;
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
            this.SetVariable(name, new MemeryUnits.NativeFunction(this.MemeryPool, fun, keys));
        }

        /// <summary>
        /// 添加路径
        /// </summary>
        /// <param name="path"></param>
        public void AddPath(string path) {
            path = path.Replace("\\", "/");
            if (!path.EndsWith("/")) path += "/";
            this.Pathes.Add(path);
        }

        /// <summary>
        /// 添加路径集合
        /// </summary>
        /// <param name="pathes"></param>
        public void AddPath(string[] pathes) {
            for (int i = 0; i < pathes.Length; i++) {
                AddPath(pathes[i]);
            }
        }

        // 引入文件
        internal void Include(string name) {
            // 优先加载自定义文件
            for (int i = 0; i < this.Pathes.Count; i++) {
                string path = $"{this.Pathes[i]}{name}.lark";
                if (eggs.CheckFileExists(path)) {
                    try {
                        ScriptEngine lib = new ScriptEngine(this.MemeryPool, true);
                        lib.ExecuteFile(path);
                        this.libs.Add(lib);
                    } catch (Exception ex) {
                        throw new Exception($"{System.IO.Path.GetFileName(path)}->{ex.Message}", ex);
                    }
                    return;
                }
            }
            // 加载内置类
            switch (name) {
                case "json":
                    Imports.Json.Reg(this);
                    break;
                case "io":
                    Imports.IO.Reg(this);
                    break;
                case "time":
                    Imports.Time.Reg(this);
                    break;
                case "math":
                    Imports.Math.Reg(this);
                    break;
                case "http":
                    Imports.Http.Reg(this);
                    break;
                case "hash":
                    Imports.Hash.Reg(this);
                    break;
                default:
                    throw new Exception($"未找到'{name}'库文件");
            }
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
            try {
                main.Execute(vars);
            } catch (Exception ex) {
                throw new Exception($"main->{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 执行文件
        /// </summary>
        public void ExecuteFile(string path) {
            string script = egg.File.UTF8File.ReadAllText(path);
            Parser.Parse(this, script);
#if DEBUG
            main.Execute(vars);
#else
            try {
                main.Execute(vars);
            } catch (Exception ex) {
                throw new Exception($"ERROR::main() -> {path}", ex);
            }
#endif
        }

        // 执行函数
        internal MemeryUnits.Unit ExecuteFunction(MemeryUnits.Function fn, ScriptMemeryPool pool) {
            return pool.None;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            main.Dispose();
        }
    }
}
