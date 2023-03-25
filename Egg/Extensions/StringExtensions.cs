using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Egg
{

    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// 以安全的方式获取长度
        /// </summary>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static int GetSize(this string? sz)
        {
            if (sz is null) return 0;
            if (sz.IsNullOrWhiteSpace()) return 0;
            return sz.Length;
        }

        /// <summary>
        /// 获取不为空的字符串
        /// </summary>
        /// <param name="sz"></param>
        /// <returns></returns>
        public static string ToNotNull(this string? sz)
        {
            return sz ?? string.Empty;
        }

        /// <summary>
        /// 是否内容为空
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string? sz)
        {
            return string.IsNullOrWhiteSpace(sz);
        }

        /// <summary>
        /// 是否内容为空
        /// </summary>
        public static bool IsNullOrEmpty(this string? sz)
        {
            return string.IsNullOrEmpty(sz);
        }

        /// <summary>
        /// 是否为一个双精度数字
        /// </summary>
        public static bool IsDouble(this string? sz)
        {
            double val = 0;
            if (double.TryParse(sz, out val))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取双精度数据
        /// </summary>
        public static double ToDouble(this string? sz)
        {
            double res = 0;
            double.TryParse(sz, out res);
            return res;
        }

        /// <summary>
        /// 是否为一个单精度数字
        /// </summary>
        public static bool IsFloat(this string? sz)
        {
            float val = 0;
            if (float.TryParse(sz, out val))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取单精度数据
        /// </summary>
        public static float ToFloat(this string? sz)
        {
            float res = 0;
            float.TryParse(sz, out res);
            return res;
        }

        /// <summary>
        /// 是否为一个字节型数字
        /// </summary>
        public static bool IsByte(this string? sz)
        {
            byte val = 0;
            if (byte.TryParse(sz, out val))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取字节型数据
        /// </summary>
        public static byte ToByte(this string? sz)
        {
            byte res = 0;
            byte.TryParse(sz, out res);
            return res;
        }

        /// <summary>
        /// 是否为一个整型数字
        /// </summary>
        public static bool IsInteger(this string? sz)
        {
            int val = 0;
            if (int.TryParse(sz, out val))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取整型数据
        /// </summary>
        public static int ToInteger(this string? sz)
        {
            int res = 0;
            int.TryParse(sz, out res);
            return res;
        }

        /// <summary>
        /// 是否为一个长整型数字
        /// </summary>
        public static bool IsLong(this string? sz)
        {
            long val = 0;
            if (long.TryParse(sz, out val))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取长整形数据
        /// </summary>
        public static long ToLong(this string? sz)
        {
            long res = 0;
            long.TryParse(sz, out res);
            return res;
        }

        /// <summary>
        /// 是否为真描述
        /// </summary>
        public static bool IsTrue(this string? sz)
        {
            if (sz is null) return false;
            // 数字情况下，大于0为真
            if (sz.IsDouble()) return sz.ToDouble() > 0;
            // 字符串情况下判断是否为true描述
            return sz.ToLower() == "true";
        }

        /// <summary>
        /// 转为UTF-8数组
        /// </summary>
        public static byte[] ToUtf8Bytes(this string? sz)
        {
            return sz.ToBytes(Encoding.UTF8);
        }

        /// <summary>
        /// 转为字节数组
        /// </summary>
        public static byte[] ToBytes(this string? sz, Encoding encoding)
        {
            if (sz.IsNullOrWhiteSpace()) return new byte[0];
            return encoding.GetBytes(sz);
        }

        /// <summary>
        /// 转为字节数组
        /// </summary>
        public static string? ToString(this byte[] bytes, Encoding encoding)
        {
            if (bytes is null) return null;
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 转为字节数组
        /// </summary>
        public static string? ToUtf8String(this byte[] bytes) => bytes.ToString(Encoding.UTF8);

        /// <summary>
        /// 是否为一个标准的IPv4字符串
        /// </summary>
        public static bool IsIPv4(this string? sz)
        {
            if (sz is null) return false;
            if (sz.IsNullOrWhiteSpace()) return false;
            string[] parts = sz.Split('.');
            if (parts.Length != 4) return false;

            for (int i = 0; i < parts.Length; i++)
            {
                if (!parts[i].IsInteger()) return false;
            }

            return true;
        }

        /// <summary>
        /// 使用Unicode进行字符串编码
        /// </summary>
        public static string GetUnicodeCoding(this string? sz)
        {
            if (sz is null) return "";
            if (sz.IsNullOrWhiteSpace()) return "";
            char[] charbuffers = sz.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(System.String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 使用Unicode进行字符串解码
        /// </summary>
        public static string GetUnicodeDecoding(this string? sz)
        {
            StringBuilder sb = new StringBuilder();
            string src = sz ?? "";
            int len = sz.GetSize() / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                sb.Append(Encoding.Unicode.GetString(bytes));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        public static string GetMD5(this string? sz)
        {
            System.Text.Encoding MyEnc = System.Text.Encoding.UTF8;
            Byte[] StrByte = MyEnc.GetBytes(sz);

            MD5CryptoServiceProvider TheMD5 = new MD5CryptoServiceProvider();
            Byte[] MD5byte = TheMD5.ComputeHash(StrByte);

            string MD5Str = "";

            for (int i = 0; i < MD5byte.Length; i++)
            {
                MD5Str += MD5byte[i].ToString("X").PadLeft(2, '0');
            }

            return MD5Str;
        }

        /// <summary>
        /// 获取字符串的sha1值
        /// </summary>
        public static string GetSha1(this string? sz)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(sz);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out).Replace("-", "").ToLower();
            //str_sha1_out = str_sha1_out.Replace("-", "");
            return str_sha1_out;
        }

        /// <summary>
        /// 获取字符串的sha256值
        /// </summary>
        public static string GetSha256(this string? sz)
        {
            var sha = new SHA256CryptoServiceProvider();
            byte[] bytes_in = UTF8Encoding.Default.GetBytes(sz);
            byte[] bytes_out = sha.ComputeHash(bytes_in);
            string str_out = BitConverter.ToString(bytes_out).Replace("-", "").ToLower();
            //str_sha1_out = str_sha1_out.Replace("-", "");
            return str_out;
        }

        /// <summary>
        /// 获取字符串的sha512值
        /// </summary>
        public static string GetSha512(this string? sz)
        {
            var sha = new SHA512CryptoServiceProvider();
            byte[] bytes_in = UTF8Encoding.Default.GetBytes(sz);
            byte[] bytes_out = sha.ComputeHash(bytes_in);
            string str_out = BitConverter.ToString(bytes_out).Replace("-", "").ToLower();
            //str_sha1_out = str_sha1_out.Replace("-", "");
            return str_out;
        }

        /*
         2019.11.01 添加
         */

        // 比较两个字符串大小
        private static int SortCompare(string? s1, string? s2)
        {
            if (s1 is null)
            {
                if (s2 is null) return 0;
                return -1;
            }
            if (s2 is null) return 1;
            int len1 = s1.GetSize();
            int len2 = s2.GetSize();
            int len = len1;
            if (len < len2) len = len2;
            for (int i = 0; i < len; i++)
            {
                if (i >= len1) return -1;
                if (i >= len2) return 1;
                if (s1[i] > s2[i]) return 1;
                if (s1[i] < s2[i]) return -1;
            }
            return 0;
        }

        /// <summary>
        /// 是否对于指定字符串排序靠前
        /// </summary>
        /// <param name="sz"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool SortBefore(this string? sz, string? str)
        {
            return SortCompare(sz, str) < 0;
        }

        /// <summary>
        /// 是否对于指定字符串排序靠后
        /// </summary>
        /// <param name="sz"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool SortAfter(this string? sz, string? str)
        {
            return SortCompare(sz, str) > 0;
        }

    }
}
