using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 存储管理器
    /// </summary>
    public class ScriptMemory : Dictionary<string, object?>, IDisposable
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (!this.ContainsKey(key)) throw new ScriptException($"变量'{key}'不存在");
            var value = this[key];
            if (value is null) throw new ScriptException($"变量'{key}'值为空");
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
