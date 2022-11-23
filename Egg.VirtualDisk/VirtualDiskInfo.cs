using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 虚拟盘信息
    /// </summary>
    public class VirtualDiskInfo
    {

        /*
         * Sign             0x00    4
         * Version          0x04    4
         * FirstFolderBlock 0x10    4
         * FirstFolderIndex 0x14    1
         * FirstFileBlock   0x15    4
         * FirstFileIndex   0x19    1
         * LastPathBlock    0x20    4
         * LastPathIndex    0x24    1
         * IdleBlock        0x30    4
         */

        // 常量定义
        private const string Disk_Sign = "EVD";
        private const string Disk_Version_Newest = "1.0.0.0";
        private const string Security_Key = "JLNgKdwaWAHqNARhcu5e4cP7u5FxkU5PhbywjwvlXxa7vncRsjuabNRgt1cG9GEg";

        // 标志
        private const int Sign_Offset = 0x00;
        private const int Sign_Size = 4;
        // 版本
        private const int Version_Offset = Sign_Offset + Sign_Size;
        private const int Version_Size = 4;
        // 最后一个路径记录
        private const int Last_Path_Block_Offset = 0x10;
        private const int Last_Path_Block_Size = 4;
        private const int Last_Path_Index_Offset = Last_Path_Block_Offset + Last_Path_Block_Size;
        // 空闲区块号
        private const int Idle_Block_Offset = 0x20;
        private const int Idle_Block_Size = 4;
        // 已使用的最大索引值
        private const int Max_Id_Index_Offset = 0x30;
        private const int Max_Id_Index_Size = 4;
        // 存储128位密码
        private const int Password_Offset = 0x40;
        private const int Password_Size = 128;

        // 私有变量
        private byte[] _password;

        /// <summary>
        /// 信息标志
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 定义版本
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// 最后一个路径记录
        /// </summary>
        public VirtualPosition LastPath { get; set; }

        /// <summary>
        /// 空闲区块号
        /// </summary>
        public long IdleBlock { get; set; }

        /// <summary>
        /// 已使用的最大索引值
        /// </summary>
        public long MaxIdIndex { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public byte[] Key { get; private set; }

        /// <summary>
        /// 虚拟盘信息
        /// </summary>
        public VirtualDiskInfo(string pwd)
        {
            this.Sign = Disk_Sign;
            this.Version = new Version(Disk_Version_Newest);
            this.LastPath = new VirtualPosition();
            this.IdleBlock = 1;
            this.MaxIdIndex = 0;
            string passwordKey = pwd.GetSha512();
            _password = (passwordKey + Security_Key).GetSha512().ToBytes(Encoding.ASCII);
            this.Key = passwordKey.ToBytes(Encoding.ASCII);
        }

        /// <summary>
        /// 虚拟盘信息
        /// </summary>
        /// <param name="bytes"></param>
        /// <exception cref="VirtualDiskException"></exception>
        public VirtualDiskInfo(byte[] bytes, string pwd)
        {
            if (bytes.Length != Disk.Part_Size) throw new VirtualDiskException($"段信息长度应为'{Disk.Part_Size}'.");
            // 读取标志和版本号
            this.Sign = Encoding.ASCII.GetString(bytes, Sign_Offset, Sign_Size).Trim('\0');
            if (this.Sign != Disk_Sign) throw new VirtualDiskException($"文件不是有效的EVD文件.");
            this.Version = new Version(bytes[Version_Offset], bytes[Version_Offset + 1], bytes[Version_Offset + 2], bytes[Version_Offset + 3]);
            // 读取最后一个路径记录
            var lastPathBlock = new Span<byte>(bytes, Last_Path_Block_Offset, Last_Path_Block_Size).ToLong();
            var lastPathIndex = bytes[Last_Path_Index_Offset];
            this.LastPath = new VirtualPosition() { Block = lastPathBlock, Index = lastPathIndex };
            // 读取空闲区块号
            this.IdleBlock = new Span<byte>(bytes, Idle_Block_Offset, Idle_Block_Size).ToLong();
            // 读取已使用的最大索引值
            this.MaxIdIndex = new Span<byte>(bytes, Max_Id_Index_Offset, Max_Id_Index_Size).ToLong();
            // 密钥校验
            string passwordKey = pwd.GetSha512();
            string password = (passwordKey + Security_Key).GetSha512();
            _password = new Span<byte>(bytes, Password_Size, Password_Size).ToArray();
            if (_password.ToString(Encoding.ASCII) != password) throw new VirtualDiskException("访问密码校验失败");
            this.Key = passwordKey.ToBytes(Encoding.ASCII);
        }

        /// <summary>
        /// 转化为字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            byte[] bytes = new byte[Disk.Part_Size];
            // 生成标志
            byte[] bsSign = Encoding.ASCII.GetBytes(this.Sign);
            Array.Copy(bsSign, 0, bytes, Sign_Offset, bsSign.Length <= Sign_Size ? bsSign.Length : Sign_Size);
            // 生成版本
            bytes[Version_Offset] = (byte)this.Version.Major;
            bytes[Version_Offset + 1] = (byte)this.Version.Minor;
            bytes[Version_Offset + 2] = (byte)this.Version.Build;
            bytes[Version_Offset + 3] = (byte)this.Version.Revision;
            // 生成最后一个路径记录
            byte[] bsLastPathBlock = this.LastPath.Block.ToBytes(Last_Path_Block_Size);
            Array.Copy(bsLastPathBlock, 0, bytes, Last_Path_Block_Offset, bsLastPathBlock.Length);
            bytes[Last_Path_Index_Offset] = this.LastPath.Index;
            // 生成空闲区块号
            byte[] bsIdleBlock = this.IdleBlock.ToBytes(Idle_Block_Size);
            Array.Copy(bsIdleBlock, 0, bytes, Idle_Block_Offset, bsIdleBlock.Length);
            // 生成已使用的最大索引值
            byte[] bsMaxIdIndex = this.MaxIdIndex.ToBytes(Max_Id_Index_Size);
            Array.Copy(bsMaxIdIndex, 0, bytes, Max_Id_Index_Offset, bsMaxIdIndex.Length);
            // 存储密码
            Array.Copy(_password, 0, bytes, Password_Size, _password.Length);
            return bytes;
        }
    }
}
