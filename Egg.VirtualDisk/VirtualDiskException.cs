using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 虚拟盘错误
    /// </summary>
    public class VirtualDiskException : System.Exception
    {
        public VirtualDiskException(string message) : base(message) { }
    }
}
