using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Egg;

namespace Egg.Lark
{
    // 算式解析信息
    internal class ParseFormulaInfo
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 操作值
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// 脚本解析器
    /// </summary>
    public static class ScriptParser
    {
        private static ScriptFunction CreateFormulaFunction(string name, object arg1, object arg2)
        {
            ScriptFunction func = new ScriptFunction() { Name = name };
            func.Parameters.Add(arg1);
            func.Parameters.Add(arg2);
            return func;
        }

        /// <summary>
        /// 解析算式脚本
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static ScriptFunction ParseFormula(string script)
        {
            List<ParseFormulaInfo> infos = new List<ParseFormulaInfo>();
            // 计算乘法和除法
            return new ScriptFunction();
        }

        /// <summary>
        /// 解析脚本
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static ScriptFunction Parse(string script)
        {
            // 调试输出
            Debug.WriteLine($"-> 解析脚本: \n{script}");
            // 函数
            ScriptFunction? func = new ScriptFunction();
            // 临时存储
            StringBuilder sb = new StringBuilder();
            // 是否在字符串中
            bool inString = false;
            // 是否处于转义模式
            bool isEscape = false;
            // 是否在注释中
            bool inNote = false;
            // 是否结束
            bool isFinish = false;
            int line = 1;
            int site = 0;
            int sign = 0;
            try
            {
                for (int i = 0; i < script.Length; i++)
                {
                    site++;
                    char chr = script[i];
                    switch (chr)
                    {
                        case ' ':
                            #region [=====空格=====]
                            if (isEscape) throw new Exception($"意外的字符'{chr}'。");
                            if (inString) sb.Append(chr);
                            #endregion
                            break;
                        case '"':
                            #region [=====双引号：字符串=====]
                            if (inNote) break;
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            if (inString)
                            {
                                if (isEscape)
                                {
                                    isEscape = false;
                                }
                                else
                                {
                                    inString = false;
                                }
                            }
                            else
                            {
                                inString = true;
                            }
                            sb.Append(chr);
                            #endregion
                            break;
                        case '\\':
                            #region [=====反斜杠：转义=====]
                            if (inNote) break;
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            if (!inString) throw new Exception($"意外的字符'{chr}'。");
                            if (isEscape)
                            {
                                isEscape = false;
                                sb.Append(chr);
                            }
                            else
                            {
                                isEscape = true;
                            }
                            #endregion
                            break;
                        case 'r':
                            #region [=====字符r=====]
                            if (inNote) break;
                            if (isEscape) { sb.Append('\r'); isEscape = false; break; }
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            sb.Append(chr);
                            #endregion
                            break;
                        case 'n':
                            #region [=====字符r=====]
                            if (inNote) break;
                            if (isEscape) { sb.Append('\n'); isEscape = false; break; }
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            sb.Append(chr);
                            #endregion
                            break;
                        case '#':
                            #region [=====井号：注释符=====]
                            if (inNote) { inNote = false; break; }
                            if (isEscape) throw new Exception($"意外的字符'{chr}'。");
                            if (inString) { sb.Append(chr); break; }
                            if (sb.Length > 0) throw new Exception($"意外的字符'{chr}'。");
                            inNote = true;
                            #endregion
                            break;
                        case '(':
                            #region [=====左括号：函数定义=====]
                            if (inNote) break;
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            if (isEscape) throw new Exception($"意外的字符'{chr}'。");
                            if (inString) { sb.Append(chr); break; }
                            sign++;
                            if (sign > 1) { sb.Append(chr); break; }
                            if (sb.Length <= 0) throw new Exception($"函数名不允许为空。");
                            func.Name = sb.ToString();
                            sb.Clear();
                            #endregion
                            break;
                        case ')':
                            #region [=====右括号：函数结束=====]
                            if (inNote) break;
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            if (isEscape) throw new Exception($"意外的字符'{chr}'。");
                            if (inString) { sb.Append(chr); break; }
                            sign--;
                            if (sign > 0) { sb.Append(chr); break; }
                            if (sb.Length > 0)
                            {
                                // 优先处理混合运算语法糖
                                if (func.Name == "!")
                                {
                                    break;
                                }
                                // 添加字符串
                                if (sb.Length > 2)
                                    if (sb[0] == '"' && sb[sb.Length - 1] == '"')
                                    {
                                        func.Parameters.Add(sb.ToString(1, sb.Length - 2));
                                        // 清理字符缓存
                                        sb.Clear();
                                        isFinish = true;
                                        break;
                                    }
                                string arg = sb.ToString();
                                if (double.TryParse(arg, out double value))
                                {
                                    // 添加数字
                                    func.Parameters.Add(value);
                                }
                                else
                                {
                                    if (arg.IndexOf('(') >= 0)
                                    {
                                        // 添加函数
                                        func.Parameters.Add(Parse(arg));
                                    }
                                    else
                                    {
                                        // 添加变量
                                        func.Parameters.Add(new ScriptVariable() { Name = arg });
                                    }
                                }
                            }
                            // 清理字符缓存
                            sb.Clear();
                            isFinish = true;
                            #endregion
                            break;
                        case ',':
                            #region [=====右括号：函数结束=====]
                            if (inNote) break;
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            if (inString) { sb.Append(chr); break; }
                            if (sign == 0) throw new Exception($"意外的字符'{chr}'。");
                            if (sign > 1) { sb.Append(chr); break; };
                            if (sb.Length <= 0) throw new Exception($"意外的字符'{chr}'。");
                            {
                                // 添加字符串
                                if (sb.Length > 2)
                                    if (sb[0] == '"' && sb[sb.Length - 1] == '"')
                                    {
                                        func.Parameters.Add(sb.ToString(1, sb.Length - 2));
                                        // 清理字符缓存
                                        sb.Clear();
                                        break;
                                    }
                                string arg = sb.ToString();
                                if (double.TryParse(arg, out double value))
                                {
                                    // 添加数字
                                    func.Parameters.Add(value);
                                }
                                else
                                {
                                    if (arg.IndexOf('(') >= 0)
                                    {
                                        // 添加函数
                                        func.Parameters.Add(Parse(arg));
                                    }
                                    else
                                    {
                                        // 添加变量
                                        func.Parameters.Add(new ScriptVariable() { Name = arg });
                                    }
                                }
                            }
                            // 清理字符缓存
                            sb.Clear();
                            #endregion
                            break;
                        case '\r': break;
                        case '\n':
                            if (inNote) { line++; site = 0; inNote = false; break; }
                            if (sign > 0)
                            {
                                if (sb.Length > 0)
                                {
                                    // 添加字符串
                                    if (sb.Length > 2)
                                        if (sb[0] == '"' && sb[sb.Length - 1] == '"')
                                        {
                                            func.Parameters.Add(sb.ToString(1, sb.Length - 2));
                                            // 清理字符缓存
                                            sb.Clear();
                                            break;
                                        }
                                    string arg = sb.ToString();
                                    if (double.TryParse(arg, out double value))
                                    {
                                        // 添加数字
                                        func.Parameters.Add(value);
                                    }
                                    else
                                    {
                                        if (arg.IndexOf('(') >= 0)
                                        {
                                            // 添加函数
                                            func.Parameters.Add(Parse(arg));
                                        }
                                        else
                                        {
                                            // 添加变量
                                            func.Parameters.Add(new ScriptVariable() { Name = arg });
                                        }
                                    }
                                }
                                break;
                            }
                            if (sb.Length > 0) throw new Exception($"意外的换行符。");
                            break;
                        default:
                            if (inNote) break;
                            if (isFinish) throw new Exception($"意外的字符'{chr}'。");
                            if (isEscape) throw new Exception($"意外的字符'{chr}'。");
                            sb.Append(chr);
                            break;
                    }
                }
                return func;
            }
            catch (Exception ex)
            {
                throw new ScriptException($"行 {line} 位置 {site} 解析发生异常: {ex.Message}", ex.InnerException);
            }
        }
    }
}
