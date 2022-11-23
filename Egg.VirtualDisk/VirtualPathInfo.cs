using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 路径信息
    /// </summary>
    public abstract class VirtualPathInfo
    {
        /// <summary>
        /// 位置信息
        /// </summary>
        public virtual VirtualPosition Position { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual VirtualPathType Type { get; set; }
        /// <summary>
        /// 路径ID
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// 所属目录ID
        /// </summary>
        public virtual long FolderId { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public virtual byte Attribute { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public virtual byte Access { get; set; }
        /// <summary>
        /// 下一个路径路径
        /// </summary>
        public virtual VirtualPosition NextPathPosition { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 获取是否为可用状态
        /// </summary>
        public bool Enable { get { return (this.Attribute & VirtualFileAttribute.VF_Enable) == VirtualFileAttribute.VF_Enable; } }
        /// <summary>
        /// 可读
        /// </summary>
        public virtual bool Readable { get { return (this.Attribute & VirtualFileAttribute.VF_Read) == VirtualFileAttribute.VF_Read; } }
        /// <summary>
        /// 可写
        /// </summary>
        public virtual bool Writeable { get { return (this.Attribute & VirtualFileAttribute.VF_Write) == VirtualFileAttribute.VF_Write; } }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public virtual bool IsHidden { get { return (this.Attribute & VirtualFileAttribute.VF_Hidden) == VirtualFileAttribute.VF_Hidden; } }

        /// <summary>
        /// 转化为字节数组
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToBytes() { throw new NotImplementedException(); }

        /// <summary>
        /// 获取磁盘信息
        /// </summary>
        protected VirtualDiskInfo DiskInfo { get; }

        /// <summary>
        /// 路径信息
        /// </summary>
        /// <param name="diskInfo"></param>
        public VirtualPathInfo(VirtualDiskInfo diskInfo)
        {
            this.DiskInfo = diskInfo;
        }
    }
}
