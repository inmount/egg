using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 目录信息
    /// </summary>
    public class VirtualFileInfo : VirtualPathInfo
    {

        // 常量定义
        private const int Id_Offset = 0x01;
        private const int Id_Size = 4;
        private const int Folder_Id_Offset = Id_Offset + Id_Size;
        private const int Folder_Id_Size = 4;
        private const int Attribute_Offset = Folder_Id_Offset + Folder_Id_Size;
        private const int Access_Offset = Attribute_Offset + 1;
        // 下一个记录的地址
        private const int Next_Path_Position_Block_Offset = 0x10;
        private const int Next_Path_Position_Block_Size = 4;
        private const int Next_Path_Position_Index_Offset = Next_Path_Position_Block_Offset + Next_Path_Position_Block_Size;
        // 数据块信息
        private const int Data_Block_Start_Offset = 0x20;
        private const int Data_Block_Start_Size = 4;
        private const int Data_Block_Count_Offset = Data_Block_Start_Offset + Data_Block_Start_Size;
        private const int Data_Block_Count_Size = 4;
        private const int Data_Length_Offset = Data_Block_Count_Offset + Data_Block_Count_Size;
        private const int Data_Length_Size = 6;
        // 名称
        private const int Name_Offset = 0x30;

        /// <summary>
        /// 数据开始区块号
        /// </summary>
        public long DataBlockStart { get; set; }
        /// <summary>
        /// 数据所占区块
        /// </summary>
        public long DataBlockCount { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public long DataLength { get; set; }

        /// <summary>
        /// 目录信息
        /// </summary>
        public VirtualFileInfo(VirtualDiskInfo diskInfo) : base(diskInfo)
        {
            // 存储位置
            this.Position = new VirtualPosition();
            // 类型
            this.Type = VirtualPathType.File;
            // Id
            this.Id = 0;
            // 所属目录Id
            this.FolderId = 0;
            // 所属目录Id
            this.Attribute = VirtualFileAttribute.VF_Enable | VirtualFileAttribute.VF_Read | VirtualFileAttribute.VF_Write;
            this.Access = 0;
            // 读取数据区块与数据长度
            this.DataBlockStart = 0;
            this.DataBlockCount = 0;
            this.DataLength = 0;
            // 下一个记录的地址
            this.NextPathPosition = new VirtualPosition();
            this.Name = "";
        }

        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="bs"></param>
        public VirtualFileInfo(VirtualDiskInfo diskInfo, byte[] bytes) : base(diskInfo)
        {
            if (bytes.Length != Disk.Part_Size) throw new VirtualDiskException($"段信息长度应为'{Disk.Part_Size}'.");
            // 存储位置
            this.Position = new VirtualPosition();
            // 类型
            if (bytes[0] != (byte)VirtualPathType.File) throw new VirtualDiskException($"段信息类型不为'{VirtualPathType.Folder.ToString()}'.");
            this.Type = VirtualPathType.File;
            // 读取Id
            this.Id = new Span<byte>(bytes, Id_Offset, Id_Size).ToLong();
            // 读取所属目录Id
            this.FolderId = new Span<byte>(bytes, Folder_Id_Offset, Folder_Id_Size).ToLong();
            // 读取属性和权限字段
            this.Attribute = bytes[Attribute_Offset];
            this.Access = bytes[Access_Offset];
            // 下一个记录的地址
            var nextPathPositionBlock = new Span<byte>(bytes, Next_Path_Position_Block_Offset, Next_Path_Position_Block_Size).ToLong();
            var nextPathPositionIndex = bytes[Next_Path_Position_Index_Offset];
            this.NextPathPosition = new VirtualPosition() { Block = nextPathPositionBlock, Index = nextPathPositionIndex };
            // 读取数据区块与数据长度
            this.DataBlockStart = new Span<byte>(bytes, Data_Block_Start_Offset, Data_Block_Start_Size).ToLong();
            this.DataBlockCount = new Span<byte>(bytes, Data_Block_Count_Offset, Data_Block_Count_Size).ToLong();
            this.DataLength = new Span<byte>(bytes, Data_Length_Offset, Data_Length_Size).ToLong();
            // 读取名称
            byte[] bsName = new Span<byte>(bytes, Name_Offset, Disk.Name_Size).ToArray();
            egg.Security.XorEncryption(this.DiskInfo.Key, ref bsName);
            this.Name = Encoding.UTF8.GetString(bsName).Trim('\0');
        }

        /// <summary>
        /// 转化为字节数组
        /// </summary>
        /// <returns></returns>
        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[Disk.Part_Size];
            // 生成类型
            bytes[0] = (byte)VirtualPathType.File;
            // 生成Id
            byte[] bsId = this.Id.ToBytes(Id_Size);
            Array.Copy(bsId, 0, bytes, Id_Offset, bsId.Length);
            // 生成所属目录Id
            byte[] bsFolderId = this.FolderId.ToBytes(Folder_Id_Size);
            Array.Copy(bsFolderId, 0, bytes, Folder_Id_Offset, bsFolderId.Length);
            // 生成属性和权限字段
            bytes[Attribute_Offset] = this.Attribute;
            bytes[Access_Offset] = this.Access;
            // 生成下一个记录的地址
            byte[] bsNextPathPositionBlock = this.NextPathPosition.Block.ToBytes(Next_Path_Position_Block_Size);
            Array.Copy(bsNextPathPositionBlock, 0, bytes, Next_Path_Position_Block_Offset, bsNextPathPositionBlock.Length);
            bytes[Next_Path_Position_Index_Offset] = this.NextPathPosition.Index;
            // 生成数据区块
            byte[] bsDataBlockStart = this.DataBlockStart.ToBytes(Data_Block_Start_Size);
            Array.Copy(bsDataBlockStart, 0, bytes, Data_Block_Start_Offset, bsDataBlockStart.Length);
            byte[] bsDataBlockCount = this.DataBlockCount.ToBytes(Data_Block_Count_Size);
            Array.Copy(bsDataBlockCount, 0, bytes, Data_Block_Count_Offset, bsDataBlockCount.Length);
            // 生成数据长度
            byte[] bsDataLength = this.DataLength.ToBytes(Data_Length_Size);
            Array.Copy(bsDataLength, 0, bytes, Data_Length_Offset, bsDataLength.Length);
            // 生成名称
            byte[] bsName = new byte[Disk.Name_Size];
            byte[] bsNameTemp = Encoding.UTF8.GetBytes(this.Name);
            if (bsNameTemp.Length > Disk.Name_Size) throw new VirtualDiskException($"名称所占字节'{bsName.Length}'超出规定长度'{Disk.Name_Size}'.");
            Array.Copy(bsNameTemp, 0, bsName, 0, bsNameTemp.Length);
            egg.Security.XorEncryption(this.DiskInfo.Key, ref bsName);
            Array.Copy(bsName, 0, bytes, Name_Offset, bsName.Length);
            return bytes;
        }
    }
}
