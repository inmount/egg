using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        /// 列表类型
        /// </summary>
        public const string TYPE_OBJECT = "System.Object";
        public const string TYPE_LIST = "System.Collections.Generic.List`1";
        public const string TYPE_DICTIONARY = "System.Collections.Generic.Dictionary`2";

        /// <summary>
        /// 判断是否为列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(Type type)
        {
            string fullName = type.Namespace + "." + type.Name;
            if (fullName == TYPE_LIST) return true;
            if (type.BaseType is null) return false;
            if (type.BaseType.FullName != TYPE_OBJECT) return IsList(type.BaseType);
            return false;
        }

        /// <summary>
        /// 判断是否为字典
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(Type type)
        {
            string fullName = type.Namespace + "." + type.Name;
            if (fullName == TYPE_DICTIONARY) return true;
            if (type.BaseType is null) return false;
            if (type.BaseType.FullName != TYPE_OBJECT) return IsDictionary(type.BaseType);
            return false;
        }

        /// <summary>
        /// 创建List
        /// </summary>
        /// <param name="type">泛型类型</param>
        /// <returns></returns>
        public static object CreateList(Type type)
        {
            //创建一个list返回
            return CreateList(type, new object[] { });
        }

        /// <summary>
        /// 创建List
        /// </summary>
        /// <param name="type">泛型类型</param>
        /// <returns></returns>
        public static object CreateList(Type type, object[] args)
        {
            Type listType = typeof(List<>);
            //指定泛型的具体类型
            Type newType = listType.MakeGenericType(new Type[] { type });
            //创建一个list返回
            return Activator.CreateInstance(newType, args);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ScriptException"></exception>
        public object? Get(string key)
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
                // 优先处理字段
                if (IsDictionary(objType))
                {
                    var objTypeItem = objType.GetProperty("Item");
                    return objTypeItem.GetValue(obj, new object[] { objPro });
                }
                var pro = objType.GetProperty(objPro, BindingFlags.Public | BindingFlags.Instance);
                if (pro is null) throw new ScriptException($"对象'{objName}'不存在'{objPro}'属性");
                return pro.GetValue(obj);
            }
            // 兼容数组
            int listIndex1 = key.IndexOf('[');
            int listIndex2 = key.IndexOf(']');
            if (listIndex1 > 0)
            {
                string listName = key.Substring(0, listIndex1);
                int listIndex = int.Parse(key.Substring(listIndex1 + 1, listIndex2 - listIndex1 - 1));
                var list = Get(listName);
                if (list is null) throw new ScriptException($"列表对象'{listName}'不存在");
                var listType = list.GetType();
                string listFullName = listType.Namespace + "." + listType.Name;
                if (listFullName != TYPE_LIST) throw new ScriptException($"变量'{listName}'不是列表");
                var listItem = listType.GetProperty("Item");
                return listItem.GetValue(list, new object[] { listIndex });
            }
            // 普通变量
            if (!this.ContainsKey(key)) return null;
            var value = this[key];
            return value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = Get(key);
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
                object? obj = Get(objName);
                if (obj is null) throw new ScriptException($"对象'{objName}'不存在");
                System.Type objType = obj.GetType();
                // 优先处理字段
                if (IsDictionary(objType))
                {
                    var objTypeItem = objType.GetProperty("Item");
                    objTypeItem.SetValue(obj, value, new object[] { objPro });
                    return;
                }
                string objFullName = objType.Namespace + "." + objType.Name;
                Debug.WriteLine(objFullName);
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
                var list = Get(listName);
                if (list is null) throw new ScriptException($"列表对象'{listName}'不存在");
                var listType = list.GetType();
                if (!IsList(listType)) throw new ScriptException($"变量'{listName}'不是列表");
                var listItem = listType.GetProperty("Item");
                var listAdd = listType.GetMethod("Add");
                var listCount = listType.GetProperty("Count");
                int listCountValue = (int)listCount.GetValue(list);
                for (int i = listCountValue - 1; i < listIndex; i++) listAdd.Invoke(list, new object?[] { null });
                listItem.SetValue(list, value, new object[] { listIndex });
                return;
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
