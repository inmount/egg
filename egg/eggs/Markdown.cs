using System;
using System.Text;
using egg;
using egg.Serializable.Markdown;

/// <summary>
/// Egg 开发套件 静态类专用命名空间
/// </summary>
namespace eggs {

    /// <summary>
    /// Markdown相关函数集合
    /// </summary>
    public static class Markdown {

        // 空操作，等待定义操作符
        private const int Parse_Null = 0x00;

        // 文本相关
        private const int Parse_Text = 0x10;
        private const int Parse_Text_Bold = 0x11;
        private const int Parse_Text_Italic = 0x12;
        private const int Parse_Text_BoldItalic = 0x13;
        private const int Parse_Text_Code = 0x14;
        private const int Parse_Text_Link_Name = 0x15;
        private const int Parse_Text_Link_Name_Finish = 0x16;
        private const int Parse_Text_Link_Url = 0x17;
        private const int Parse_Text_Strikethrough = 0x18;

        // 标题
        private const int Parse_Title = 0x20;

        // 列表
        private const int Parse_List = 0x30;
        private const int Parse_List_Unordered = 0x31;
        private const int Parse_List_Ordered = 0x32;
        private const int Parse_List_Finish = 0x3F;

        // 区块
        private const int Parse_Block = 0x40;
        private const int Parse_Block_Finish = 0x4F;

        // 代码
        private const int Parse_Code = 0x50;
        private const int Parse_Code_Line = 0x51;
        private const int Parse_Code_Block = 0x52;

        // 表格
        private const int Parse_Table = 0x60;
        private const int Parse_Table_Header = 0x61;
        private const int Parse_Table_Align = 0x62;
        private const int Parse_Table_Data = 0x63;

        // 创建新的文本行
        private static void CreateNewTextLine(ref MdBasicBlock pb, ref MdTextLine tlBefore, ref int pt, ref int blk, ref int blkBefore, ref int ls, ref int lsBefore) {
            // 当为空时，增加文本行
            if (pt == Parse_Null || pt == Parse_Block || pt == Parse_Title || (pt & Parse_List) == Parse_List || (pt & Parse_Table) == Parse_Table) {

                // 记录区块相关
                if (pt == Parse_Block) {
                    blkBefore = blk;
                }

                // 记录列表相关
                if ((pt & Parse_List) == Parse_List) {
                    lsBefore = ls;
                }

                // 预处理表格相关
                if (pt == Parse_Table) {
                    MdTable mdTable = (MdTable)pb;
                    if (mdTable.Row == 0) {
                        if (mdTable.Children.Count <= 0) {
                            // 添加头
                            MdTableHeader mdTableHeader = new MdTableHeader();
                            mdTable.Children.Add(mdTableHeader);
                            pb = mdTableHeader;
                        }
                    } else if (mdTable.Row == 1) {
                        if (mdTable.Children.Count <= 1) {
                            // 添加对齐
                            MdTableAlign mdTableAlign = new MdTableAlign();
                            mdTable.Children.Add(mdTableAlign);
                            pb = mdTableAlign;
                        }
                    } else {
                        if (mdTable.Children.Count <= mdTable.Row) {
                            // 添加数据
                            MdTableRow mdTableRow = new MdTableRow();
                            mdTable.Children.Add(mdTableRow);
                            pb = mdTableRow;
                        }
                    }
                    pb = (MdBasicBlock)mdTable.Children[mdTable.Row];
                }

                // 增加文本行对象
                pt = Parse_Text;
                MdTextLine mdTextLine = new MdTextLine();
                pb.Children.Add(mdTextLine);
                pb = mdTextLine;

            }
        }

