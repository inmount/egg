using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 类型
        /// </summary>
        public virtual VirtualPathType Type { get; }
        /// <summary>
        /// 可读
        /// </summary>
        public virtual bool Readable { get; }
        /// <summary>
        /// 可写
        /// </summary>
        public virtual bool Writeable { get; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public virtual bool IsHidden { get; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; }
        /// <summary>
        /// 长度
        /// </summary>
        public virtual long Length { get; }
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="info"></param>
        public FileInfo(VirtualFileInfo info)
        {
            Type = info.Type;
            Readable = info.Readable;
            Writeable = info.Writeable;
            IsHidden = info.IsHidden;
            Name = info.Name;
            Length = info.DataLength;
        }
    }
}
