using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 函数集合
    /// </summary>
    public class ScriptFunctions : Dictionary<string, Func<ScriptVariables, object?>>, IDisposable
    {
        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public ScriptFunctions Reg(string name, Func<ScriptVariables, object> func)
        {
            this[name] = func;
            return this;
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public ScriptFunctions Reg(string name, Action<ScriptVariables> action)
        {
            this[name] = new Func<ScriptVariables, object?>((args) => { action(args); return null; });
            return this;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