        // 处理换行
        private static void ExecuteTurnLine(MdDocumentRoot doc, StringBuilder sb, ref MdBasicBlock pb, ref MdTextLine tlBefore, ref int pt, ref int blk, ref int blkBefore, ref int ls, ref int lsBefore) {
            #region [=====处理换行=====]

            if (pt == Parse_Code_Block) {
                #region [=====代码区块模式=====]
                // 代码块模式下，直接放全放进去

                if (sb.Length == 3) {
                    if (sb.ToString() == "```") {
                        // 清理缓存
                        sb.Clear();
                        // 缓存信息
                        tlBefore = null;
                        lsBefore = ls;
                        blkBefore = blk;
                        // 重新设定模式
                        pb = doc;
                        pt = Parse_Null;
                        ls = 0;
                        blk = 0;
                    }
                }

                if (pt == Parse_Code_Block) {
                    var codeBlock = (MdCodeBlock)pb;
                    if (codeBlock.Language.IsNull()) {
                        codeBlock.Language = sb.ToString();
                    } else {
                        MdCodeLine mdCodeLine = new MdCodeLine();
                        codeBlock.Children.Add(mdCodeLine);
                        MdText mdText = new MdText() { Content = sb.ToString() };
                        mdCodeLine.Children.Add(mdText);
                    }
                    // 清理缓存
                    sb.Clear();
                }
                #endregion
            } else if (pt == Parse_Table) {
                // 移动到下一行
                MdTable mdTable = (MdTable)pb;
                if (sb.Length > 0) {
                    // 检测并添加一个文本行对象
                    CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                    if (pb.ParentBlock.Type == MdTypes.TableAlign) {
                        string temp = sb.ToString().Trim();
                        string align = "";
                        if (temp.Length > 2) {
                            if (temp.StartsWith(":")) {
                                if (temp.EndsWith(":")) {
                                    align = "center";
                                } else {
                                    align = "left";
                                }
                            } else {
                                if (temp.EndsWith(":")) {
                                    align = "right";
                                }
                            }
                        }
                        MdText mdText = new MdText() { Content = align };
                        pb.Children.Add(mdText);
                        // 退回到表格模式
                        pt = Parse_Table;
                        // 清理缓存
                        sb.Clear();
                    } else if (pb.ParentBlock.Type == MdTypes.TableHeader || pb.ParentBlock.Type == MdTypes.TableRow) {
                        MdText mdText = new MdText() { Content = sb.ToString().Trim() };
                        pb.Children.Add(mdText);
                        // 退回到表格模式
                        pt = Parse_Table;
                        // 清理缓存
                        sb.Clear();
                    }
                }
                mdTable.Row++;
                // 缓存信息
                lsBefore = ls;
                blkBefore = blk;
                // 重新设定模式
                pb = doc;
                pt = Parse_Null;
                ls = 0;
                blk = 0;
            } else if (pt == Parse_Text_BoldItalic) {
                // 粗斜体未关闭
                // 将之前的内容进行添加
                MdText mdText = new MdText() { Content = sb.ToString(3, sb.Length - 3) };
                mdText.IsBold = true;
                mdText.IsItalic = true;
                pb.Children.Add(mdText);
                // 清理缓存
                sb.Clear();
                // 缓存信息
                tlBefore = (MdTextLine)pb;
                lsBefore = ls;
                blkBefore = blk;
                // 重新设定模式
                pb = doc;
                pt = Parse_Null;
                ls = 0;
                blk = 0;
            } else if (pt == Parse_Text_Bold) {
                // 粗体未关闭
                // 将之前的内容进行添加
                MdText mdText = new MdText() { Content = sb.ToString(2, sb.Length - 2) };
                mdText.IsBold = true;
                pb.Children.Add(mdText);
                // 清理缓存
                sb.Clear();
                // 缓存信息
                tlBefore = (MdTextLine)pb;
                lsBefore = ls;
                blkBefore = blk;
                // 重新设定模式
                pb = doc;
                pt = Parse_Null;
                ls = 0;
                blk = 0;
            } else if (pt == Parse_Text_Italic) {
                // 斜体未关闭
                // 将之前的内容进行添加
                MdText mdText = new MdText() { Content = sb.ToString(1, sb.Length - 1) };
                mdText.IsItalic = true;
                pb.Children.Add(mdText);
                // 清理缓存
                sb.Clear();
                // 缓存信息
                tlBefore = (MdTextLine)pb;
                lsBefore = ls;
                blkBefore = blk;
                // 重新设定模式
                pb = doc;
                pt = Parse_Null;
                ls = 0;
                blk = 0;
            } else if (pt == Parse_Text_Strikethrough) {
                // 斜体未关闭
                // 将之前的内容进行添加
                MdText mdText = new MdText() { Content = sb.ToString() };
                mdText.IsStrikethrough = true;
                pb.Children.Add(mdText);
                // 清理缓存
                sb.Clear();
                // 缓存信息
                tlBefore = (MdTextLine)pb;
                lsBefore = ls;
                blkBefore = blk;
                // 重新设定模式
                pb = doc;
                pt = Parse_Null;
                ls = 0;
                blk = 0;
            } else {
                // 普通模式下，先判断是否有内容
                if (sb.Length > 0) {
                    #region [=====添加内容=====]

                    // 判断当前是否为文本行，如果不是，则先添加一个文本行对象
                    if (pb.Type != MdTypes.TextLine) {
                        #region [=====添加一个文本行对象=====]
                        // 当一行文本尚未组成特殊格式，则直接按文本处理
                        if (pb == doc) {
                            // 判断是否添加到之前对象增加文本行对象
                            var pbLast = doc.Children[doc.Children.Count - 1];
                            if (pbLast.Type == MdTypes.Null) {
                                MdTextLine mdTextLine = new MdTextLine();
                                pb.Children.Add(mdTextLine);
                                pb = mdTextLine;
                            } else {
                                pb = tlBefore;
                            }
                        } else {
                            // 增加文本行对象
                            MdTextLine mdTextLine = new MdTextLine();
                            pb.Children.Add(mdTextLine);
                            pb = mdTextLine;
                        }
                        // 转为文本解析模式
                        pt = Parse_Text;
                        #endregion
                    }

                    // 判断是否为连续的*-_中的符号
                    bool isLine = false;
                    if (sb.Length > 3) {
                        if (sb[0] == '*' || sb[0] == '-' || sb[0] == '_') {
                            isLine = true;
                            for (int s = 1; s < sb.Length; s++) {
                                if (sb[s] != sb[0]) {
                                    isLine = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (isLine) {
                        // 添加一个行对象
                        pb.Children.Add(new MdLine());
                    } else {
                        // 添加文本
                        pb.Children.Add(new MdText() { Content = sb.ToString() });
                    }

                    // 清理缓存
                    sb.Clear();
                    // 缓存信息
                    tlBefore = (MdTextLine)pb;
                    lsBefore = ls;
                    blkBefore = blk;
                    // 重新设定模式
                    pb = doc;
                    pt = Parse_Null;
                    ls = 0;
                    blk = 0;

                    #endregion
                } else {
                    #region [=====添加空行=====]
                    // 当本行无任何东西时的处理
                    if (pb == doc) {

                        if (doc.Children.Count > 0) {
                            var pbLast = doc.Children[doc.Children.Count - 1];
                            if (pbLast.Type == MdTypes.TextLine) {
                                MdTextLine mdTextLine = (MdTextLine)pbLast;
                                mdTextLine.IsSection = true;
                            }
                        }

                        // 添加一个空行
                        MdNull mdNull = new MdNull();
                        pb.Children.Add(mdNull);

                        // 缓存信息
                        tlBefore = null;

                    } else {
                        // 添加一个空行
                        if (pb.Type != MdTypes.TextLine) {
                            MdTextLine mdTextLine = new MdTextLine();
                            pb.Children.Add(mdTextLine);
                            pb = mdTextLine;
                        }
                        MdNull mdNull = new MdNull();
                        pb.Children.Add(mdNull);

                        // 缓存信息
                        tlBefore = (MdTextLine)pb;
                    }

                    // 缓存信息
                    lsBefore = ls;
                    blkBefore = blk;
                    // 重新设定模式
                    pb = doc;
                    pt = Parse_Null;
                    ls = 0;
                    blk = 0;
                    #endregion
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static MdDocumentRoot GetDocument(string str) {
            MdDocumentRoot doc = new MdDocumentRoot();

            // 当前操作的区块
            MdTextLine tlBefore = null;
            MdBasicBlock pb = doc;

            int blk = 0;
            int blkBefore = 0;
            int ls = 0;
            int lsBefore = 0;

            // 初始化字符串
            StringBuilder sb = new StringBuilder();

            // 初始化字符串和转义标志
            bool isEscape = false;

            // 初始化操作类型为空操作
            int pt = Parse_Null;

            // 行列计数器
            int line = 1;
            int col = 0;

            //try {

            //遍历字符串进行解析
            for (int i = 0; i < str.Length; i++) {
                col++;
                char chr = str[i];
                switch (chr) {
                    case '\\':
                        #region [=====处理转义符=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            // 当未处于转义模式，则进入转义模式
                            if (pt == Parse_Code_Line || pt == Parse_Code_Block) {
                                // 代码行及块不支持转义
                                sb.Append(chr);
                            } else {
                                // 检测并添加一个文本行对象
                                CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                isEscape = true;
                            }
                        }
                        break;
                    #endregion
                    case '#':
                    case '>':
                    case '+':
                    case '-':
                    case '.':
                        #region [=====处理特殊字符=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            sb.Append(chr);
                        }
                        break;
                    #endregion
                    case '|':
                        #region [=====处理特殊字符=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            // 刚进入表格,读到了第一列数据
                            if (pt == Parse_Table) {
                                #region [=====表格模式=====]
                                // 检测并添加一个文本行对象
                                var pbTable = pb;
                                CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);

                                string temp = sb.ToString().Trim();
                                if (!temp.IsEmpty()) {
                                    #region [=====处理在表格中的情况]
                                    // 当父对象是表格对象时，添加表格内容
                                    if (pb.ParentBlock.Type == MdTypes.TableAlign) {
                                        //string temp = sb.ToString().Trim();
                                        string align = "";
                                        if (temp.Length > 2) {
                                            if (temp.StartsWith(":")) {
                                                if (temp.EndsWith(":")) {
                                                    align = "center";
                                                } else {
                                                    align = "left";
                                                }
                                            } else {
                                                if (temp.EndsWith(":")) {
                                                    align = "right";
                                                }
                                            }
                                        }
                                        MdText mdText = new MdText() { Content = align };
                                        pb.Children.Add(mdText);
                                        // 退回到表格模式
                                        pt = Parse_Table;
                                        pb = pb.ParentBlock.ParentBlock;
                                        // 清理缓存
                                        sb.Clear();
                                        break;
                                    } else if (pb.ParentBlock.Type == MdTypes.TableHeader || pb.ParentBlock.Type == MdTypes.TableRow) {
                                        MdText mdText = new MdText() { Content = temp };
                                        pb.Children.Add(mdText);
                                        // 退回到表格模式
                                        pt = Parse_Table;
                                        pb = pb.ParentBlock.ParentBlock;
                                        // 清理缓存
                                        sb.Clear();
                                        break;
                                    }
                                    #endregion
                                }

                                // 退回到表格模式
                                pb = pbTable;
                                break;
                                #endregion
                            } else if (pt == Parse_Text) {
                                if (pb.ParentBlock != null) {
                                    #region [=====处理在表格中的情况]
                                    // 当父对象是表格对象时，添加表格内容
                                    if (pb.ParentBlock.Type == MdTypes.TableAlign) {
                                        string temp = sb.ToString().Trim();
                                        string align = "";
                                        if (temp.Length > 2) {
                                            if (temp.StartsWith(":")) {
                                                if (temp.EndsWith(":")) {
                                                    align = "center";
                                                } else {
                                                    align = "left";
                                                }
                                            } else {
                                                if (temp.EndsWith(":")) {
                                                    align = "right";
                                                }
                                            }
                                        }
                                        MdText mdText = new MdText() { Content = align };
                                        pb.Children.Add(mdText);
                                        // 退回到表格模式
                                        pt = Parse_Table;
                                        pb = pb.ParentBlock.ParentBlock;
                                        // 清理缓存
                                        sb.Clear();
                                        break;
                                    } else if (pb.ParentBlock.Type == MdTypes.TableHeader || pb.ParentBlock.Type == MdTypes.TableRow) {
                                        MdText mdText = new MdText() { Content = sb.ToString().Trim() };
                                        pb.Children.Add(mdText);
                                        // 退回到表格模式
                                        pt = Parse_Table;
                                        pb = pb.ParentBlock.ParentBlock;
                                        // 清理缓存
                                        sb.Clear();
                                        break;
                                    }
                                    #endregion
                                }
                            }
                            sb.Append(chr);
                        }
                        break;
                    #endregion
                    case '`':
                        #region [=====处理代码段=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            if (sb.ToString() == "``") {
                                if (pt != Parse_Code_Block) {
                                    // 创建新的代码段
                                    MdCodeBlock mdCodeBlock = new MdCodeBlock();
                                    pb.Children.Add(mdCodeBlock);
                                    pb = mdCodeBlock;
                                    // 切换为代码段
                                    pt = Parse_Code_Block;
                                    // 清理缓存
                                    sb.Clear();
                                } else {
                                    sb.Append(chr);
                                }
                            } else {
                                sb.Append(chr);
                            }
                        }
                        break;
                    #endregion
                    case '*':
                    case '_':
                        #region [=====处理字体=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            if (pt == Parse_Text) {
                                #region [=====操作字体=====]
                                // 优先进入斜体模式
                                if (sb.Length > 1) {
                                    // 将之前的内容进行添加
                                    MdText mdText = new MdText() { Content = sb.ToString() };
                                    pb.Children.Add(mdText);
                                }
                                // 设置模式
                                pt = Parse_Text_Italic;
                                // 清理缓存
                                sb.Clear();
                                sb.Append(chr);
                                #endregion
                            } else if (pt == Parse_Text_BoldItalic) {
                                // 文本模式中操作字体
                                sb.Append(chr);
                                string temp = sb.ToString();
                                if ((temp.EndsWith("***") || temp.EndsWith("___")) && temp.Length >= 6) {
                                    // 将之前的内容进行添加
                                    MdText mdText = new MdText() { Content = temp.Substring(3, temp.Length - 6) };
                                    mdText.IsBold = true;
                                    mdText.IsItalic = true;
                                    pb.Children.Add(mdText);
                                    // 设置模式
                                    pt = Parse_Text;
                                    // 清理缓存
                                    sb.Clear();
                                }
                            } else if (pt == Parse_Text_Bold) {
                                // 文本模式中操作字体
                                sb.Append(chr);
                                string temp = sb.ToString();
                                if (sb.ToString() == "***" || sb.ToString() == "___") {
                                    // 进粗斜体模式
                                    pt = Parse_Text_BoldItalic;
                                    break;
                                }
                                if (temp.EndsWith("**") || temp.EndsWith("__")) {
                                    // 将之前的内容进行添加
                                    MdText mdText = new MdText() { Content = temp.Substring(2, temp.Length - 4) };
                                    mdText.IsBold = true;
                                    pb.Children.Add(mdText);
                                    // 设置模式
                                    pt = Parse_Text;
                                    // 清理缓存
                                    sb.Clear();
                                }
                            } else if (pt == Parse_Text_Italic) {
                                sb.Append(chr);
                                if (sb.ToString() == "**" || sb.ToString() == "__") {
                                    // 进粗体模式
                                    pt = Parse_Text_Bold;
                                    break;
                                }
                                // 将之前的内容进行添加
                                MdText mdText = new MdText() { Content = sb.ToString(0, sb.Length - 1) };
                                mdText.IsItalic = true;
                                pb.Children.Add(mdText);
                                // 设置模式
                                pt = Parse_Text;
                                // 清理缓存
                                sb.Clear();
                            } else {
                                sb.Append(chr);
                            }
                        }
                        break;
                    #endregion
                    case '[':
                        #region [=====处理超链接名称=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            if (pt == Parse_Text) {
                                #region [=====操作字体=====]
                                // 优先进入超链接模式
                                if (sb.Length > 1) {
                                    // 将之前的内容进行添加
                                    MdText mdText = new MdText() { Content = sb.ToString() };
                                    pb.Children.Add(mdText);
                                }
                                // 设置模式
                                pt = Parse_Text_Link_Name;
                                // 清理缓存
                                sb.Clear();
                                #endregion
                            } else {
                                sb.Append(chr);
                            }
                        }
                        break;
                    #endregion
                    case ']':
                        #region [=====处理超链接名称结束=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            if (pt == Parse_Text_Link_Name) {
                                #region [=====操作字体=====]
                                // 优先进入超链接模式
                                if (sb.Length > 1) {
                                    // 将之前的内容进行添加
                                    MdTextLink mdTextLink = new MdTextLink() { Content = sb.ToString() };
                                    mdTextLink.Url = "#";
                                    pb.Children.Add(mdTextLink);
                                }
                                // 设置模式
                                pt = Parse_Text_Link_Name_Finish;
                                // 清理缓存
                                sb.Clear();
                                #endregion
                            } else {
                                sb.Append(chr);
                            }
                        }
                        break;
                    #endregion
                    case '(':
                        #region [=====处理链接=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            if (pt == Parse_Text_Link_Name_Finish) {
                                // 设置模式
                                pt = Parse_Text_Link_Url;
                                // 清理缓存
                                sb.Clear();
                            } else {
                                sb.Append(chr);
                            }
                        }
                        break;
                    #endregion
                    case ')':
                        #region [=====处理超链接名称结束=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            if (pt == Parse_Text_Link_Url) {
                                #region [=====操作字体=====]
                                // 优先进入超链接模式
                                if (sb.Length > 1) {
                                    // 将之前的内容进行添加
                                    MdTextLink mdTextLink = (MdTextLink)pb.Children[pb.Children.Count - 1];
                                    mdTextLink.Url = sb.ToString();
                                }
                                // 设置模式
                                pt = Parse_Text;
                                // 清理缓存
                                sb.Clear();
                                #endregion
                            } else {
                                sb.Append(chr);
                            }
                        }
                        break;
                    #endregion
                    case '~':
                        #region [=====处理删除线=====]
                        // 当处于字符串模式时，操作转义
                        if (isEscape) {
                            // 当处于转义模式，则直接添加字符并退出转义
                            sb.Append(chr);
                            isEscape = false;
                        } else {
                            if (pt == Parse_Text) {
                                string temp = sb.ToString();
                                if (temp.EndsWith("~")) {
                                    // 将之前的内容进行添加
                                    MdText mdText = new MdText() { Content = sb.ToString(0, sb.Length - 1) };
                                    pb.Children.Add(mdText);
                                    // 设置模式
                                    pt = Parse_Text_Strikethrough;
                                    // 清理缓存
                                    sb.Clear();
                                } else {
                                    sb.Append(chr);
                                }
                            } else if (pt == Parse_Text_Strikethrough) {
                                string temp = sb.ToString();
                                if (temp.EndsWith("~")) {
                                    // 将之前的内容进行添加
                                    MdText mdText = new MdText() { Content = sb.ToString(0, sb.Length - 1) };
                                    mdText.IsStrikethrough = true;
                                    pb.Children.Add(mdText);
                                    // 设置模式
                                    pt = Parse_Text;
                                    // 清理缓存
                                    sb.Clear();
                                } else {
                                    sb.Append(chr);
                                }
                            } else {
                                sb.Append(chr);
                            }
                        }
                        break;
                    #endregion
                    case ' ':
                        #region [=====处理空格=====]

                        // 当处于字符串模式时，操作转义
                        if (pt == Parse_Null || pt == Parse_Block || pt == Parse_List) {
                            string temp = sb.ToString();
                            switch (temp) {
                                case ">":
                                    #region [=====区块模式=====]
                                    // 列表模式模式中直接按字符串处理
                                    if (pt == Parse_List) {
                                        sb.Append(chr);
                                        break;
                                    }
                                    // 区块模式
                                    if (blk < blkBefore) {
                                        // 已存在区块则直接指向已有区块
                                        pb = (MdBasicBlock)pb.Children[pb.Children.Count - 1];
                                    } else {
                                        // 添加一个区块
                                        MdBlock mdBlock = new MdBlock();
                                        pb.Children.Add(mdBlock);
                                        pb = mdBlock;
                                    }
                                    blk++;
                                    // 变更解析类型
                                    pt = Parse_Block;
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "#":
                                    #region [=====标题模式=====]
                                    // 标题模式
                                    MdTitle mdTitle = new MdTitle(1);
                                    pb.Children.Add(mdTitle);
                                    pb = mdTitle;
                                    sb.Clear();

                                    // 检测并添加一个文本行对象
                                    CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "##":
                                    #region [=====二级标题模式=====]
                                    // 二级标题模式
                                    mdTitle = new MdTitle(2);
                                    pb.Children.Add(mdTitle);
                                    pb = mdTitle;

                                    // 检测并添加一个文本行对象
                                    CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "###":
                                    #region [=====三级标题模式=====]
                                    // 三级标题模式
                                    mdTitle = new MdTitle(3);
                                    pb.Children.Add(mdTitle);
                                    pb = mdTitle;

                                    // 检测并添加一个文本行对象
                                    CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "####":
                                    #region [=====四级标题模式=====]
                                    // 四级标题模式
                                    mdTitle = new MdTitle(4);
                                    pb.Children.Add(mdTitle);
                                    pb = mdTitle;

                                    // 检测并添加一个文本行对象
                                    CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "#####":
                                    #region [=====五级标题模式=====]
                                    // 五级标题模式
                                    mdTitle = new MdTitle(5);
                                    pb.Children.Add(mdTitle);
                                    pb = mdTitle;

                                    // 检测并添加一个文本行对象
                                    CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "######":
                                    #region [=====六级标题模式=====]
                                    // 六级标题模式
                                    mdTitle = new MdTitle(6);
                                    pb.Children.Add(mdTitle);
                                    pb = mdTitle;

                                    // 检测并添加一个文本行对象
                                    CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "*":
                                case "+":
                                case "-":
                                    #region [=====无序列表模式=====]
                                    // 列表模式模式中直接按字符串处理
                                    if (pt == Parse_List) {
                                        sb.Append(chr);
                                        break;
                                    }
                                    // 先判读是否再同一区块下
                                    if (blk == blkBefore) {
                                        // 判断当前是否为已有无序列表的项目
                                        bool isItem = false;
                                        if (pb.Children.Count > 0) {
                                            var pbLast = pb.Children[pb.Children.Count - 1];

                                            if (pbLast.Type == MdTypes.List) {
                                                MdList mdList = (MdList)pbLast;
                                                isItem = !mdList.IsOrdered;
                                                if (isItem) pb = mdList;
                                            }
                                        }

                                        // 新项目则添加一个列表对象
                                        if (!isItem) {
                                            MdList mdList = new MdList();
                                            pb.Children.Add(mdList);
                                            pb = mdList;
                                        }

                                        // 添加列表项目
                                        MdListItem mdListItem = new MdListItem();
                                        pb.Children.Add(mdListItem);
                                        pb = mdListItem;
                                    } else {
                                        // 新项目则添加一个列表对象
                                        MdList mdList = new MdList();
                                        pb.Children.Add(mdList);
                                        pb = mdList;

                                        // 添加列表项目
                                        MdListItem mdListItem = new MdListItem();
                                        pb.Children.Add(mdListItem);
                                        pb = mdListItem;
                                    }
                                    ls++;
                                    // 变更解析类型
                                    pt = Parse_List;
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                case "   ":
                                    #region [=====列表层级模式=====]
                                    // 列表模式模式中直接按字符串处理
                                    if (pt == Parse_List) {
                                        sb.Append(chr);
                                        break;
                                    }
                                    // 判断是否为代码行；
                                    if (pt == Parse_Null) {
                                        MdCodeLine mdCodeLine = new MdCodeLine();
                                        pb.Children.Add(mdCodeLine);
                                        pb = mdCodeLine;
                                        // 清理缓存
                                        sb.Clear();
                                        break;
                                    }
                                    // 先判读是否再同一区块下
                                    if (blk == blkBefore) {
                                        // 判断之前是否有列表
                                        if (ls <= lsBefore) {
                                            // 已存在区块则直接指向已有区块
                                            pb = (MdBasicBlock)pb.Children[pb.Children.Count - 1];
                                            ls++;
                                            // 清理缓存
                                            sb.Clear();
                                        }
                                    } else {

                                        CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                                    }
                                    break;
                                #endregion
                                case "|":
                                    #region [=====表格模式=====]
                                    // 列表模式模式中直接按字符串处理
                                    if (pt == Parse_List) {
                                        sb.Append(chr);
                                        break;
                                    }

                                    // 判断对比上一行是否在同一区块
                                    bool isNew = true;
                                    if (blk == blkBefore) {
                                        // 判断是否已经存在表格
                                        if (pb.Children.Count > 0) {
                                            var pbLast = pb.Children[pb.Children.Count - 1];

                                            if (pbLast.Type == MdTypes.Table) {
                                                MdTable mdTable = (MdTable)pbLast;
                                                pb = mdTable;
                                                isNew = false;
                                            }
                                        }
                                    }

                                    if (isNew) {
                                        MdTable mdTable = new MdTable();
                                        pb.Children.Add(mdTable);
                                        pb = mdTable;
                                    }

                                    // 变更解析类型
                                    pt = Parse_Table;
                                    // 清理缓存
                                    sb.Clear();
                                    break;
                                #endregion
                                default:
                                    // 列表模式模式中直接按字符串处理
                                    if (pt == Parse_List) {
                                        sb.Append(chr);
                                        break;
                                    }
                                    #region [=====判断是否为有序列表=====]
                                    // 判断是否为有序列表
                                    bool isInList = false;
                                    if (temp.EndsWith(".")) {
                                        string tempNum = temp.Substring(0, temp.Length - 1);
                                        if (tempNum.IsInteger()) {
                                            // 处于有序列表中
                                            isInList = true;
                                            // 先判读是否再同一区块下
                                            if (blk == blkBefore) {
                                                // 判断当前是否为已有无序列表的项目
                                                bool isItem = false;
                                                if (pb.Children.Count > 0) {
                                                    var pbLast = pb.Children[pb.Children.Count - 1];

                                                    if (pbLast.Type == MdTypes.List) {
                                                        MdList mdList = (MdList)pbLast;
                                                        isItem = mdList.IsOrdered;
                                                        if (isItem) pb = mdList;
                                                    }
                                                }

                                                // 新项目则添加一个列表对象
                                                if (!isItem) {
                                                    MdList mdList = new MdList();
                                                    mdList.IsOrdered = true;
                                                    mdList.SerialNumber = tempNum.ToInteger();
                                                    pb.Children.Add(mdList);
                                                    pb = mdList;
                                                }

                                                // 添加列表项目
                                                MdListItem mdListItem = new MdListItem();
                                                mdListItem.SerialNumber = tempNum.ToInteger();
                                                pb.Children.Add(mdListItem);
                                                pb = mdListItem;
                                            } else {
                                                // 新项目则添加一个列表对象
                                                MdList mdList = new MdList();
                                                mdList.IsOrdered = true;
                                                mdList.SerialNumber = tempNum.ToInteger();
                                                pb.Children.Add(mdList);
                                                pb = mdList;

                                                // 添加列表项目
                                                MdListItem mdListItem = new MdListItem();
                                                mdListItem.SerialNumber = tempNum.ToInteger();
                                                pb.Children.Add(mdListItem);
                                                pb = mdListItem;
                                            }
                                            ls++;
                                            // 变更解析类型
                                            pt = Parse_List;
                                            // 清理缓存
                                            sb.Clear();
                                        }
                                    }
                                    #endregion
                                    if (!isInList) sb.Append(chr);
                                    break;
                            }
                        } else {
                            sb.Append(chr);
                        }
                        break;
                    #endregion
                    case '\r': break;//忽略回车符
                    case '\n':
                        // 处理换行
                        ExecuteTurnLine(doc, sb, ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                        line++;
                        col = 0;
                        break;
                    default:
                        bool isMayBeList = false;
                        if ((pt & Parse_Table) != Parse_Table) {
                            if (chr >= '0' && chr <= '9') {
                                // 当为数字时，假定可能为有序列表
                                if (sb.Length == 0) {
                                    isMayBeList = true;
                                } else {
                                    if (sb.ToString().IsInteger()) {
                                        isMayBeList = true;
                                    }
                                }
                            }
                        }
                        // 检测并添加一个文本行对象
                        if (!isMayBeList) CreateNewTextLine(ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
                        // 添加字符
                        sb.Append(chr);
                        break;
                }
            }

            if (sb.Length > 0) {
                // 处理换行
                ExecuteTurnLine(doc, sb, ref pb, ref tlBefore, ref pt, ref blk, ref blkBefore, ref ls, ref lsBefore);
            }

            return doc;
        }

        /// <summary>
        /// 获取HTML转码后的序列化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(string str) {
            if (str.IsNull()) return null;
            string res = str;
            res = res.Replace("\\", @"\\");
            res = res.Replace("`", @"\`");
            res = res.Replace("*", @"\*");
            res = res.Replace("{", @"\{");
            res = res.Replace("}", @"\}");
            res = res.Replace("[", @"\[");
            res = res.Replace("]", @"\]");
            res = res.Replace("(", @"\(");
            res = res.Replace(")", @"\)");
            res = res.Replace("#", @"\#");
            res = res.Replace("+", @"\+");
            res = res.Replace("-", @"\-");
            res = res.Replace(".", @"\.");
            res = res.Replace("!", @"\!");
            return res;
        }

    }

}
