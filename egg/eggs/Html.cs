using System;
using System.Text;
using egg;
using egg.Serializable.Html;

/// <summary>
/// Egg 开发套件 静态类专用命名空间
/// </summary>
namespace eggs {

    /// <summary>
    /// Html相关函数集合
    /// </summary>
    public static class Html {

        /// <summary>
        /// 解析器类型
        /// </summary>
        private enum ParserTypes {

            /// <summary>
            /// 无操作
            /// </summary>
            None,

            /// <summary>
            /// 节点名称
            /// </summary>
            NodeName,

            /// <summary>
            /// 节点结束
            /// </summary>
            NodeFinish,

            /// <summary>
            /// 数据
            /// </summary>
            CData,

            /// <summary>
            /// 属性名称
            /// </summary>
            PropertyName,

            /// <summary>
            /// 属性名称结束
            /// </summary>
            PropertyNameFinish,

            /// <summary>
            /// 属性值
            /// </summary>
            PropertyValue,

            /// <summary>
            /// 属性值结束
            /// </summary>
            PropertyValueFinish,

            /// <summary>
            /// 注释
            /// </summary>
            Note
        }

        /// <summary>
        /// 获取HTML转码后的序列化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EscapeEncode(string str) {
            string res = str;
            res = res.Replace("&", "&amp;");//处理特殊输入
            //res = res.Replace("\r", "").Replace("\n", "&enter;");//处理换行
            res = res.Replace("\"", "&quot;");//处理双引号
            res = res.Replace(" ", "&nbsp;");//处理空格
            res = res.Replace("<", "&lt;");//处理小于号
            res = res.Replace(">", "&gt;");//处理大于号
            res = res.Replace("'", "&apos;");//处理单引号
            return res;
        }

