using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 路径类型
    /// </summary>
    public enum VirtualPathType : byte
    {
        /// <summary>
        /// 目录
        /// </summary>
        Folder = 0x1,
        /// <summary>
        /// 文件
        /// </summary>
        File = 0x2,
    }
}
