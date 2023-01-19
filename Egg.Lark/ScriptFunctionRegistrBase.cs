using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 脚本函数注册器基础类
    /// </summary>
    public abstract class ScriptFunctionRegistrBase : IScriptFunctionRegistr
    {
        /// <summary>
        /// 设置脚本引擎
        /// </summary>
        /// <param name="engine"></param>
        public void SetEngine(ScriptEngine engine)
        {
            this.Engine = engine;
        }

        /// <summary>
        /// 获取关联引擎
        /// </summary>
        public ScriptEngine Engine { get; private set; }
    }
}
