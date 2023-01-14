﻿using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool isNumber(object? obj)
        {
            if (obj is null) return false;
            if (obj is byte) return true;
            if (obj is uint) return true;
            if (obj is int) return true;
            if (obj is ulong) return true;
            if (obj is long) return true;
            if (obj is float) return true;
            if (obj is double) return true;
            if (obj is decimal) return true;
            return false;
        }

        // 获取值
        private T GetValue<T>(ScriptEngine engine, object? obj)
        {
            // 返回变量值
            if (obj is ScriptVariable) return engine.Memory.Get<T>(((ScriptVariable)obj).Name);
            // 返回函数执行结果
            if (obj is ScriptFunction) obj = ((ScriptFunction)obj).Execute(engine);
            // 为空判断
            if (obj == null) throw new ScriptException($"变量值为空");
            if (typeof(T) == typeof(double)) return (T)(object)Convert.ToDouble(obj);
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
                case "!":
                    #region [=====依次执行=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    #endregion
                    return GetValue(engine, this.Parameters[0]);
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
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < this.Parameters.Count; i++)
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
                case "equal":
                    #region [=====相等判断=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    var value1 = GetValue(engine, this.Parameters[0]);
                    var value2 = GetValue(engine, this.Parameters[1]);
                    if (value1 is null)
                    {
                        if (value2 is null) return true;
                        return false;
                    }
                    if (value2 is null) return false;
                    // 对象直接相等
                    if (value1 == value2) return true;
                    // 处理数字情况
                    if (isNumber(value1))
                    {
                        if (isNumber(value2)) return Convert.ToDouble(value1) == Convert.ToDouble(value2);
                        return false;
                    }
                    if (isNumber(value2)) return false;
                    // 处理字符串情况
                    if (value1 is string)
                    {
                        if (value2 is string) return value1.ToString() == value2.ToString();
                        return false;
                    }
                    #endregion
                    return false;
                case "not":
                    #region [=====取反操作=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    bool bValue = GetValue<bool>(engine, this.Parameters[0]);
                    #endregion
                    return !bValue;
                case "compare":
                    #region [=====比较两个数=====]
                    if (this.Parameters.Count < 2) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    var dbValue1 = GetValue<double>(engine, this.Parameters[0]);
                    var dbValue2 = GetValue<double>(engine, this.Parameters[1]);
                    if (dbValue1 > dbValue2) return 1.0;
                    if (dbValue1 < dbValue2) return -1.0;
                    #endregion
                    return 0.0;
                case "and":
                    #region [=====与判断操作=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    for (int i = 0; i < this.Parameters.Count; i++)
                    {
                        if (!GetValue<bool>(engine, this.Parameters[i])) return false;
                    }
                    #endregion
                    return true;
                case "or":
                    #region [=====或判断操作=====]
                    if (this.Parameters.Count < 1) throw new ScriptException($"函数'{this.Name}'缺少必要参数");
                    for (int i = 0; i < this.Parameters.Count; i++)
                    {
                        if (GetValue<bool>(engine, this.Parameters[i])) return true;
                    }
                    #endregion
                    return false;
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
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Name != "!")
            {
                sb.Append(this.Name);
                sb.Append('(');
            }
            for (int i = 0; i < this.Parameters.Count; i++)
            {
                if (i > 0) sb.Append(',');
                var p = this.Parameters[i];
                if (p is null) { sb.Append("null"); continue; }
                if (isNumber(p)) { sb.Append(Convert.ToDouble(p)); continue; }
                if (p is string) { sb.Append('"'); sb.Append((string)p); sb.Append('"'); continue; }
                if (p is ScriptFunction) { sb.Append(((ScriptFunction)p).ToString()); continue; }
                if (p is ScriptVariable) { sb.Append(((ScriptVariable)p).Name); continue; }
                throw new ScriptException($"不支持的参数类型'{p.GetType().FullName}'。");
            }
            if (this.Name != "!") sb.Append(')');
            return sb.ToString();
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