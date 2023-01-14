using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Egg.Lark
{
    /// <summary>
    /// 函数集合
    /// </summary>
    public class ScriptFunctions : Dictionary<string, Func<ScriptVariables, object?>>, IDisposable
    {
        // 获取函数名称
        private string GetFunctionName(string name)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < name.GetSize(); i++)
            {
                char chr = name[i];
                if (chr >= 'A' && chr <= 'Z')
                {
                    if (i == 0)
                    {
                        sb.Append(chr.ToString().ToLower());
                    }
                    else
                    {
                        sb.Append('_');
                        sb.Append(chr.ToString().ToLower());
                    }
                }
                else
                {
                    sb.Append(chr);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public ScriptFunctions Reg(string name, Func<ScriptVariables, object> func)
        {
            this[name] = func;
            return this;
        }

        /// <summary>
        /// 注册函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public ScriptFunctions Reg(string name, Action<ScriptVariables> action)
        {
            this[name] = new Func<ScriptVariables, object?>((args) => { action(args); return null; });
            return this;
        }

        /// <summary>
        /// 将类中的所有函数进行注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ScriptFunctions Reg(string? name, System.Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var func = method.GetCustomAttribute<FuncAttribute>();
                if (func != null)
                {
                    string? funName = null;
                    if (!func.Name.IsEmpty()) funName = func.Name;
                    if (string.IsNullOrWhiteSpace(funName))
                        funName = GetFunctionName(method.Name);
                    funName = (string.IsNullOrWhiteSpace(name) ? "" : name + "_") + funName;
                    var parameters = method.GetParameters();
                    Debug.WriteLine($"[Method] -> {method.ReturnParameter.Name} {funName}({parameters.Length})");
                    this[funName] = new Func<ScriptVariables, object?>((args) =>
                    {
                        object?[] ps = new object[parameters.Length];
                        for (int i = 0; i < ps.Length; i++)
                        {
                            ps[i] = args[i];
                        }
                        var obj = type.Assembly.CreateInstance(type.FullName);
                        return method.Invoke(obj, ps);
                    });
                }
            }
            return this;
        }

        /// <summary>
        /// 将类中的所有函数进行注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ScriptFunctions Reg<T>(string? name) where T : class
        {
            return Reg(name, typeof(T));
        }

        /// <summary>
        /// 将类中的所有函数进行注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ScriptFunctions Reg<T>() where T : class
        {
            return Reg(null, typeof(T));
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
