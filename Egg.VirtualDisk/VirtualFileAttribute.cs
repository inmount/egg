using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 文件属性
    /// </summary>
    public class VirtualFileAttribute
    {
        /// <summary>
        /// 有效
        /// </summary>
        public const byte VF_Enable = 0x1;
        /// <summary>
        /// 可读
        /// </summary>
        public const byte VF_Read = 0x2;
        /// <summary>
        /// 可写
        /// </summary>
        public const byte VF_Write = 0x4;
        /// <summary>
        /// 隐藏
        /// </summary>
        public const byte VF_Hidden = 0x8;
    }
}
