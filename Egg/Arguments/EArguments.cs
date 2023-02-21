using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Egg.Arguments
{

    /// <summary>
    /// E格式参数集合
    /// 详细格式 --key:value
    /// </summary>
    public class EArguments : StringKeyValues, IArguments
    {

        /// <summary>
        /// 对象实例化
        /// </summary>
        public EArguments() { }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="args"></param>
        public EArguments(string[]? args = null)
        {
            this.SetParams(args);
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetParam(string key)
        {
            return this[key];
        }

        /// <summary>
        /// 设置命令行参数
        /// </summary>
        /// <param name="args"></param>
        public void SetParams(string[]? args)
        {
            // 获取参数
            for (int i = 0; i < (args?.Length ?? 0); i++)
            {
                string str = args?[i] ?? "";
                // 双横线开头为定义开始
                if (str.StartsWith("--"))
                {
                    // 冒号为内容定义
                    int idxSign = str.IndexOf(':');
                    if (idxSign > 0)
                    {
                        string sign = str.Substring(2, idxSign - 2);
                        string value = str.Substring(idxSign + 1);
                        if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length > 1) value = value.Substring(1, value.Length - 2);
                        this[sign] = value;
                    }
                    else
                    {
                        string sign = str.Substring(2);
                        this[sign] = "";
                    }
                }
            }
        }

        /// <summary>
        /// 设置命令行参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetParam(string key, string value)
        {
            this[key] = value;
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this)
            {
                if (sb.Length > 0) sb.Append(' ');
                if (item.Value.IsNullOrWhiteSpace())
                {
                    sb.Append("--");
                    sb.Append(item.Key);
                }
                else
                {
                    sb.Append("--");
                    sb.Append(item.Key);
                    sb.Append(":\"");
                    sb.Append(item.Value);
                    sb.Append("\"");
                }
            }
            return sb.ToString();
        }

    }
}
