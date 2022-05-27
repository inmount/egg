using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Xml {

    /// <summary>
    /// 解析器
    /// </summary>
    public static class Parser {

        /// <summary>
        /// 解析器类型
        /// </summary>
        public enum ParserTypes {

            /// <summary>
            /// 无
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
        /// <param name="xml"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static XmlNodeCollection GetNodes(string xml, XmlNode parent = null) {

            // 初始化对象
            XmlNodeCollection nodes = new XmlNodeCollection(parent);
            BasicNode np = null;

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

            for (int i = 0; i < xml.Length; i++) {
                site++;
                char chr = xml[i];
                switch (chr) {
                    case '<':
                        #region [=====左尖括号=====]
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
                                    var npNormal = (XmlNode)np;
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
                    case '>':
                        #region [=====右尖括号=====]
                        if (pt == ParserTypes.NodeName) {
                            // 无属性情况
                            tagName = sb.ToString();
                            if (tagName.IsNone()) throw new Exception($"规则外的字符'{chr}'");
                            pt = ParserTypes.None;
                            // 判断是否存在处理对象
                            if (np == null) {
                                // 新增主对象
                                var node = new XmlNode(tagName);
                                nodes.Add(node);
                                np = node;
                            } else {
                                // 新增子对象
                                var npNormal = (XmlNode)np;
                                var nodeNew = new XmlNode(tagName);
                                npNormal.Nodes.Add(nodeNew);
                                np = nodeNew;
                            }
                            // 清理缓存
                            tagName = null;
                            sb.Clear();
                        } else if (pt == ParserTypes.NodeFinish) {
                            // 处在标签结尾
                            if (np.NodeType == NodeType.Declaration) {
                                if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");
                                // 返回上一层
                                np = np.Parent;
                                // 设置解析对象为空
                                pt = ParserTypes.None;
                            } else {
                                if (np.NodeType != NodeType.Normal) throw new Exception($"规则外的字符'{chr}'");
                                var npNormal = (XmlNode)np;
                                tagName = sb.ToString();
                                // 单节点模式
                                if (npNormal.IsSingle) {
                                    if (!tagName.IsNone()) throw new Exception($"语法错误");
                                    // 返回上一层
                                    np = np.Parent;
                                    // 过滤父对象
                                    if (np == parent) np = null;
                                } else {
                                    if (tagName.IsNone()) throw new Exception($"缺少标签名称");
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
                            }
                        } else if (pt == ParserTypes.PropertyValueFinish) {
                            // 设置解析对象为空
                            pt = ParserTypes.None;
                            // 清理缓存
                            sb.Clear();
                        } else if (pt == ParserTypes.PropertyName) {
                            if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");
                            // 设置解析对象为空
                            pt = ParserTypes.None;
                            // 清理缓存
                            sb.Clear();
                        } else if (pt == ParserTypes.Note) {
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
                        } else if (pt == ParserTypes.CData) {
                            // 判断是否为数据块结束
                            if (sb.Length >= 2) {
                                if (sb[sb.Length - 1] == ']' && sb[sb.Length - 2] == ']') {
                                    // 结束注释
                                    var npCData = (CDataNode)np;
                                    sb.Remove(sb.Length - 2, 2);
                                    npCData.Value = sb.ToString();
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
                    case '/':
                        #region [=====斜杠=====]
                        if (pt == ParserTypes.NodeName) {

                            // 设置为标签结尾
                            pt = ParserTypes.NodeFinish;

                            if (sb.Length > 0) {
                                // 新增子对象，并设置为独立标签
                                tagName = sb.ToString();
                                var npNormal = (XmlNode)np;
                                var nodeNew = new XmlNode(tagName);
                                nodeNew.IsSingle = true;
                                npNormal.Nodes.Add(nodeNew);
                                np = nodeNew;

                                // 清理缓存
                                tagName = null;
                                sb.Clear();
                            }
                        } else if (pt == ParserTypes.PropertyName) {
                            if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");
                            // 设置为结尾标签，并设置为独立标签
                            pt = ParserTypes.NodeFinish;
                            var npNormal = (XmlNode)np;
                            npNormal.IsSingle = true;
                        } else if (pt == ParserTypes.PropertyValueFinish) {
                            // 设置为结尾标签，并设置为独立标签
                            pt = ParserTypes.NodeFinish;
                            var npNormal = (XmlNode)np;
                            npNormal.IsSingle = true;
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note || pt == ParserTypes.None) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    case ' ':
                        #region [=====空格=====]
                        if (pt == ParserTypes.NodeName) {
                            // 标签名称设定
                            tagName = sb.ToString();
                            if (tagName.IsNone()) throw new Exception($"规则外的字符'{chr}'");
                            if (tagName == "?xml") {
                                // 申明定义情况
                                if (parent != null) throw new Exception($"定义只允许在文档开始位置定义");
                                if (np != null) throw new Exception($"定义只允许在文档开始位置定义");
                                if (nodes.Count > 0) throw new Exception($"定义只允许在文档开始位置定义");
                                // 新增主对象
                                var node = new DeclarationNode();
                                nodes.Add(node);
                                np = node;
                                // 设置解析类型为属性名称
                                pt = ParserTypes.PropertyName;
                                // 清理缓存
                                tagName = null;
                                sb.Clear();
                            } else {
                                // 判断是否存在处理对象
                                if (np == null) {
                                    // 新增主对象
                                    var node = new XmlNode(tagName);
                                    nodes.Add(node);
                                    np = node;
                                } else {
                                    // 新增子对象
                                    var npNormal = (XmlNode)np;
                                    var nodeNew = new XmlNode(tagName);
                                    npNormal.Nodes.Add(nodeNew);
                                    np = nodeNew;
                                }
                                // 设置解析类型为属性名称
                                pt = ParserTypes.PropertyName;
                                // 清理缓存
                                tagName = null;
                                sb.Clear();
                            }
                        } else if (pt == ParserTypes.PropertyValueFinish) {
                            // 设置解析类型为属性名称
                            pt = ParserTypes.PropertyName;
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else if (pt == ParserTypes.NodeFinish || pt == ParserTypes.None) {
                            if (sb.Length > 0) sb.Append(chr);
                        } else {
                            //throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    case '=':
                        #region [=====斜杠=====]
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
                    case '"':
                        #region [=====双引号=====]
                        if (pt == ParserTypes.PropertyNameFinish) {
                            // 设置为标签值开始
                            pt = ParserTypes.PropertyValue;
                        } else if (pt == ParserTypes.PropertyValue) {
                            // 填充值
                            pValue = sb.ToString();
                            if (np.NodeType == NodeType.Declaration) {
                                var npDeclaration = (DeclarationNode)np;
                                npDeclaration.Attr[pName] = pValue;
                            } else if (np.NodeType == NodeType.Normal) {
                                var npNormal = (XmlNode)np;
                                //npNormal.Attr[pName] = pValue;
                                npNormal.SetEncodeAttr(pName, pValue);
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
                    case '-':
                        #region [=====横杠=====]
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
                                        var npNormal = (XmlNode)np;
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
                    case '[':
                        #region [=====中括号=====]
                        if (pt == ParserTypes.None) {
                            //throw new Exception($"规则外的字符'{chr}'");
                            sb.Append(chr);
                        } else if (pt == ParserTypes.NodeName) {
                            // 判断是否为注释
                            if (sb.Length == 7) {
                                if (sb[0] == '!' && sb[1] == '[' && sb[2] == 'C' && sb[3] == 'D' && sb[4] == 'A' && sb[5] == 'T' && sb[6] == 'A') {
                                    // 判断是否存在处理对象
                                    if (np == null) {
                                        // 新增主对象
                                        var node = new CDataNode();
                                        nodes.Add(node);
                                        np = node;
                                    } else {
                                        // 新增子对象
                                        var npNormal = (XmlNode)np;
                                        var nodeNew = new CDataNode();
                                        npNormal.Nodes.Add(nodeNew);
                                        np = nodeNew;
                                    }
                                    // 设置解析模式为注释
                                    pt = ParserTypes.CData;
                                    // 清理缓存
                                    sb.Clear();
                                } else {
                                    sb.Append(chr);
                                }
                            } else {
                                sb.Append(chr);
                            }
                        } else if (pt == ParserTypes.CData || pt == ParserTypes.Note || pt == ParserTypes.PropertyName || pt == ParserTypes.PropertyValue) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    case '?':
                        #region [=====问号=====]
                        if (pt == ParserTypes.PropertyName) {
                            // 属性名称模式时，无属性定义继续
                            if (np.NodeType != NodeType.Declaration) throw new Exception($"规则外的字符'{chr}'");
                            if (sb.Length > 0) throw new Exception($"规则外的字符'{chr}'");
                            // 设置为标签结尾
                            pt = ParserTypes.NodeFinish;
                        } else if (pt == ParserTypes.PropertyValueFinish) {
                            // 设置为结尾标签，并设置为独立标签
                            if (np.NodeType != NodeType.Declaration) throw new Exception($"规则外的字符'{chr}'");
                            pt = ParserTypes.NodeFinish;
                        } else if (pt == ParserTypes.NodeName || pt == ParserTypes.CData || pt == ParserTypes.PropertyValue || pt == ParserTypes.Note || pt == ParserTypes.None) {
                            sb.Append(chr);
                        } else {
                            throw new Exception($"规则外的字符'{chr}'");
                        }
                        break;
                    #endregion
                    case '\r': break;//忽略回车符
                    case '\n':
                        #region [=====换行符=====]
                        if (pt == ParserTypes.CData || pt == ParserTypes.Note) {
                            sb.Append(chr);
                        } else if (pt == ParserTypes.NodeName) {
                            // 当作空格使用
                            // 标签名称设定
                            tagName = sb.ToString();
                            if (tagName.IsNone()) throw new Exception($"规则外的字符'{chr}'");
                            // 判断是否存在处理对象
                            if (np == null) {
                                // 新增主对象
                                var node = new XmlNode(tagName);
                                nodes.Add(node);
                                np = node;
                            } else {
                                // 新增子对象
                                var npNormal = (XmlNode)np;
                                var nodeNew = new XmlNode(tagName);
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
                    default:
                        #region [=====常规字符=====]
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
        /// 获取一个文档对象
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlDocument GetDocument(string xml) {
            XmlDocument doc = new XmlDocument(xml);
            return doc;
        }

    }
}
