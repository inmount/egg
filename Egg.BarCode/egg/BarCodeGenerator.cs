using Egg.BarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace egg
{
    /// <summary>
    /// 生成器
    /// </summary>
    public static class BarCodeGenerator
    {

        // 随机器
        private static Random? _rnd = null;

        // 随机器
        internal static Random Random { get { return _rnd ?? new Random(); } }

        // 获取关键字的值
        private static string GetKeyword(string key, IRuleCollection rules, long index, object? obj = null)
        {
            if (obj != null)
            {
                Type tp = obj.GetType();
                var pro = tp.GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
                if (pro != null) return pro.GetValue(obj).ToString();
            }
            if (rules.KeyRules.ContainsKey(key)) return rules.KeyRules[key].GetValue(index);
            throw new Exception($"未找到'{key}'的定义值");
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="sz"></param>
        /// <param name="rules"></param>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Generate(string sz, IRuleCollection rules, long index, object? obj = null)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder key = new StringBuilder();
            // 遍历所有文本
            for (int i = 0; i < sz.Length; i++)
            {
                char chr = sz[i];
                switch (chr)
                {
                    case '$':
                        if (key.Length == 0) { key.Append(chr); break; }
                        if (key.Length == 1 && key[0] == chr) { sb.Append(chr); key.Clear(); break; }
                        throw new Exception($"意外的'{chr}'字符");
                    case '(':
                        if (key.Length == 0) { sb.Append(chr); break; }
                        if (key.Length == 1 && key[0] == '$') { key.Append(chr); break; }
                        throw new Exception($"意外的'{chr}'字符");
                    case ')':
                        if (key.Length == 0) { sb.Append(chr); break; }
                        if (key.Length <= 2) { throw new Exception($"意外的'{chr}'字符"); }
                        string keyword = key.ToString(2, key.Length - 2);
                        sb.Append(GetKeyword(keyword, rules, index, obj));
                        key.Clear();
                        break;
                    default:
                        if (key.Length >= 2)
                        {
                            key.Append(chr);
                        }
                        else if (key.Length == 1)
                        {
                            throw new Exception($"意外的'{chr}'字符");
                        }
                        else
                        {
                            sb.Append(chr);
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Generate(IRules rules, long index, object? obj = null)
        {
            return rules.GetValue(index);
        }
    }
}
