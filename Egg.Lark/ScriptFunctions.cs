using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 标准函数集合
    /// </summary>
    public class ScriptFunctions : ScriptFunctionsBase
    {

        /// <summary>
        /// 标准函数集合
        /// </summary>
        public ScriptFunctions()
        {
            // 依次执行
            base.Reg("@", args =>
            {
                for (int i = 0; i < args.Count; i++)
                {
                    FuncInvoke(this.Engine, args[i]);
                }
            });
            // 依次执行
            base.Reg("step", args =>
            {
                for (int i = 0; i < args.Count; i++)
                {
                    FuncInvoke(this.Engine, args[i]);
                }
            });
            // 返回
            base.Reg("return", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'return'缺少必要参数");
                for (int i = 0; i < args.Count - 1; i++)
                {
                    FuncInvoke(this.Engine, args[i]);
                }
                return GetValue(this.Engine, args[args.Count - 1]);
            });
            // 计算
            base.Reg("!", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'calculate/!'缺少必要参数");
                return GetValue(this.Engine, args[0]);
            });
            // 计算
            base.Reg("calculate", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'calculate/!'缺少必要参数");
                return GetValue(this.Engine, args[0]);
            });
            // 字符串
            base.Reg("$", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'string/$'缺少必要参数");
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Count; i++)
                {
                    var value = GetValue(this.Engine, args[i]);
                    if (value != null) sb.Append(value);
                }
                return sb.ToString();
            });
            // 字符串
            base.Reg("string", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'string/$'缺少必要参数");
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Count; i++)
                {
                    var value = GetValue(this.Engine, args[i]);
                    if (value != null) sb.Append(value);
                }
                return sb.ToString();
            });
            // 设置
            base.Reg("let", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'let'缺少必要参数");
                var letVariable = args[0];
                string letVariableName;
                if (letVariable is ScriptVariable)
                {
                    letVariableName = ((ScriptVariable)letVariable).Name;
                }
                else throw new ScriptException($"函数'let'的首参数必须是变量");
                // 设置变量
                this.Engine.Memory.Set(letVariableName, GetValue(this.Engine, args[1]));
            });
            // 判断语句
            base.Reg("if", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'if'缺少必要参数");
                var ifJudge = GetValue<bool>(this.Engine, args[0]);
                if (ifJudge)
                {
                    if (args.Count >= 2) FuncInvoke(this.Engine, args[1]);
                }
                else
                {
                    if (args.Count >= 3) FuncInvoke(this.Engine, args[2]);
                }
            });
            // 判断循环
            base.Reg("while", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'while'缺少必要参数");
                if (args.Count < 2) return;
                while (GetValue<bool>(this.Engine, args[0]))
                {
                    FuncInvoke(this.Engine, args[1]);
                }
            });
            // 规律循环
            base.Reg("for", args =>
            {
                if (args.Count < 4) throw new ScriptException($"函数'for'缺少必要参数");
                var forVariable = args[0];
                string forVariableName;
                if (forVariable is ScriptVariable)
                {
                    forVariableName = ((ScriptVariable)forVariable).Name;
                    if (!this.Engine.Memory.ContainsKey(forVariableName)) this.Engine.Memory.Set(forVariableName, null);
                }
                else throw new ScriptException($"函数'for'的首参数必须是变量");
                var forStart = GetValue<double>(this.Engine, args[1]);
                var forEnd = GetValue<double>(this.Engine, args[2]);
                var forStep = GetValue<double>(this.Engine, args[3]);
                object? forFunc = null;
                if (args.Count > 4) forFunc = args[4];
                for (double dblValue = forStart; dblValue <= forEnd; dblValue += forStep)
                {
                    // 设置循环变量
                    this.Engine.Memory[forVariableName] = dblValue;
                    if (forFunc != null) FuncInvoke(this.Engine, forFunc);
                }
            });
            // 依次循环
            base.Reg("foreach", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'foreach'缺少必要参数");
                var forVariable = args[0];
                string forVariableName;
                if (forVariable is ScriptVariable)
                {
                    forVariableName = ((ScriptVariable)forVariable).Name;
                    if (!this.Engine.Memory.ContainsKey(forVariableName)) this.Engine.Memory.Set(forVariableName, null);
                }
                else throw new ScriptException($"函数'foreach'的首参数必须是变量");
                var forList = GetValue(this.Engine, args[1]);
                if (forList is null) throw new ScriptException($"函数'foreach'缺少列表对象");
                var listType = forList.GetType();
                string listFullName = listType.Namespace + "." + listType.Name;
                object? forFunc = null;
                if (args.Count > 2) forFunc = args[2];
                if (ScriptMemory.IsList(listType)) // 处理列表
                {
                    var listItem = listType.GetProperty("Item");
                    var listCount = listType.GetProperty("Count");
                    int listCountValue = (int)listCount.GetValue(forList);
                    for (var i = 0; i < listCountValue; i++)
                    {
                        // 设置循环变量
                        this.Engine.Memory.Set(forVariableName, listItem.GetValue(forList, new object[] { i }));
                        if (forFunc != null) FuncInvoke(this.Engine, forFunc);
                    }
                }
                else if (ScriptMemory.IsDictionary(listType)) // 处理字典
                {
                    var objTypeEnumerator = listType.GetMethod("GetEnumerator");
                    IDictionaryEnumerator objEnumerator = (IDictionaryEnumerator)objTypeEnumerator.Invoke(forList, new object[] { });
                    while (objEnumerator.MoveNext())
                    {
                        // 设置循环变量
                        this.Engine.Memory.Set(forVariableName, objEnumerator.Current);
                        if (forFunc != null) FuncInvoke(this.Engine, forFunc);
                    }
                }
                else throw new ScriptException($"函数'foreach'缺少可列表的对象");
            });
            // 判断相等
            base.Reg("equal", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'equal'缺少必要参数");
                var value1 = GetValue(this.Engine, args[0]);
                var value2 = GetValue(this.Engine, args[1]);
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
                return false;
            });
            // 取反
            base.Reg("not", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'not'缺少必要参数");
                bool bValue = GetValue<bool>(this.Engine, args[0]);
                return !bValue;
            });
            // 比较
            base.Reg("compare", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'compare'缺少必要参数");
                var dbValue1 = GetValue<double>(this.Engine, args[0]);
                var dbValue2 = GetValue<double>(this.Engine, args[1]);
                if (dbValue1 > dbValue2) return 1;
                if (dbValue1 < dbValue2) return -1;
                return 0;
            });
            // 同时成立
            base.Reg("and", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'and'缺少必要参数");
                for (int i = 0; i < args.Count; i++)
                {
                    if (!GetValue<bool>(this.Engine, args[i])) return false;
                }
                return true;
            });
            // 单一成立
            base.Reg("or", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'or'缺少必要参数");
                for (int i = 0; i < args.Count; i++)
                {
                    if (GetValue<bool>(this.Engine, args[i])) return true;
                }
                return false;
            });
        }
    }
}
