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
    public class ScriptFunctionsBase : Dictionary<string, Func<ScriptVariables, object?>>, IDisposable, IScriptFunctionRegistr
    {
        /// <summary>
        /// 获取关联引擎
        /// </summary>
        public ScriptEngine Engine { get; private set; }

        /// <summary>
        /// 函数集合
        /// </summary>
        /// <exception cref="ScriptException"></exception>
        public ScriptFunctionsBase()
        {
            // 加
            this.Reg("+", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'+'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue += GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
            // 减
            this.Reg("-", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'-'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue -= GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
            // 乘
            this.Reg("*", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'*'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue *= GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
            // 除
            this.Reg("/", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'/'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue /= GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
        }

        /// <summary>
        /// 设置脚本引擎
        /// </summary>
        /// <param name="engine"></param>
        public void SetEngine(ScriptEngine engine)
        {
            this.Engine = engine;
        }

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
        public ScriptFunctionsBase Reg(string name, Func<ScriptVariables, object?> func)
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
        public ScriptFunctionsBase Reg(string name, Action<ScriptVariables> action)
        {
            this[name] = new Func<ScriptVariables, object?>(args => { action(args); return null; });
            return this;
        }

        /// <summary>
        /// 将类中的所有函数进行注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ScriptFunctionsBase Reg(string? name, System.Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var funcs = method.GetCustomAttributes<FuncAttribute>();
                foreach (var func in funcs)
                {
                    string? funName = null;
                    if (!func.Name.IsEmpty()) funName = func.Name;
                    if (string.IsNullOrWhiteSpace(funName))
                        funName = GetFunctionName(method.Name);
                    funName = (string.IsNullOrWhiteSpace(name) ? "" : name + "_") + funName;
                    var parameters = method.GetParameters();
                    Debug.WriteLine($"[Method] -> {method.ReturnParameter.Name} {funName}({parameters.Length})");
                    this[funName] = new Func<ScriptVariables, object?>(args =>
                    {
                        object?[] ps = new object[parameters.Length];
                        for (int i = 0; i < ps.Length; i++)
                        {
                            ps[i] = GetValue(this.Engine, args[i]);
                        }
                        var obj = Activator.CreateInstance(type);
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
        public ScriptFunctionsBase Reg<T>(string? name) where T : IScriptFunctionRegistr
        {
            return Reg(name, typeof(T));
        }

        /// <summary>
        /// 将类中的所有函数进行注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ScriptFunctionsBase Reg<T>() where T : IScriptFunctionRegistr
        {
            return Reg(null, typeof(T));
        }

        #region [=====静态函数=====]

        /// <summary>
        /// 执行对象
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="func"></param>
        /// <exception cref="ScriptException"></exception>
        public static void FuncInvoke(ScriptEngine engine, object? func)
        {
            // 为空跳出
            if (func is null) return;
            // 为函数则执行
            if (func is ScriptFunction)
            {
                ((ScriptFunction)func).Execute(engine);
                return;
            }
            // 抛出异常
            throw new ScriptException($"不可执行的类型'{func.GetType().FullName}'。");
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool isNumber(object? obj)
        {
            if (obj is null) return false;
            if (obj is byte) return true;
            if (obj is uint) return true;
            if (obj is int) return true;
            if (obj is ulong) return true;
            if (obj is long) return true;
            if (obj is float) return true;
            if (obj is double) return true;
            if (obj is decimal) return true;
            return false;
        }

        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object? GetVariableValue(ScriptEngine? engine, ScriptVariable obj)
        {
            // 处理特殊名称
            if (obj.Name == "null") return null;
            if (obj.Name == "true") return true;
            if (obj.Name == "false") return false;
            if (engine is null) return null;
            return engine.Memory.Get(obj.Name);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="engine"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetValue<T>(ScriptEngine? engine, object? obj) where T : struct
        {
            TryGetValue(engine, obj, out T? value);
            if (value is null) throw new Exception("对象值为空");
            return value.Value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="engine"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T? GetValueOrNull<T>(ScriptEngine? engine, object? obj) where T : class
        {
            TryGetValue(engine, obj, out T? value);
            return value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="engine"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<T>(ScriptEngine? engine, object? obj, out T? value) where T : struct
        {
            value = default(T);
            // 返回变量值
            if (obj is ScriptVariable)
            {
                var varValue = GetVariableValue(engine, (ScriptVariable)obj);
                if (varValue is null) return false;
                if (typeof(T) == typeof(double))
                {
                    value = (T)(object)Convert.ToDouble(varValue);
                    return true;
                }
                value = (T)varValue;
                return true;
            }
            // 如果引擎为空，则返回失败
            if (engine is null) return false;
            // 返回函数执行结果
            if (obj is ScriptFunction) obj = ((ScriptFunction)obj).Execute(engine);
            // 为空判断
            if (obj == null) throw new ScriptException($"变量值为空");
            if (typeof(T) == typeof(double))
            {
                value = (T)(object)Convert.ToDouble(obj);
                return true;
            }
            // 直接返回类型
            value = (T)obj;
            return true;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="engine"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<T>(ScriptEngine? engine, object? obj, out T? value) where T : class
        {
            value = null;
            try
            {
                // 返回变量值
                if (obj is ScriptVariable)
                {
                    var varValue = GetVariableValue(engine, (ScriptVariable)obj);
                    if (varValue is null) return false;
                    value = (T)varValue;
                    return true;
                }
                // 如果引擎为空，则返回失败
                if (engine is null) return false;
                // 返回函数执行结果
                if (obj is ScriptFunction) obj = ((ScriptFunction)obj).Execute(engine);
                // 为空判断
                if (obj == null) throw new ScriptException($"变量值为空");
                if (typeof(T) == typeof(double))
                {
                    value = (T)(object)Convert.ToDouble(obj);
                    return true;
                }
                // 直接返回类型
                value = (T)obj;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object? GetValue(ScriptEngine? engine, object? obj)
        {
            // 返回变量值
            if (obj is ScriptVariable) return GetVariableValue(engine, (ScriptVariable)obj);
            // 如果引擎为空，则返回空
            if (engine is null) return null;
            // 返回函数执行结果
            if (obj is ScriptFunction) return ((ScriptFunction)obj).Execute(engine);
            // 直接返回类型
            return obj;
        }

        #endregion

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
