﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 脚本引擎
    /// </summary>
    public class ScriptEngine : IDisposable
    {
        // 是否独立内存
        private readonly bool _singleMemory;
        // 函数集合
        private Dictionary<string, Func<ScriptVariables, object?>> _funcs;
        // 入口函数
        private ScriptFunction _scriptFunction;

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
            _singleMemory = true;
            this.Memory = new ScriptMemory();
        }

        /// <summary>
        /// 脚本引擎
        /// </summary>
        /// <param name="script"></param>
        /// <param name="memory"></param>
        public ScriptEngine(string script, ScriptMemory memory)
        {
            _singleMemory = false;
            this.Memory = memory;
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
            if (!_funcs.ContainsKey(name)) throw new ScriptException($"函数'{name}'尚未定义");
            return _funcs[name](variables);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        public void Execute()
        {
            _scriptFunction.Execute();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            if (_singleMemory) this.Memory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
