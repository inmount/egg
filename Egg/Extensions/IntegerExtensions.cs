using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Egg
{

    /// <summary>
    /// 整型扩展类
    /// </summary>
    public static class IntegerExtensions
    {


        /// <summary>
        /// 整型转为字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this int value, int len = 4)
        {
            byte[] bytes = new byte[len];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(value >> (i * 8));
            }
            return bytes;
        }

        /// <summary>
        /// 字节数组转化为整型
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>

        public static int ToInteger(this byte[] bytes)
        {
            int len = bytes.Length <= 4 ? bytes.Length : 4;
            int result = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                result = result | ((int)bytes[i]) << (8 * i);
            }
            return result;
        }

        /// <summary>
        /// 字节数组转化为整型
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>

        public static int ToInteger(this Span<byte> bytes)
        {
            int len = bytes.Length <= 4 ? bytes.Length : 4;
            int result = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                result = result | ((int)bytes[i]) << (8 * i);
            }
            return result;
        }

    }
}
