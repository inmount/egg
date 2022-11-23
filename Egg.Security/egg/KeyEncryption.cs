﻿using System;

namespace egg
{
    /// <summary>
    /// 密钥加密
    /// </summary>
    public static class KeyEncryption
    {
        /// <summary>
        /// Xor加密 - 重复加密即为解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void XorEncryption(byte[] key, ref byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key[i % key.Length]);
            }
        }
    }
}
