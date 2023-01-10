using System;
using System.Collections.Generic;

namespace Egg.Lark
{
    /// <summary>
    /// 变量集合
    /// </summary>
    public class ScriptVariables : List<object?>, IDisposable
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Get<T>(int index)
        {
            var value = this[index];
            if (value is null) throw new ScriptException($"索引'{index}'值为空");
            return (T)value;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
