using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 函数
    /// </summary>
    public class ScriptFunction : IDisposable
    {

        /// <summary>
        /// 函数
        /// </summary>
        public ScriptFunction()
        {
            Parameters = new ScriptVariables();
        }

        /// <summary>
        /// 函数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数集合
        /// </summary>
        public ScriptVariables Parameters { get; }

        // 获取值
        private T GetValue<T>(ScriptEngine engine, object? obj)
        {
            // 返回变量值
            if (obj is ScriptVariable) return engine.Memory.Get<T>(((ScriptVariable)obj).Name);
            // 返回函数执行结果
            if (obj is ScriptFunction) obj = ((ScriptFunction)obj).Execute(engine);
            // 为空判断
            if (obj == null) throw new ScriptException($"变量值为空");
            // 直接返回类型
            return (T)obj;
        }

        // 获取值
        private object? GetValue(ScriptEngine engine, object? obj)
        {
            // 返回变量值
            if (obj is ScriptVariable) return engine.Memory[((ScriptVariable)obj).Name];
            // 返回函数执行结果
            if (obj is ScriptFunction) return ((ScriptFunction)obj).Execute(engine);
            // 直接返回类型
            return obj;
        }

        // 执行对象
        private void FuncInvoke(ScriptEngine engine, object? func)
        {
            // 为空跳出
            if (func is null) return;
            // 为函数则执行
            if (func is ScriptFunction)
            {
                ((ScriptFunction)func).Execute(engine);
                return;
            }
            // 抛出异常
            throw new ScriptException($"不可执行的类型'{func.GetType().FullName}'。");
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public object? Execute(ScriptEngine engine)
        {
            // 处理函数
            switch (this.Name)
            {
                case "@":
                    #region [=====依次执行=====]
                    for (int i = 0; i < this.Parameters.Count; i++)
                    {
                        FuncInvoke(engine, this.Parameters[i]);
                    }
                    #endregion
                    return null;
                case "+":
                    #region [=====加法执行=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    double dblValue = GetValue<double>(engine, this.Parameters[0]);
                    for (int i = 1; i < this.Parameters.Count; i++)
                    {
                        dblValue += GetValue<double>(engine, this.Parameters[i]);
                    }
                    #endregion
                    return dblValue;
                case "-":
                    #region [=====加法执行=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    dblValue = GetValue<double>(engine, this.Parameters[0]);
                    for (int i = 1; i < this.Parameters.Count; i++)
                    {
                        dblValue -= GetValue<double>(engine, this.Parameters[i]);
                    }
                    #endregion
                    return dblValue;
                case "*":
                    #region [=====加法执行=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    dblValue = GetValue<double>(engine, this.Parameters[0]);
                    for (int i = 1; i < this.Parameters.Count; i++)
                    {
                        dblValue *= GetValue<double>(engine, this.Parameters[i]);
                    }
                    #endregion
                    return dblValue;
                case "/":
                    #region [=====加法执行=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    dblValue = GetValue<double>(engine, this.Parameters[0]);
                    for (int i = 1; i < this.Parameters.Count; i++)
                    {
                        dblValue /= GetValue<double>(engine, this.Parameters[i]);
                    }
                    #endregion
                    return dblValue;
                case "$":
                    #region [=====加法执行=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i < this.Parameters.Count; i++)
                    {
                        var value = GetValue(engine, this.Parameters[i]);
                        if (value != null) sb.Append(value);
                    }
                    #endregion
                    return sb.ToString();
                case "let":
                    #region [=====赋值函数执行=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    var letVariable = this.Parameters[0];
                    string letVariableName;
                    if (letVariable is ScriptVariable)
                    {
                        letVariableName = ((ScriptVariable)letVariable).Name;
                    }
                    else throw new ScriptException($"函数'{this.Name}'的首参数必须是变量");
                    // 设置变量
                    engine.Memory[letVariableName] = GetValue(engine, this.Parameters[1]);
                    #endregion
                    return null;
                case "if":
                    #region [=====分支函数执行=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    var ifJudge = GetValue<bool>(engine, this.Parameters[0]);
                    if (ifJudge)
                    {
                        if (this.Parameters.Count >= 2) FuncInvoke(engine, this.Parameters[1]);
                    }
                    else
                    {
                        if (this.Parameters.Count >= 3) FuncInvoke(engine, this.Parameters[2]);
                    }
                    #endregion
                    return null;
                case "while":
                    #region [=====循环函数执行=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    while (GetValue<bool>(engine, this.Parameters[0]))
                    {
                        if (this.Parameters.Count >= 2) FuncInvoke(engine, this.Parameters[1]);
                    }
                    #endregion
                    return null;
                case "for":
                    #region [=====循环函数执行=====]
                    if (this.Parameters.Count < 4) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    var forVariable = this.Parameters[0];
                    string forVariableName;
                    if (forVariable is ScriptVariable)
                    {
                        forVariableName = ((ScriptVariable)forVariable).Name;
                        if (!engine.Memory.ContainsKey(forVariableName)) engine.Memory[forVariableName] = null;
                    }
                    else throw new ScriptException($"函数'{this.Name}'的首参数必须是变量");
                    var forStart = GetValue<double>(engine, this.Parameters[1]);
                    var forEnd = GetValue<double>(engine, this.Parameters[2]);
                    var forStep = GetValue<double>(engine, this.Parameters[3]);
                    object? forFunc = null;
                    if (this.Parameters.Count > 4) forFunc = this.Parameters[4];
                    for (dblValue = forStart; dblValue <= forEnd; dblValue += forStep)
                    {
                        // 设置循环变量
                        engine.Memory[forVariableName] = dblValue;
                        if (forFunc != null) FuncInvoke(engine, forFunc);
                    }
                    #endregion
                    return null;
                default:
                    #region [=====常规函数执行=====]
                    using (ScriptVariables args = new ScriptVariables())
                    {
                        for (int i = 0; i < this.Parameters.Count; i++)
                        {
                            args.Add(GetValue(engine, this.Parameters[i]));
                        }
                        return engine.Execute(this.Name, args);
                    }
                    #endregion
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Parameters.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
