using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 脚本函数控制器接口
    /// </summary>
    public interface IScriptFunctionController
    {
        /// <summary>
        /// 设置脚本引擎
        /// </summary>
        /// <param name="engine"></param>
        void SetEngine(ScriptEngine engine);
    }
}
