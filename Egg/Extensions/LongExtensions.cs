using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Egg
{

    /// <summary>
    /// 长整型扩展类
    /// </summary>
    public static class LongExtensions
    {


        /// <summary>
        /// 长整型转为字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this long value, int len = 8)
        {
            byte[] bytes = new byte[len];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(value >> (i * 8));
            }
            return bytes;
        }

        /// <summary>
        /// 字节数组转化为长整型
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>

        public static long ToLong(this byte[] bytes)
        {
            int len = bytes.Length <= 8 ? bytes.Length : 8;
            long result = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                result = result | ((long)bytes[i]) << (8 * i);
            }
            return result;
        }

        /// <summary>
        /// 字节数组转化为长整型
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>

        public static long ToLong(this Span<byte> bytes)
        {
            int len = bytes.Length <= 8 ? bytes.Length : 8;
            long result = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                result = result | ((long)bytes[i]) << (8 * i);
            }
            return result;
        }

    }
}
