using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {
    /// <summary>
    /// 解析器
    /// </summary>
    public static class Parser {

        // 检查名称
        private static bool CheckName(string name) {
            if (name == "+" || name == "-" || name == "*" || name == "/" || name == "#") return true;
            for (int i = 0; i < name.Length; i++) {
                char chr = name[i];
                if (i == 0 && ((chr >= '0' && chr <= '9') || (chr == '.'))) return false;
                if (chr != '$' && chr != '_' && chr != '.' && !(chr >= '0' && chr <= '9') && !(chr >= 'a' && chr <= 'z') && !(chr >= 'A' && chr <= 'Z')) return false;
            }
            return true;
        }

        /// <summary>
        /// 执行解析
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="script"></param>
        public static void Parse(ScriptEngine engine, string script) {
            MemeryUnits.Function fn = null;
            StringBuilder sb = new StringBuilder();
            bool inString = false;
            bool isEscape = false;
            bool inNote = false;
            int line = 1;
            int site = 0;
            try {
                for (int i = 0; i < script.Length; i++) {
                    site++;
                    char chr = script[i];
                    switch (chr) {
                        case '"':
                            #region [=====引号=====]
                            // 处理注释
                            if (inNote) {
                                sb.Append(chr);
                                break;
                            }
                            if (inString) {
                                if (isEscape) {
                                    sb.Append(chr);
                                    isEscape = false;
                                } else {
                                    sb.Append(chr);
                                    inString = false;
                                }
                            } else {
                                if (sb.Length > 0) throw new Exception("规则外的引号");
                                sb.Append(chr);
                                inString = true;
                            }
                            #endregion
                            break;
                        case '\\':
                            #region [=====反斜杠=====]
                            if (!inString) throw new Exception($"规则外的'{chr}'字符");
                            // 处理注释
                            if (inNote) {
                                sb.Append(chr);
                                break;
                            }
                            if (isEscape) {
                                sb.Append(chr);
                                isEscape = false;
                            } else {
                                isEscape = true;
                            }
                            #endregion
                            break;
                        case '(':
                            #region [=====左括号=====]
                            if (inString || inNote) {
                                sb.Append(chr);
                            } else {
                                // 增加函数
                                string name = sb.ToString();
                                if (!CheckName(name)) throw new Exception("名称不合法");
                                if (eggs.Object.IsNull(fn)) {
                                    fn = engine.AddFunction(name);
                                } else {
                                    fn = (MemeryUnits.Function)fn.Params.AddFunction(name).GetMemeryUnit();
                                }
                                sb.Clear();
                            }
                            #endregion
                            break;
                        case ')':
                            #region [=====右括号=====]
                            if (inString || inNote) {
                                sb.Append(chr);
                            } else {
                                if (eggs.Object.IsNull(fn)) throw new Exception($"多余的'{chr}'字符");
                                // 添加参数
                                if (sb.Length > 0) {
                                    string name = sb.ToString();
                                    if (name.StartsWith("\"")) {
                                        fn.Params.AddString(name.Substring(1, name.Length - 2));
                                    } else if (name.IsDouble()) {
                                        fn.Params.AddNumber(name.ToDouble());
                                    } else {
                                        if (!CheckName(name)) throw new Exception("名称不合法");
                                        fn.Params.AddDefine(name);
                                    }
                                    sb.Clear();
                                }
                                fn = fn.ParentFunction;
                            }
                            #endregion
                            break;
                        case ',':
                            #region [=====逗号=====]
                            if (inString || inNote) {
                                sb.Append(chr);
                            } else {
                                if (sb.Length > 0) {
                                    if (eggs.Object.IsNull(fn)) throw new Exception($"语法错误，顶层代码只允许函数");
                                    string name = sb.ToString();
                                    if (name.StartsWith("\"")) {
                                        fn.Params.AddString(name.Substring(1, name.Length - 2));
                                    } else if (name.IsDouble()) {
                                        fn.Params.AddNumber(name.ToDouble());
                                    } else {
                                        if (!CheckName(name)) throw new Exception("名称不合法");
                                        fn.Params.AddDefine(name);
                                    }
                                    sb.Clear();
                                } else {
                                    if (fn.Params.Count == 0) throw new Exception($"多余的'{chr}'字符");
                                    if (fn.Params[fn.Params.Count - 1].GetMemeryUnit().UnitType != MemeryUnits.UnitTypes.Function) throw new Exception($"多余的'{chr}'字符");
                                }
                            }
                            #endregion
                            break;
                        case 'r':
                            #region [=====字母R=====]
                            if (isEscape) {
                                sb.Append('\r');
                                isEscape = false;
                            } else {
                                sb.Append(chr);
                            }
                            #endregion
                            break;
                        case 'n':
                            #region [=====字母R=====]
                            if (isEscape) {
                                sb.Append('\n');
                                isEscape = false;
                            } else {
                                sb.Append(chr);
                            }
                            #endregion
                            break;
                        case '#':
                            #region [=====注释=====]
                            if (inString) {
                                sb.Append(chr);
                            } else {
                                // 处理头尾闭合的注释
                                if (inNote) {
                                    sb.Clear();
                                    inNote = false;
                                    break;
                                } else {
                                    if (sb.Length > 0) throw new Exception($"意外的'\\{chr}'操作符");
                                    inNote = true;
                                }
                            }
                            #endregion
                            break;
                        case '\r':
                        case '\n':
                            #region [=====换行回车=====]
                            if (inString) throw new Exception("字符串不可换行");
                            if (chr == '\n') {
                                line++;
                                site = 0;
                            }
                            #endregion
                            break;
                        case ' ':
                            #region [=====空格=====]
                            if (inString) sb.Append(chr);
                            #endregion
                            break;
                        default:
                            if (isEscape) throw new Exception($"不支持的'\\{chr}'转义");
                            if (inString || inNote) {
                                sb.Append(chr);
                            } else {
                                if (sb.Length <= 0) {
                                    sb.Append(chr);
                                } else if (sb[0] != '"') {
                                    sb.Append(chr);
                                } else {
                                    throw new Exception("字符串已结束");
                                }
                            }
                            break;
                    }
                }
                if (sb.Length > 0) throw new Exception("脚本尚未结束");
            } catch (Exception ex) {
                if (ex.InnerException == null) {
                    throw new Exception($"脚本在解析[行{line}位置{site}]时发生异常:{ex.Message}");
                } else {
                    throw new Exception($"脚本在解析[行{line}位置{site}]时发生异常:{ex.Message}", ex.InnerException);
                }
            }
        }

    }
}
