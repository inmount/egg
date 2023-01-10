using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 函数
    /// </summary>
    public class ScriptFunction
    {
        // 脚本引擎
        private readonly ScriptEngine _scriptEngine;

        /// <summary>
        /// 函数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数集合
        /// </summary>
        public ScriptVariables Parameters { get; }

        // 获取值
        private T GetValue<T>(object? obj)
        {
            // 返回变量值
            if (obj is ScriptVariable) return _scriptEngine.Memory.Get<T>(((ScriptVariable)obj).Name);
            // 返回函数执行结果
            if (obj is ScriptFunction) obj = ((ScriptFunction)obj).Execute();
            // 为空判断
            if (obj == null) throw new ScriptException($"变量值为空");
            // 直接返回类型
            return (T)obj;
        }

        // 获取值
        private object? GetValue(object? obj)
        {
            // 返回变量值
            if (obj is ScriptVariable) return _scriptEngine.Memory[((ScriptVariable)obj).Name];
            // 返回函数执行结果
            if (obj is ScriptFunction) return ((ScriptFunction)obj).Execute();
            // 直接返回类型
            return obj;
        }

        // 执行对象
        private void FuncInvoke(object? func)
        {
            // 为空跳出
            if (func is null) return;
            // 为函数则执行
            if (func is ScriptFunction)
            {
                ((ScriptFunction)func).Execute();
                return;
            }
            // 抛出异常
            throw new ScriptException($"不可执行的类型'{func.GetType().FullName}'。");
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public object? Execute()
        {
            // 处理函数
            switch (this.Name)
            {
                case "@":
                    #region [=====依次执行=====]
                    for (int i = 0; i < this.Parameters.Count; i++)
                    {
                        FuncInvoke(this.Parameters[i]);
                    }
                    #endregion
                    return null;
                case "if":
                    #region [=====分支函数执行=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    var ifJudge = GetValue<bool>(this.Parameters[0]);
                    if (ifJudge)
                    {
                        if (this.Parameters.Count >= 2) FuncInvoke(this.Parameters[1]);
                    }
                    else
                    {
                        if (this.Parameters.Count >= 3) FuncInvoke(this.Parameters[2]);
                    }
                    #endregion
                    return null;
                case "while":
                    #region [=====循环函数执行=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    while (GetValue<bool>(this.Parameters[0]))
                    {
                        if (this.Parameters.Count >= 2) FuncInvoke(this.Parameters[1]);
                    }
                    #endregion
                    return null;
                case "for":
                    #region [=====循环函数执行=====]

                    #endregion
                    return null;
                default:
                    #region [=====常规函数执行=====]
                    using (ScriptVariables args = new ScriptVariables())
                    {
                        for (int i = 0; i < this.Parameters.Count; i++)
                        {
                            args.Add(GetValue(this.Parameters[i]));
                        }
                        return _scriptEngine.Execute(this.Name, args);
                    }
                    #endregion
            }
        }

        /// <summary>
        /// 函数
        /// </summary>
        public ScriptFunction()
        {
            Parameters = new ScriptVariables();
        }
    }
}
