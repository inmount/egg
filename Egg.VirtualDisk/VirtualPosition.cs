using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 虚拟地址
    /// </summary>
    public struct VirtualPosition
    {
        /// <summary>
        /// 区块号
        /// </summary>
        public long Block { get; set; }

        /// <summary>
        /// 索引号
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// 虚拟地址
        /// </summary>
        /// <param name="block"></param>
        /// <param name="index"></param>
        public VirtualPosition(long block, byte index)
        {
            Block = block;
            Index = index;
        }
    }
}
