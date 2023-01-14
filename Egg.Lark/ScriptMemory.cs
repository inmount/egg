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
                var pro = objType.GetProperty(objPro, BindingFlags.Public | BindingFlags.Instance);
                if (pro is null) throw new ScriptException($"对象'{objName}'不存在'{objPro}'属性");
                var proValue = pro.GetValue(obj);
                if (typeof(T) == typeof(double)) return (T)(object)Convert.ToDouble(proValue);
                return (T)proValue;
            }
            // 兼容数组
            int listIndex1 = key.IndexOf('[');
            int listIndex2 = key.IndexOf(']');
            if (listIndex1 > 0)
            {
                string listName = key.Substring(0, listIndex1);
                int listIndex = int.Parse(key.Substring(listIndex1 + 1, listIndex2 - listIndex1 - 1));
                IList<object?> list = Get<IList<object?>>(listName);
                if (list is null) throw new ScriptException($"列表对象'{listName}'不存在");
                var lisyValue = list[listIndex];
                if (lisyValue is null) throw new ScriptException($"列表对象'{listName}'的第{listIndex}项为空。");
                if (typeof(T) == typeof(double)) return (T)(object)Convert.ToDouble(lisyValue);
                return (T)lisyValue;
            }
            // 普通变量
            if (!this.ContainsKey(key)) throw new ScriptException($"变量'{key}'不存在");
            var value = this[key];
            if (value is null) throw new ScriptException($"变量'{key}'值为空");
            if (typeof(T) == typeof(double)) return (T)(object)Convert.ToDouble(value);
            return (T)value;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ScriptException"></exception>
        public void Set(string key, object? value)
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
                var pro = objType.GetProperty(objPro, BindingFlags.Public | BindingFlags.Instance);
                if (pro is null) throw new ScriptException($"对象'{objName}'不存在'{objPro}'属性");
                pro.SetValue(obj, value ?? null);
            }
            // 兼容数组
            int listIndex1 = key.IndexOf('[');
            int listIndex2 = key.IndexOf(']');
            if (listIndex1 > 0)
            {
                string listName = key.Substring(0, listIndex1);
                int listIndex = int.Parse(key.Substring(listIndex1 + 1, listIndex2 - listIndex1 - 1));
                IList<object?> list = Get<IList<object?>>(listName);
                if (list is null) throw new ScriptException($"列表对象'{listName}'不存在");
                for (int i = list.Count - 1; i <= listIndex; i++) list.Add(null);
                list[listIndex] = value;
            }
            // 普通变量
            this[key] = value;
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
