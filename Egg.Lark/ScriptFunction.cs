using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
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
        /// 执行
        /// </summary>
        /// <returns></returns>
        public object? Execute(ScriptEngine engine)
        {
            return engine.Execute(this.Name, this.Parameters);
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
                if (ScriptFunctions.isNumber(p)) { sb.Append(Convert.ToDouble(p)); continue; }
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
