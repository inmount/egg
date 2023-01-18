using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 函数控制器
    /// </summary>
    public abstract class ScriptFunctionController : IScriptFunctionController
    {
        /// <summary>
        /// 获取关联引擎
        /// </summary>
        public ScriptEngine Engine { get; private set; }

        /// <summary>
        /// 设置脚本引擎
        /// </summary>
        /// <param name="engine"></param>
        public void SetEngine(ScriptEngine engine)
        {
            this.Engine = engine;
        }
    }
}
