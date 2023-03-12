using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Egg
{

    /// <summary>
    /// 类型扩展类
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取外层类型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetTopName(this Type? type)
        {
            if (type is null) return "System.Null";
            return type.Namespace + "." + type.Name;
        }

        /// <summary>
        /// 获取是否为可空类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsNullable(this Type? type)
        {
            if (type is null) return true;
            return type.GetTopName() == "System.Nullable";
        }

        /// <summary>
        /// 判断是否为数值类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(this Type? type)
        {
            if (type is null) return false;
            string topName = type.GetTopName();
            switch (topName)
            {
                case "System.Byte":
                case "System.SByte":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Boolean":
                    return true;
                default:
                    if (type.BaseType != null) return type.BaseType.IsNumeric();
                    return false;
            }
        }

        /// <summary>
        /// 判断是否包含接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeInterface"></param>
        /// <returns></returns>
        public static bool HasInterface(this Type? type, Type typeInterface)
        {
            if (type is null) return false;
            if (!typeInterface.IsInterface) throw new Exception($"'{typeInterface.Name}'不是一个有效的接口");
            var ifs = type.GetInterfaces();
            foreach (var ifc in ifs)
            {
                if (ifc.Equals(typeInterface)) return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否包含接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasInterface<T>(this Type? type)
        {
            return type.HasInterface(typeof(T));
        }
    }
}
