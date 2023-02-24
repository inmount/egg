using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Egg
{

    /// <summary>
    /// 对象扩展类
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object? obj)
        {
            return obj is null;
        }

        /// <summary>
        /// 获取不为空的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="defaultObj"></param>
        /// <returns></returns>
        public static T ToNotNull<T>(this object? obj, T? defaultObj) where T : struct
        {
            return (T)(obj ?? (defaultObj ?? default(T)));
        }

        /// <summary>
        /// 获取不为空的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="defaultObj"></param>
        /// <returns></returns>
        public static T ToNotNull<T>(this object? obj, T defaultObj) where T : class
        {
            return (T)(obj ?? (defaultObj));
        }

        /// <summary>
        /// 获取不为空的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToNotNull<T>(this object? obj) where T : new()
        {
            if (obj is null) return new T();
            return (T)obj;
        }

        /// <summary>
        /// 转化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T? ConvertOrNull<T>(this object? obj) where T : class
        {
            if (obj is null) return default(T);
            return obj.ConvertTo<T>();
        }

        /// <summary>
        /// 转化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object obj)
        {
            var typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Boolean: return (T)(object)Convert.ToBoolean(obj);
                case TypeCode.Byte: return (T)(object)Convert.ToByte(obj);
                case TypeCode.Int16: return (T)(object)Convert.ToInt16(obj);
                case TypeCode.Int32: return (T)(object)Convert.ToInt32(obj);
                case TypeCode.Int64: return (T)(object)Convert.ToInt64(obj);
                case TypeCode.UInt16: return (T)(object)Convert.ToUInt16(obj);
                case TypeCode.UInt32: return (T)(object)Convert.ToUInt32(obj);
                case TypeCode.UInt64: return (T)(object)Convert.ToUInt64(obj);
                case TypeCode.Single: return (T)(object)Convert.ToSingle(obj);
                case TypeCode.Double: return (T)(object)Convert.ToDouble(obj);
                case TypeCode.Char: return (T)(object)Convert.ToChar(obj);
                case TypeCode.Decimal: return (T)(object)Convert.ToDecimal(obj);
                case TypeCode.DateTime: return (T)(object)Convert.ToDateTime(obj);
                default: return (T)obj;
            }
        }

    }
}