        /// <summary>
        /// 获取HTML反转码后的序列化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EscapeDecode(string str) {
            string res = str;
            //res = res.Replace("&enter;", "\r\n");//处理换行
            res = res.Replace("&quot;", "\"");//处理双引号
            res = res.Replace("&apos;", "'");//处理双引号
            res = res.Replace("&nbsp;", " ");//处理空格
            res = res.Replace("&lt;", "<");//处理小于号
            res = res.Replace("&gt;", ">");//处理大于号
            res = res.Replace("&amp;", "&");//处理特殊输入
            return res;
        }

        /// <summary>
        /// 获取节点对象
        /// </summary>
        /// <param name="html"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static HtmlNodeCollection GetNodes(string html, HtmlElement parent = null) {

            // 初始化对象
            HtmlNodeCollection nodes = new HtmlNodeCollection(parent);
            Node np = null;

            // 当前解析器类型
            ParserTypes pt = ParserTypes.None;

            // 缓存
            string tagName = null;
            string pName = null;
            string pValue = null;
            StringBuilder sb = new StringBuilder();

            // 文档数据
            int line = 1;
            int site = 0;

            for (int i = 0; i < html.Length; i++) {
                site++;
                char chr = html[i];
                switch (chr) {
                    #region [=====左尖括号=====]
                    case '<':
                        if (pt == ParserTypes.None) {
                            // 当有内容独立文本时，增加文本节点
                            if (sb.Length > 0) {
                                // 判断是否存在处理对象
                                if (np == null) {
                                    // 新增主对象
                                    var node = new TextNode();
                                    node.SetEncodeValue(sb.ToString());
                                    nodes.Add(node);
                                } else {
                                    // 新增子对象
                                    var npNormal = (HtmlElement)np;
                                    var nodeNew = new TextNode();
                                    npNormal.Nodes.Add(nodeNew);
                                    nodeNew.SetEncodeValue(sb.ToString());
                                }
                                // 清理缓存
                                sb.Clear();
                            }
                            pt = ParserTypes.NodeName;
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====右尖括号=====]
                    case '>':
                        if (pt == ParserTypes.NodeName) {
                            // 无属性情况
                            tagName = sb.ToString();
                            if (tagName.IsEmpty()) throw new Exception($"规则外的字符'{chr}'");
                            pt = ParserTypes.None;
                            // 判断是否存在处理对象
                            if (np == null) {
                                // 新增主对象
                                if (tagName.ToUpper() == "!DOCTYPE") {
                                    if (parent.Nodes.Count > 0) throw new Exception($"DOCTYPE必须定义在文档前端");
                                    var node = new DeclarationNode();
                                    nodes.Add(node);
                                    np = node;
                                } else if (tagName.ToLower() == "style" || tagName.ToLower() == "script") {
                                    var node = new HtmlDataElement(tagName);
                                    nodes.Add(node);
                                    np = node;
                                } else {
                                    var node = new HtmlElement(tagName);
                                    nodes.Add(node);
                                    np = node;
                                }
                            } else {
                                // 新增子对象
                                if (tagName.ToUpper() == "!DOCTYPE") throw new Exception($"DOCTYPE必须定义在文档前端");
                                if (tagName.ToLower() == "style" || tagName.ToLower() == "script") {
                                    var npNormal = (HtmlElement)np;
                                    var nodeNew = new HtmlDataElement(tagName);
                                    npNormal.Children.Add(nodeNew);
                                    np = nodeNew;
                                } else if (tagName.ToLower() == "link" || tagName.ToLower() == "meta") {
                                    var npNormal = (HtmlElement)np;
                                    var nodeNew = new HtmlElement(tagName);
                                    npNormal.Children.Add(nodeNew);
                                } else {
                                    var npNormal = (HtmlElement)np;
                                    var nodeNew = new HtmlElement(tagName);
                                    npNormal.Children.Add(nodeNew);
                                    np = nodeNew;
                                }
                            }
                            // 清理缓存
                            tagName = null;
                            sb.Clear();
                        } else if (pt == ParserTypes.NodeFinish) {
                            if (np.NodeType != NodeType.Element) throw new Exception($"规则外的字符'{chr}'");
                            var npNormal = (HtmlElement)np;
                            tagName = sb.ToString();
                            // 单节点模式
                            if (npNormal.IsSingle) {
                                if (!tagName.IsEmpty()) throw new Exception($"语法错误");
                                // 返回上一层
                                np = np.Parent;
                                // 过滤父对象
                                if (np == parent) np = null;
                            } else {
                                if (tagName.IsEmpty()) throw new Exception($"缺少标签名称");
                                //if (np.Parent == null) throw new Exception($"多余的尾部标签");
                                if (npNormal.TagName != tagName) throw new Exception($"首尾标签名不匹配");
                                // 返回上一层
                                np = np.Parent;
                                // 过滤父对象
                                if (np == parent) np = null;
                            }
                            // 设置解析对象为空
                            pt = ParserTypes.None;
                            // 清理缓存
                            tagName = null;
                            sb.Clear();
                        } else if (pt == ParserTypes.PropertyValueFinish || pt == ParserTypes.PropertyName) {
                            // 根据标签类型确定之后的解析情况
                            var npNormal = (HtmlElement)np;
                            string npTagName = npNormal.TagName.ToLower();
                            if (npTagName == "style" || npTagName == "script") {
                                // 设置解析对象为空
                                pt = ParserTypes.CData;
                            } else {
                                // 设置解析对象为空
                                pt = ParserTypes.None;
                            }
                            // 清理缓存
                            sb.Clear();
                        } else if (pt == ParserTypes.Note) {
                            // 处在标签结尾
                            if (np.NodeType == NodeType.Declaration) {
                                // 结束申明
                                var npDeclaration = (DeclarationNode)np;
                                npDeclaration.Content = sb.ToString();
                                // 返回上一层
                                np = np.Parent;
                                // 过滤父对象
                                if (np == parent) np = null;
                                // 设置解析对象为空
                                pt = ParserTypes.None;
                            } else {
                                // 判断是否为注释结束
                                if (sb.Length >= 2) {
                                    if (sb[sb.Length - 1] == '-' && sb[sb.Length - 2] == '-') {
                                        // 结束注释
                                        var npNote = (NoteNode)np;
                                        sb.Remove(sb.Length - 2, 2);
                                        npNote.Note = sb.ToString();
                                        // 返回上层节点
                                        np = np.Parent;
                                        // 过滤父对象
                                        if (np == parent) np = null;
                                        // 设置解析
                                        pt = ParserTypes.None;
                                        // 清理缓存
                                        sb.Clear();
                                    } else {
                                        sb.Append(chr);
                                    }
                                } else {
                                    sb.Append(chr);
                                }
                            }
                        } else if (pt == ParserTypes.CData) {
                            // 判断是否为数据块结束
                            if (sb.Length >= 2) {

                                // 数据块模式，一般为style或是script
                                var npCData = (HtmlDataElement)np;
                                bool isCDataFinish = false;
                                int nameLen = npCData.TagName.Length;
                                int posStart = sb.Length - nameLen - 3;

                                if (sb.Length < npCData.TagName.Length + 2) {
                                    if (sb[posStart] != '<' && sb[posStart + 1] != '/') {

                                        bool check = true;

                                        for (int s = 0; s < nameLen; s++) {
                                            if (sb[posStart + 2 + s] != npCData.TagName[s]) {
                                                check = false;
                                                break;
                                            }
                                        }

                                        if (check) isCDataFinish = true;
                                    }
                                }

                                if (isCDataFinish) {
                                    sb.Remove(sb.Length - nameLen - 2, 2);
                                    npCData.Data = sb.ToString();
                                    // 返回上层节点
                                    np = np.Parent;
                                    // 过滤父对象
                                    if (np == parent) np = null;
                                    // 设置解析
                                    pt = ParserTypes.None;
                                    // 清理缓存
                                    sb.Clear();
                                } else {
                                    sb.Append(chr);
                                }
                            } else {
                                sb.Append(chr);
                            }
                        } else if (pt == ParserTypes.PropertyValue) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====斜杠=====]
                    case '/':
                        if (pt == ParserTypes.NodeName) {

                            // 设置为标签结尾
                            pt = ParserTypes.NodeFinish;

                            if (sb.Length > 0) {
                                // 新增子对象，并设置为独立标签
                                tagName = sb.ToString();
                                var npNormal = (HtmlElement)np;
                                var nodeNew = new HtmlElement(tagName);
                                nodeNew.IsSingle = true;
                                npNormal.Children.Add(nodeNew);
                                np = nodeNew;

                                // 清理缓存
                                tagName = null;
                                sb.Clear();
                            }
                        } else if (pt == ParserTypes.PropertyName) {
                            if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");
                            // 设置为结尾标签，并设置为独立标签
                            pt = ParserTypes.NodeFinish;
                            var npNormal = (HtmlElement)np;
                            npNormal.IsSingle = true;
                        } else if (pt == ParserTypes.PropertyValueFinish) {
                            // 设置为结尾标签，并设置为独立标签
                            pt = ParserTypes.NodeFinish;
                            var npNormal = (HtmlElement)np;
                            npNormal.IsSingle = true;
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note || pt == ParserTypes.None) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====空格=====]
                    case ' ':
                        if (pt == ParserTypes.NodeName) {
                            // 标签名称设定
                            tagName = sb.ToString();
                            if (tagName.IsEmpty()) throw new Exception($"规则外的字符'{chr}'");
                            if (tagName.ToUpper() == "!DOCTYPE") {
                                // 申明定义情况
                                if (parent != null) throw new Exception($"定义只允许在文档开始位置定义");
                                if (np != null) throw new Exception($"定义只允许在文档开始位置定义");
                                if (nodes.Count > 0) throw new Exception($"定义只允许在文档开始位置定义");
                                // 新增主对象
                                var node = new DeclarationNode();
                                nodes.Add(node);
                                np = node;
                                // 设置解析类型为属性名称
                                pt = ParserTypes.Note;
                                // 清理缓存
                                tagName = null;
                                sb.Clear();
                            } else {
                                if (tagName.ToLower() == "style" || tagName.ToLower() == "script") {
                                    // 判断是否存在处理对象
                                    if (np == null) {
                                        // 新增主对象
                                        var node = new HtmlDataElement(tagName);
                                        nodes.Add(node);
                                        np = node;
                                    } else {
                                        // 新增子对象
                                        var npNormal = (HtmlDataElement)np;
                                        var nodeNew = new HtmlDataElement(tagName);
                                        npNormal.Nodes.Add(nodeNew);
                                        np = nodeNew;
                                    }
                                } else if (tagName.ToLower() == "link" || tagName.ToLower() == "meta") {
                                    // 判断是否存在处理对象
                                    if (np == null) {
                                        // 新增主对象
                                        var node = new HtmlElement(tagName);
                                        nodes.Add(node);
                                    } else {
                                        // 新增子对象
                                        var npNormal = (HtmlElement)np;
                                        var nodeNew = new HtmlElement(tagName);
                                        npNormal.Nodes.Add(nodeNew);
                                    }
                                } else {
                                    // 判断是否存在处理对象
                                    if (np == null) {
                                        // 新增主对象
                                        var node = new HtmlElement(tagName);
                                        nodes.Add(node);
                                        np = node;
                                    } else {
                                        // 新增子对象
                                        var npNormal = (HtmlElement)np;
                                        var nodeNew = new HtmlElement(tagName);
                                        npNormal.Nodes.Add(nodeNew);
                                        np = nodeNew;
                                    }
                                }
                                // 设置解析类型为属性名称
                                pt = ParserTypes.PropertyName;
                                // 清理缓存
                                tagName = null;
                                sb.Clear();
                            }
                        } else if (pt == ParserTypes.PropertyName) {
                            // 判断是否为无值属性并处理
                            if (sb.Length > 0) {
                                pName = sb.ToString();
                                // 设置标签内容
                                if (np.NodeType == NodeType.Element) {
                                    var npNormal = (HtmlElement)np;
                                    npNormal.Attr[pName] = null;
                                    //npNormal.SetEncodeAttr(pName, pValue);
                                } else {
                                    throw new Exception($"语法错误");
                                }
                                // 清理缓存
                                sb.Clear();
                                pName = null;
                            }
                        } else if (pt == ParserTypes.PropertyValueFinish) {
                            // 设置解析类型为属性名称
                            pt = ParserTypes.PropertyName;
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else {
                            //throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====等号=====]
                    case '=':
                        if (pt == ParserTypes.PropertyName) {
                            // 填充名称
                            pName = sb.ToString();
                            // 设置为标签结尾
                            pt = ParserTypes.PropertyNameFinish;
                            // 清理缓存
                            sb.Clear();
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note || pt == ParserTypes.None) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====双引号=====]
                    case '"':
                        if (pt == ParserTypes.PropertyNameFinish) {
                            // 设置为标签值开始
                            pt = ParserTypes.PropertyValue;
                        } else if (pt == ParserTypes.PropertyValue) {
                            // 填充值
                            pValue = sb.ToString();
                            if (np.NodeType == NodeType.Element) {
                                var npNormal = (HtmlElement)np;
                                npNormal.Attr[pName] = pValue;
                                //npNormal.SetEncodeAttr(pName, pValue);
                            } else {
                                throw new Exception($"语法错误");
                            }
                            // 设置为标签值结束
                            pt = ParserTypes.PropertyValueFinish;
                            // 清理缓存
                            sb.Clear();
                            pName = null;
                            pValue = null;
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.Note || pt == ParserTypes.None) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====横杠=====]
                    case '-':
                        if (pt == ParserTypes.NodeName) {
                            // 判断是否为注释
                            if (sb.Length == 2) {
                                if (sb[0] == '!' && sb[1] == '-') {
                                    // 判断是否存在处理对象
                                    if (np == null) {
                                        // 新增主对象
                                        var node = new NoteNode();
                                        nodes.Add(node);
                                        np = node;
                                    } else {
                                        // 新增子对象
                                        var npNormal = (HtmlElement)np;
                                        var nodeNew = new NoteNode();
                                        npNormal.Nodes.Add(nodeNew);
                                        np = nodeNew;
                                    }
                                    // 设置解析模式为注释
                                    pt = ParserTypes.Note;
                                    // 清理缓存
                                    sb.Clear();
                                } else {
                                    sb.Append(chr);
                                }
                            } else {
                                sb.Append(chr);
                            }
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.Note || pt == ParserTypes.PropertyName || pt == ParserTypes.PropertyValue || pt == ParserTypes.None) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    case '\r': break;//忽略回车符
                    #region [=====换行符=====]
                    case '\n':
                        if (pt == ParserTypes.CData || pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else if (pt == ParserTypes.NodeName) {
                            // 当作空格使用
                            // 标签名称设定
                            tagName = sb.ToString();
                            if (tagName.IsEmpty()) throw new Exception($"规则外的字符'{chr}'");
                            // 判断是否存在处理对象
                            if (np == null) {
                                // 新增主对象
                                var node = new HtmlElement(tagName);
                                nodes.Add(node);
                                np = node;
                            } else {
                                // 新增子对象
                                var npNormal = (HtmlElement)np;
                                var nodeNew = new HtmlElement(tagName);
                                npNormal.Nodes.Add(nodeNew);
                                np = nodeNew;
                            }
                            // 设置解析类型为属性名称
                            pt = ParserTypes.PropertyName;
                            // 清理缓存
                            tagName = null;
                            sb.Clear();
                        } else if (pt == ParserTypes.PropertyValueFinish) {
                            // 设置解析类型为属性名称
                            pt = ParserTypes.PropertyName;
                        } else if (pt == ParserTypes.PropertyName || pt == ParserTypes.PropertyNameFinish || pt == ParserTypes.PropertyValue) {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        line++;
                        site = 0;
                        break;
                    #endregion
                    #region [=====常规字符=====]
                    default:

                        if (pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note || pt == ParserTypes.NodeName || pt == ParserTypes.PropertyName || pt == ParserTypes.NodeFinish || pt == ParserTypes.None) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                        #endregion
                }
            }

            if (pt != ParserTypes.None) throw new Exception($"内容尚未结束");
            if (sb.Length > 0) {
                var node = new TextNode();
                //node.Value = sb.ToString();
                node.SetEncodeValue(sb.ToString());
                nodes.Add(node);
                // 清理缓存
                sb.Clear();
            }

            return nodes;
        }

        /// <summary>
        /// 填充CSS内容
        /// </summary>
        /// <param name="css"></param>
        /// <param name="cssText"></param>
        public static void FillCss(HtmlCss css, string cssText) {

            // 当前解析器类型
            ParserTypes pt = ParserTypes.NodeName;

            // CSSS节点
            HtmlCssMedia cssMedia = null;

            // 缓存
            string pName = null;
            string pValue = null;
            StringBuilder sb = new StringBuilder();

            // 文档数据
            int line = 1;
            int site = 0;

            for (int i = 0; i < cssText.Length; i++) {
                site++;
                char chr = cssText[i];
                switch (chr) {
                    #region [=====左大括号=====]
                    case '{':
                        if (pt == ParserTypes.NodeName || pt == ParserTypes.NodeFinish) {
                            if (sb.Length <= 0) throw new Exception($"规则外的字符'{chr}'");
                            if (cssMedia != null) throw new Exception($"规则外的字符'{chr}'");
                            string name = sb.ToString().Trim();
                            cssMedia = new HtmlCssMedia(name);
                            css.Items.Add(cssMedia);

                            // 设置解析模式
                            pt = ParserTypes.PropertyName;
                            // 清理缓存
                            sb.Clear();
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====右大括号=====]
                    case '}':
                        if (pt == ParserTypes.PropertyName) {
                            if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");
                            if (cssMedia == null) throw new Exception($"规则外的字符'{chr}'");
                            cssMedia = null;
                            // 设置解析模式
                            pt = ParserTypes.NodeName;
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====冒号=====]
                    case ':':
                        if (pt == ParserTypes.PropertyName || pt == ParserTypes.PropertyNameFinish) {
                            if (sb.Length <= 0) throw new Exception($"规则外的字符'{chr}'");
                            if (cssMedia == null) throw new Exception($"规则外的字符'{chr}'");
                            pName = sb.ToString();
                            // 设置解析模式
                            pt = ParserTypes.PropertyValue;
                            // 清理缓存
                            sb.Clear();
                        } else if (pt == ParserTypes.NodeName || pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====分号=====]
                    case ';':
                        if (pt == ParserTypes.PropertyValue || pt == ParserTypes.PropertyValueFinish) {
                            if (pName.IsEmpty()) throw new Exception($"规则外的字符'{chr}'");
                            if (sb.Length <= 0) throw new Exception($"规则外的字符'{chr}'");
                            if (cssMedia == null) throw new Exception($"规则外的字符'{chr}'");
                            pValue = sb.ToString();

                            var unit = new HtmlCssItem() { Name = pName.Trim(), Content = pValue.Trim() };
                            cssMedia.Items.Add(unit);

                            // 设置解析模式
                            pt = ParserTypes.PropertyName;
                            // 清理缓存
                            pName = null;
                            pValue = null;
                            sb.Clear();
                        } else if (pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    #region [=====斜杠=====]
                    case '/':
                        if (pt == ParserTypes.Note) {
                            if (sb.Length <= 0) throw new Exception($"规则外的字符'{chr}'");
                            if (sb[0] != '*') throw new Exception($"规则外的字符'{chr}'");

                            if (sb.Length > 2) {
                                if (sb[sb.Length - 1] == '*') {
                                    string note = sb.ToString();
                                    var unit = new HtmlCssItem() { Name = "/", Content = note };

                                    if (cssMedia == null) {
                                        css.Items.Add(unit);

                                        // 设置解析模式
                                        pt = ParserTypes.NodeName;
                                    } else {
                                        cssMedia.Items.Add(unit);

                                        // 设置解析模式
                                        pt = ParserTypes.PropertyName;
                                    }

                                    // 清理缓存
                                    sb.Clear();
                                } else {
                                    sb.Append(chr);
                                }
                            }
                        } else {
                            if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");

                            // 设置解析模式
                            pt = ParserTypes.Note;
                        }
                        break;
                    #endregion
                    #region [=====空格=====]
                    case ' ':
                        if (pt == ParserTypes.NodeName || pt == ParserTypes.PropertyName || pt == ParserTypes.PropertyValue) {
                            if (sb.Length > 0) sb.Append(chr);
                        }
                        break;
                    #endregion
                    #region [=====回车换行=====]
                    case '\r':
                    case '\n':
                        if (pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else if (pt == ParserTypes.NodeName) {
                            if (sb.Length > 0) {
                                // 设置解析模式
                                pt = ParserTypes.NodeFinish;
                            }
                        } else if (pt == ParserTypes.PropertyName || pt == ParserTypes.PropertyValue) {
                            if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");
                        }
                        if (chr == '\n') {
                            line++;
                            site = 0;
                        }
                        break;
                    #endregion
                    default:
                        if (pt == ParserTypes.NodeFinish || pt == ParserTypes.PropertyNameFinish || pt == ParserTypes.PropertyValueFinish) {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        sb.Append(chr);
                        break;
                }
            }
        }

        /// <summary>
        /// 获取一个文档对象
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static HtmlDocument GetDocument(string xml) {
            HtmlDocument doc = new HtmlDocument(xml);
            return doc;
        }

    }

}
