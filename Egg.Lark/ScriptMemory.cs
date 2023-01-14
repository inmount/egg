using System;
using System.Collections.Generic;
using System.Reflection;
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
            if (key.IsEmpty()) throw new ScriptException($"变量名称不能为空");
            // 兼容对象
            int objIndex = key.IndexOf('.');
            if (objIndex > 0)
            {
                string objName = key.Substring(0, objIndex);
                string objPro = key.Substring(objIndex + 1);
                object? obj = Get<object>(objName);
                if (obj is null) throw new ScriptException($"对象'{objName}'不存在");
                System.Type objType = obj.GetType();
                var pro = objType.GetProperty(objName, BindingFlags.Public | BindingFlags.Instance);
            }
            if (!this.ContainsKey(key)) throw new ScriptException($"变量'{key}'不存在");
            var value = this[key];
            if (value is null) throw new ScriptException($"变量'{key}'值为空");
            if (typeof(T) == typeof(double)) return (T)(object)Convert.ToDouble(value);
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
