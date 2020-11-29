using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// Json解析器
    /// </summary>
    public static class Parser {

        // 空操作，等待定义操作符
        private const int Parse_Null = 0x00;

        // 解析名称
        private const int Parse_Name = 0x11;

        // 解析值
        private const int Parse_Value = 0x12;

        // 操作完成标志
        private const int Parse_Done = 0x20;

        // 对象操作完成
        private const int Parse_Done_Object = 0x21;

        // 数组操作完成
        private const int Parse_Done_Array = 0x22;

        // 名称操作完成
        private const int Parse_Done_Name = 0x23;

        // 值操作完成
        private const int Parse_Done_Value = 0x24;

        /// <summary>
        /// 是否强制使用Unicode进行非Ascii字符编码
        /// </summary>
        public static bool EnforceUnicode = false;

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JsonUnit ParseJson(string json) {

            // 初始化操作对象为空
            JsonUnit res = null;
            JsonUnit jup = null;

            // 初始化字符串
            string name = null;
            string value = null;
            StringBuilder sb = new StringBuilder();

            // 初始化字符串和转义标志
            bool isString = false;
            bool isEscape = false;

            // unicode支持
            bool isUnicode = false;
            int uLen = 0;
            int uCode = 0;

            // 初始化操作类型为空操作
            int pt = Parse_Null;

            // 行列计数器
            int line = 1;
            int col = 0;

            //try {

            //遍历字符串进行解析
            for (int i = 0; i < json.Length; i++) {
                col++;
                char chr = json[i];
                switch (chr) {
                    case '{':
                        #region [=====处理左大括号=====]
                        if (isString) {
                            // 此字符不支持转义及Unicode
                            if (isEscape) throw new Exception($"规则外的\"{chr}\"操作符");
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，直接添加到字符串生成器
                            sb.Append(chr);
                        } else {
                            // 判断解析类型，必须为空
                            if (pt == Parse_Null) {
                                // 当处于字符串模式时，则为添加一个对象
                                if (jup == null) {
                                    // 无操作对象时，为顶层操作对象，不允许有名称和值
                                    if (!(name.IsNull() || value.IsNull())) throw new Exception($"规则外的\"{chr}\"操作符");
                                    res = new JsonObject();
                                    jup = res;
                                } else {
                                    throw new Exception($"规则外的\"{chr}\"操作符");
                                }
                            } else if (pt == Parse_Value) {
                                // 父对象不允许为空
                                if (jup == null) throw new Exception($"规则外的\"{chr}\"操作符");
                                // 设置子对象
                                if (jup.UnitType == UnitType.Object) {
                                    if (name.IsNoneOrNull()) throw new Exception($"语法错误：名称不允许未空");
                                    jup = jup.Object(name);
                                } else if (jup.UnitType == UnitType.Array) {
                                    if (!name.IsNoneOrNull()) throw new Exception($"语法错误：无效名称");
                                    jup = jup.Object(jup.Count);
                                } else {
                                    throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                }
                                // 清理缓存
                                name = null;
                            } else {
                                throw new Exception($"规则外的\"{chr}\"操作符");
                            }
                            // 解析类型设置为值
                            pt = Parse_Name;
                        }
                        break;
                    #endregion
                    case '}':
                        #region [=====处理右大括号=====]
                        if (isString) {
                            // 此字符不支持转义及Unicode
                            if (isEscape) throw new Exception($"规则外的\"{chr}\"操作符");
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，直接添加到字符串生成器
                            sb.Append(chr);
                        } else {

                            // 特殊处理，如果当前为值解析，同时值为数值或null，则进行值设置操作
                            if (pt == Parse_Name) {

                                // 判断是否已经定义名称
                                if (sb.Length > 0) throw new Exception($"规则外的\"{chr}\"操作符");

                                // 清理缓存
                                sb.Clear();
                                name = null;
                                value = null;
                            } else if (pt == Parse_Value) {

                                // 父对象不允许为空
                                if (jup == null) throw new Exception($"规则外的\"{chr}\"操作符");

                                if (name.IsNoneOrNull()) throw new Exception($"规则外的\"{chr}\"操作符");
                                value = sb.ToString();
                                if (value == "null" || value == "Null" || value == "NULL") {
                                    // 添加NULL值
                                    jup.String(name, null);
                                } else if (value == "true" || value == "True" || value == "TRUE") {
                                    // 添加布尔值
                                    jup.Bool(name, true);
                                } else if (value == "false" || value == "False" || value == "FALSE") {
                                    // 添加布尔值
                                    jup.Bool(name, false);
                                } else if (value.IsDouble()) {
                                    // 添加数值
                                    jup.Number(name, value.ToDouble());
                                } else {
                                    throw new Exception($"规则外的\"{chr}\"操作符");
                                }

                                // 清理缓存
                                sb.Clear();
                                name = null;
                                value = null;
                            } else if (pt == Parse_Done_Value) {
                                // 添加字符串值
                                if (name.IsNoneOrNull()) throw new Exception($"语法错误：缺少名称");
                                jup.String(name, value);
                                // 清理缓存
                                sb.Clear();
                                name = null;
                                value = null;
                            } else if ((pt & Parse_Done) == Parse_Done) {
                                // 完成状态时继续
                            } else {
                                throw new Exception($"规则外的\"{chr}\"操作符");
                            }

                            // 当前必须要有操作对象
                            if (jup == null) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 返回到上层对象
                            jup = jup.Parent;
                            // 解析类型设置为子对象操作完成
                            pt = Parse_Done_Object;
                        }
                        break;
                    #endregion
                    case '[':
                        #region [=====处理左中括号=====]
                        if (isString) {
                            // 此字符不支持转义及Unicode
                            if (isEscape) throw new Exception($"规则外的\"{chr}\"操作符");
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，直接添加到字符串生成器
                            sb.Append(chr);
                        } else {
                            // 判断解析类型，必须为空
                            if (pt == Parse_Null) {
                                // 当处于字符串模式时，则为添加一个对象
                                if (jup == null) {
                                    // 无操作对象时，为顶层操作对象，不允许有名称和值
                                    if (!(name.IsNull() || value.IsNull())) throw new Exception($"规则外的\"{chr}\"操作符");
                                    res = new JsonArray();
                                    jup = res;
                                } else {
                                    throw new Exception($"规则外的\"{chr}\"操作符");
                                }
                            } else if (pt == Parse_Value) {
                                // 父对象不允许为空
                                if (jup == null) throw new Exception($"规则外的\"{chr}\"操作符");
                                // 设置子对象
                                if (jup.UnitType == UnitType.Object) {
                                    if (name.IsNoneOrNull()) throw new Exception($"语法错误：名称不允许未空");
                                    jup = jup.Array(name);
                                } else if (jup.UnitType == UnitType.Array) {
                                    if (!name.IsNoneOrNull()) throw new Exception($"语法错误：无效名称");
                                    jup = jup.Array(jup.Count);
                                } else {
                                    throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                }
                                // 清理缓存
                                name = null;
                            } else {
                                throw new Exception($"规则外的\"{chr}\"操作符");
                            }
                            // 解析类型设置为值
                            pt = Parse_Value;
                        }
                        break;
                    #endregion
                    case ']':
                        #region [=====处理右中括号=====]
                        if (isString) {
                            // 此字符不支持转义及Unicode
                            if (isEscape) throw new Exception($"规则外的\"{chr}\"操作符");
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，直接添加到字符串生成器
                            sb.Append(chr);
                        } else {

                            // 特殊处理，如果当前为值解析，同时值为数值或null，则进行值设置操作
                            if (pt == Parse_Value) {

                                // 父对象不允许为空
                                if (jup == null) throw new Exception($"规则外的\"{chr}\"操作符");

                                if (!name.IsNoneOrNull()) throw new Exception($"语法错误：无效名称");
                                value = sb.ToString();
                                if (value == "") {
                                    // 空数组，不做处理
                                } else if (value == "null" || value == "Null" || value == "NULL") {
                                    // 添加NULL值
                                    jup.String(jup.Count, null);
                                } else if (value == "true" || value == "True" || value == "TRUE") {
                                    // 添加布尔值
                                    jup.Bool(jup.Count, true);
                                } else if (value == "false" || value == "False" || value == "FALSE") {
                                    // 添加布尔值
                                    jup.Bool(jup.Count, false);
                                } else if (value.IsDouble()) {
                                    // 添加数值
                                    jup.Number(jup.Count, value.ToDouble());
                                } else {
                                    throw new Exception($"规则外的\"{chr}\"操作符");
                                }

                                // 清理缓存
                                sb.Clear();
                                name = null;
                                value = null;
                            } else if (pt == Parse_Done_Value) {
                                // 添加字符串值
                                if (!name.IsNoneOrNull()) throw new Exception($"语法错误：无效名称");
                                jup.String(jup.Count, value);
                                // 清理缓存
                                sb.Clear();
                                name = null;
                                value = null;
                            } else if ((pt & Parse_Done) == Parse_Done) {
                                // 完成状态时继续
                            } else {
                                throw new Exception($"规则外的\"{chr}\"操作符");
                            }

                            // 当前必须要有操作对象
                            if (jup == null) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 返回到上层对象
                            jup = jup.Parent;
                            // 解析类型设置为子对象操作完成
                            pt = Parse_Done_Array;
                        }
                        break;
                    #endregion
                    case '\\':
                        #region [=====处理转义符=====]
                        if (isString) {
                            // 此字符不支持Unicode
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，操作转义
                            if (isEscape) {
                                // 当处于转义模式，则直接添加字符并退出转义
                                sb.Append(chr);
                                isEscape = false;
                            } else {
                                // 当未处于转义模式，则进入转义模式
                                isEscape = true;
                            }
                        } else {
                            throw new Exception($"规则外的\"{chr}\"操作符");
                        }
                        break;
                    #endregion
                    case '"':
                        #region [=====处理双引号=====]
                        if (isString) {
                            // 此字符不支持Unicode
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，操作转义
                            if (isEscape) {
                                // 当处于转义模式，则直接添加字符并退出转义
                                sb.Append(chr);
                                isEscape = false;
                            } else {
                                // 当未处于转义模式，则表示字符串结束
                                if (pt == Parse_Name) {
                                    // 名称模式，则设置名称
                                    name = sb.ToString();
                                    sb.Clear();
                                    pt = Parse_Done_Name;
                                } else if (pt == Parse_Value) {
                                    // 值模式，则设置值
                                    value = sb.ToString();
                                    sb.Clear();
                                    pt = Parse_Done_Value;
                                } else {
                                    throw new Exception($"规则外的\"{chr}\"操作符");
                                }
                                // 退出字符串模式
                                isString = false;
                            }
                        } else {
                            // 当未处于字符串模式时，则进入字符串模式
                            if (pt == Parse_Name) {
                                if (!name.IsNoneOrNull()) throw new Exception($"规则外的\"{chr}\"操作符");
                                isString = true;
                            } else if (pt == Parse_Value) {
                                if (!value.IsNoneOrNull()) throw new Exception($"规则外的\"{chr}\"操作符");
                                isString = true;
                            } else {
                                throw new Exception($"规则外的\"{chr}\"操作符");
                            }
                        }
                        break;
                    #endregion
                    case ':':
                        #region [=====处理冒号=====]
                        if (isString) {
                            // 此字符不支持转义及Unicode
                            if (isEscape) throw new Exception($"规则外的\"{chr}\"操作符");
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，直接添加到字符串生成器
                            sb.Append(chr);
                        } else {
                            // 特殊处理，如果当前为名称解析时，完成名称解析
                            if (pt == Parse_Name) {
                                // 当名称模式时，可作为隐藏结束符使用
                                name = sb.ToString();
                                if (name.IsNoneOrNull()) throw new Exception($"语法错误，名称不允许未空");
                                sb.Clear();
                                // 设置为值模式
                                pt = Parse_Value;
                            } else if (pt == Parse_Done_Name) {
                                // 设置为值模式
                                pt = Parse_Value;
                            } else {
                                throw new Exception($"规则外的\"{chr}\"操作符");
                            }
                        }
                        break;
                    #endregion
                    case ',':
                        #region [=====处理逗号=====]
                        if (isString) {
                            // 此字符不支持转义及Unicode
                            if (isEscape) throw new Exception($"规则外的\"{chr}\"操作符");
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"操作符");
                            // 当处于字符串模式时，直接添加到字符串生成器
                            sb.Append(chr);
                        } else {

                            // 父对象不允许为空
                            if (jup == null) throw new Exception($"规则外的\"{chr}\"操作符");

                            // 特殊处理，如果当前为值解析时，完成数值解析
                            if (pt == Parse_Value || pt == Parse_Done_Value) {

                                // 当为内容时，进行内容添加
                                if (pt == Parse_Value) {
                                    // 当为值模式时，可作为隐藏结束符使用
                                    value = sb.ToString();

                                    if (value == "null" || value == "Null" || value == "NULL") {
                                        // 添加字符串值
                                        if (jup.UnitType == UnitType.Object) {
                                            jup.String(name, null);
                                        } else if (jup.UnitType == UnitType.Array) {
                                            jup.String(jup.Count, null);
                                        } else {
                                            throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                        }
                                    } else if (value == "true" || value == "True" || value == "TRUE") {
                                        // 添加布尔值
                                        if (jup.UnitType == UnitType.Object) {
                                            jup.Bool(name, true);
                                        } else if (jup.UnitType == UnitType.Array) {
                                            jup.Bool(jup.Count, true);
                                        } else {
                                            throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                        }
                                    } else if (value == "false" || value == "False" || value == "FALSE") {
                                        // 添加布尔值
                                        if (jup.UnitType == UnitType.Object) {
                                            jup.Bool(name, false);
                                        } else if (jup.UnitType == UnitType.Array) {
                                            jup.Bool(jup.Count, false);
                                        } else {
                                            throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                        }
                                    } else if (value.IsDouble()) {
                                        // 添加数值
                                        double dbl = value.ToDouble();
                                        if (jup.UnitType == UnitType.Object) {
                                            jup.Number(name, dbl);
                                        } else if (jup.UnitType == UnitType.Array) {
                                            jup.Number(jup.Count, dbl);
                                        } else {
                                            throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                        }
                                    } else {
                                        throw new Exception($"语法错误：字符串必须使用双引号定义");
                                    }

                                    // 清理缓存
                                    sb.Clear();
                                    name = null;
                                    value = null;

                                } else {
                                    // 添加字符串值
                                    if (jup.UnitType == UnitType.Object) {
                                        jup.String(name, value);
                                    } else if (jup.UnitType == UnitType.Array) {
                                        jup.String(jup.Count, value);
                                    } else {
                                        throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                    }

                                    // 清理缓存
                                    sb.Clear();
                                    name = null;
                                    value = null;
                                }

                            } else if (pt == Parse_Done_Object || pt == Parse_Done_Array) {
                                // 当为对象或数组时，则进入下一个对象的操作
                            } else {
                                throw new Exception($"规则外的\"{chr}\"操作符");
                            }

                            // 判断父对象类型
                            if (jup.UnitType == UnitType.Object) {
                                // 当父对象是对象时，设置子对象，进入名称模式
                                pt = Parse_Name;
                            } else if (jup.UnitType == UnitType.Array) {
                                // 当父对象是对象时，进入值模式
                                pt = Parse_Value;
                            } else {
                                throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                            }

                        }
                        break;
                    #endregion
                    case ' ':
                        #region [=====处理空格=====]
                        if (isString) {
                            // 此字符不支持转义
                            if (isEscape) throw new Exception($"规则外的\"{chr}\"字符");
                            // 当处于字符串模式时，直接添加到字符串生成器
                            sb.Append(chr);
                        } else {
                            if (pt == Parse_Name) {
                                // 判断是否为多余的空格
                                if (sb.Length <= 0) break;
                                // 当名称模式时，可作为隐藏结束符使用
                                name = sb.ToString();
                                sb.Clear();
                                pt = Parse_Done_Name;
                            } else if (pt == Parse_Value) {
                                // 判断是否为多余的空格
                                if (sb.Length <= 0) break;
                                // 当名称模式时，可作为隐藏结束符使用
                                value = sb.ToString();

                                if (value == "null" || value == "Null" || value == "NULL") {
                                    // 添加字符串值
                                    if (jup.UnitType == UnitType.Object) {
                                        if (name.IsNoneOrNull()) throw new Exception($"语法错误：名称不允许未空");
                                        jup.String(name, null);
                                    } else if (jup.UnitType == UnitType.Array) {
                                        if (!name.IsNoneOrNull()) throw new Exception($"语法错误：无效名称");
                                        jup.String(jup.Count, null);
                                    } else {
                                        throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                    }
                                } else if (value == "true" || value == "True" || value == "TRUE") {
                                    // 添加布尔值
                                    if (jup.UnitType == UnitType.Object) {
                                        jup.Bool(name, true);
                                    } else if (jup.UnitType == UnitType.Array) {
                                        jup.Bool(jup.Count, true);
                                    } else {
                                        throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                    }
                                } else if (value == "false" || value == "False" || value == "FALSE") {
                                    // 添加布尔值
                                    if (jup.UnitType == UnitType.Object) {
                                        jup.Bool(name, false);
                                    } else if (jup.UnitType == UnitType.Array) {
                                        jup.Bool(jup.Count, false);
                                    } else {
                                        throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                    }
                                } else if (value.IsDouble()) {
                                    // 添加数值
                                    double dbl = value.ToDouble();
                                    if (jup.UnitType == UnitType.Object) {
                                        if (name.IsNoneOrNull()) throw new Exception($"语法错误：名称不允许未空");
                                        jup.Number(name, dbl);
                                    } else if (jup.UnitType == UnitType.Array) {
                                        if (!name.IsNoneOrNull()) throw new Exception($"语法错误：无效名称");
                                        jup.Number(jup.Count, dbl);
                                    } else {
                                        throw new Exception($"数据类型\"{jup.UnitType.ToString()}\"不支持设置子元素");
                                    }
                                } else {
                                    throw new Exception($"语法错误：字符串必须使用双引号定义");
                                }
                                sb.Clear();
                                pt = Parse_Done_Value;
                            }
                        }
                        break;
                    #endregion
                    case '\r': break;//忽略回车符
                    case '\n':
                        #region [=====处理换行=====]
                        if (isString) {
                            // 不支持字符串中换行
                            throw new Exception($"规则外的换行符");
                        }
                        line++;
                        col = 0;
                        break;
                    #endregion
                    case 'u':
                        #region [=====处理u字符=====]
                        if (isString) {
                            // 不支持的转义字符
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"字符");
                            // 字符串中处理
                            if (isEscape) {
                                // 当为转义时，进入unicode模式
                                isUnicode = true;
                                isEscape = false;
                            } else {
                                // 将字符添加到字符串生成器
                                sb.Append(chr);
                            }
                        } else {
                            // 将字符添加到字符串生成器
                            sb.Append(chr);
                        }
                        break;
                    #endregion
                    case 'n':
                        #region [=====处理n字符=====]
                        if (isString) {
                            // 不支持的转义字符
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"字符");
                            // 字符串中处理
                            if (isEscape) {
                                // 当为转义时，添加换行符
                                sb.Append('\n');
                                isEscape = false;
                            } else {
                                // 将字符添加到字符串生成器
                                sb.Append(chr);
                            }
                        } else {
                            // 将字符添加到字符串生成器
                            sb.Append(chr);
                        }
                        break;
                    #endregion
                    case 'r':
                        #region [=====处理r字符=====]
                        if (isString) {
                            // 不支持的转义字符
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"字符");
                            // 字符串中处理
                            if (isEscape) {
                                // 当为转义时，添加回车符
                                sb.Append('\r');
                            } else {
                                // 将字符添加到字符串生成器
                                sb.Append(chr);
                                isEscape = false;
                            }
                        } else {
                            // 将字符添加到字符串生成器
                            sb.Append(chr);
                        }
                        break;
                    #endregion
                    case '/':
                        #region [=====处理r字符=====]
                        if (isString) {
                            // 不支持的转义字符
                            if (isUnicode) throw new Exception($"规则外的\"{chr}\"字符");
                            // 字符串中处理
                            if (isEscape) {
                                // 当为转义时，添加回车符
                                sb.Append(chr);
                                isEscape = false;
                            } else {
                                // 将字符添加到字符串生成器
                                sb.Append(chr);
                            }
                        } else {
                            // 将字符添加到字符串生成器
                            sb.Append(chr);
                        }
                        break;
                    #endregion
                    default:
                        if (isString) {
                            // 字符串处理
                            if (isEscape) {
                                switch (chr) {
                                    case 'n': sb.Append('\n'); break;
                                    case 'r': sb.Append('\r'); break;
                                    case '/': sb.Append('/'); break;
                                    case 'b': sb.Append('\b'); break;
                                    case 'f': sb.Append('\f'); break;
                                    case 't': sb.Append('\t'); break;
                                    default: throw new Exception($"规则外的\"{chr}\"字符");
                                }
                                isEscape = false;
                            } else if (isUnicode) {
                                uLen++;
                                uCode += int.Parse(chr.ToString(), System.Globalization.NumberStyles.HexNumber) * (int)Math.Pow(16, 4 - uLen);
                                if (uLen == 4) {
                                    char uc = (char)uCode;
                                    // 将字符添加到字符串生成器
                                    sb.Append(uc);
                                    isUnicode = false;
                                    uLen = 0;
                                    uCode = 0;
                                }
                            } else {
                                // 将字符添加到字符串生成器
                                sb.Append(chr);
                            }
                        } else {
                            // 所有完成状态之后不允许尾随字符
                            if ((pt & Parse_Done) == Parse_Done) throw new Exception($"规则外的\"{chr}\"字符");

                            // 将字符添加到字符串生成器
                            sb.Append(chr);
                        }
                        break;
                }
            }

            //} catch (Exception ex) {
            //    throw new Exception($"行 {line} 字符 {col} 解析发生异常", ex);
            //}

            if (jup != null) throw new Exception($"语法错误：Json代码不完整");

            return res;

        }

        /// <summary>
        /// 将Json对象覆盖到标准对象
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static object GetObject(JsonUnit unit, Type tp) {

            object obj;
            if (unit == null) return null;
            //var tp = obj.GetType();

            switch (tp.FullName) {
                case "System.String":
                    // 覆盖字符串
                    obj = unit.GetString();
                    break;
                case "System.Int32":
                    // 覆盖整型
                    obj = (int)unit.GetNumber();
                    break;
                case "System.Int64":
                    // 覆盖长整型
                    obj = (long)unit.GetNumber();
                    break;
                case "System.Single":
                    // 覆盖单精度浮点类型
                    obj = (float)unit.GetNumber();
                    break;
                case "System.Double":
                    // 覆盖双精度浮点类型
                    obj = unit.GetNumber();
                    break;
                case "System.Boolean":
                    // 覆盖布尔类型
                    obj = unit.GetBoolean();
                    break;
                default:
                    if (tp.Name == "List`1") {
                        // 覆盖列表
                        var ptct = tp.GetConstructor(System.Type.EmptyTypes);
                        obj = ptct.Invoke(null);
                        var objAdd = tp.GetMethod("Add");
                        //List<object> list = (List<object>)obj;

                        // 获取列表关联类型
                        var gas = tp.GetGenericArguments();
                        Type ga = null;
                        if (gas.Length > 0) ga = gas[0];

                        // 添加列表单元
                        for (int i = 0; i < unit.Count; i++) {
                            objAdd.Invoke(obj, new object[] { GetObject(unit[i], ga) });
                        }
                    } else {
                        // 覆盖对象
                        var ptct = tp.GetConstructor(System.Type.EmptyTypes);
                        obj = ptct.Invoke(null);
                        var pros = tp.GetProperties();

                        // 遍历属性
                        foreach (var pro in pros) {
                            var pto = GetObject(unit[pro.Name], pro.PropertyType);
                            pro.SetValue(obj, pto, null);
                        }

                    }
                    break;
            }

            return obj;

        }

        /// <summary>
        /// 将对象进行Json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj) {

            StringBuilder sb = new StringBuilder();
            var tp = obj.GetType();

            switch (tp.FullName) {
                case "System.String":
                    // 序列化字符串
                    sb.Append(JsonString.GetJsonString(obj.ToString()));
                    break;
                case "System.Int32":
                case "System.Int64":
                case "System.Single":
                case "System.Double":
                    // 序列化数值
                    sb.Append(obj.ToString());
                    break;
                case "System.Boolean":
                    // 序列化布尔类型
                    sb.Append((bool)obj ? "true" : "false");
                    break;
                default:
                    if (tp.Name == "List`1") {
                        // 序列化列表
                        // 获取相关函数接口
                        var objGetItem = tp.GetMethod("get_Item");
                        var objGetCount = tp.GetMethod("get_Count");

                        // 获取Count属性
                        int count = (int)objGetCount.Invoke(obj, null);

                        // 添加列表单元
                        sb.Append("[");
                        for (int i = 0; i < count; i++) {
                            if (i > 0) sb.Append(",");
                            sb.Append(SerializeObject(objGetItem.Invoke(obj, new object[] { i })));
                        }
                        sb.Append("]");
                    } else {
                        // 序列化对象
                        bool isFirst = true;
                        sb.Append("{");

                        // 遍历属性
                        var pros = tp.GetProperties();
                        foreach (var pro in pros) {
                            if (isFirst) { isFirst = false; } else { sb.Append(","); }
                            sb.AppendFormat("\"{0}\":{1}", pro.Name, SerializeObject(pro.GetValue(obj)));
                        }
                        sb.Append("}");

                    }
                    break;
            }

            return sb.ToString();

        }

        /// <summary>
        /// 将Json字符串反序列化并按类型返回对象
        /// </summary>
        /// <param name="json"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static object DeserializeObject(string json, Type tp) {
            object obj;
            using (var js = ParseJson(json)) {
                obj = GetObject(js, tp);
            }
            return obj;
        }

    }
}
