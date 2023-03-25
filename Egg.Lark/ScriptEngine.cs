using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 脚本引擎
    /// </summary>
    public class ScriptEngine : IDisposable
    {
        // 默认的最大执行次数
        public const long MAX_EXECUTION_DEFAULT = 1000000;
        private long _maxExecutionCount = MAX_EXECUTION_DEFAULT;
        private long _executionCount = 0;
        // 是否独立内存
        private readonly bool _singleMemory;
        private readonly bool _singleFunction;
        // 函数集合
        private Dictionary<string, Func<ScriptVariables, object?>> _funcs;
        // 入口函数
        private ScriptFunction _scriptFunction;

        /// <summary>
        /// 设置最大执行次数
        /// </summary>
        /// <param name="value"></param>
        public void SetMaxExecution(long value)
            => _maxExecutionCount = value;

        /// <summary>
        /// 更新执行次数
        /// </summary>
        public void UpdateExecutionCount()
        {
            _executionCount++;
            if (_executionCount > _maxExecutionCount) throw new ScriptException("超出最大执行次数限制");
        }

        /// <summary>
        /// 内存管理器
        /// </summary>
        public ScriptMemory Memory { get; }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script">脚本内容</param>
        public ScriptEngine(string script)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = true;
            _singleFunction = true;
            this.Memory = new ScriptMemory();
            _scriptFunction = ScriptParser.Parse(script);
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script"></param>
        /// <param name="funcs"></param>
        public ScriptEngine(string script, ScriptFunctionsBase funcs)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = true;
            _singleFunction = true;
            this.Memory = new ScriptMemory();
            _scriptFunction = ScriptParser.Parse(script);
            funcs.SetEngine(this);
            foreach (var func in funcs) _funcs[func.Key] = func.Value;
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script"></param>
        /// <param name="memory"></param>
        public ScriptEngine(string script, ScriptMemory memory)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = false;
            _singleFunction = true;
            this.Memory = memory;
            _scriptFunction = ScriptParser.Parse(script);
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script"></param>
        /// <param name="funcs"></param>
        /// <param name="memory"></param>
        public ScriptEngine(string script, ScriptFunctionsBase funcs, ScriptMemory memory)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = false;
            _singleFunction = true;
            this.Memory = memory;
            _scriptFunction = ScriptParser.Parse(script);
            funcs.SetEngine(this);
            foreach (var func in funcs) _funcs[func.Key] = func.Value;
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="scriptFunction">主函数</param>
        public ScriptEngine(ScriptFunction scriptFunction)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = true;
            _singleFunction = false;
            this.Memory = new ScriptMemory();
            _scriptFunction = scriptFunction;
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script"></param>
        /// <param name="funcs"></param>
        public ScriptEngine(ScriptFunction scriptFunction, ScriptFunctionsBase funcs)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = true;
            _singleFunction = false;
            this.Memory = new ScriptMemory();
            _scriptFunction = scriptFunction;
            funcs.SetEngine(this);
            foreach (var func in funcs) _funcs[func.Key] = func.Value;
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="func"></param>
        /// <param name="memory"></param>
        public ScriptEngine(ScriptFunction scriptFunction, ScriptMemory memory)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = false;
            _singleFunction = false;
            this.Memory = memory;
            _scriptFunction = scriptFunction;
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script"></param>
        /// <param name="funcs"></param>
        /// <param name="memory"></param>
        public ScriptEngine(ScriptFunction scriptFunction, ScriptFunctionsBase funcs, ScriptMemory memory)
        {
            _funcs = new Dictionary<string, Func<ScriptVariables, object?>>();
            _singleMemory = false;
            _singleFunction = false;
            this.Memory = memory;
            _scriptFunction = scriptFunction;
            funcs.SetEngine(this);
            foreach (var func in funcs) _funcs[func.Key] = func.Value;
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public ScriptEngine Reg(string name, Func<ScriptVariables, object> func)
        {
            _funcs[name] = func;
            return this;
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public ScriptEngine Reg(string name, Action<ScriptVariables> action)
        {
            _funcs[name] = new Func<ScriptVariables, object?>((args) => { action(args); return null; });
            return this;
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        /// <exception cref="ScriptException"></exception>
        public object? Execute(string name, ScriptVariables variables)
        {
            // 更新执行次数
            this.UpdateExecutionCount();
            if (!_funcs.ContainsKey(name))
                throw new ScriptException($"函数'{name}'尚未定义");
            return _funcs[name](variables);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        public object? Execute()
        {
            // 初始化执行次数
            _executionCount = 0;
            return _scriptFunction.Execute(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            if (_singleMemory) this.Memory.Dispose();
            if (_singleFunction) _scriptFunction.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
