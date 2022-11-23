using egg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Egg.VirtualDisk
{
    /// <summary>
    /// 虚拟盘
    /// </summary>
    public class Disk : IDisposable
    {
        // 常量定义
        public const int Page_Size = 0x1000;
        public const int Part_Size = 0x200;
        public const int Part_Count = Page_Size / Part_Size;
        public const int Name_Size = 0x1B0;

        // 私有变量
        private List<VirtualPathInfo> _paths;
        private static object obj = new object();

        /// <summary>
        /// 磁盘文件存储路径
        /// </summary>
        public string StoragePath { get; }

        /// <summary>
        /// 磁盘信息
        /// </summary>
        public VirtualDiskInfo DiskInfo { get; private set; }

        /// <summary>
        /// 磁盘文件流
        /// </summary>
        public FileStream FileStream { get; private set; }

        /// <summary>
        /// 保存磁盘信息
        /// </summary>
        public void SaveDiskInfo()
        {
            if (this.FileStream is null) throw new VirtualDiskException("文件流不可用.");
            var f = this.FileStream;
            lock (obj)
            {
                f.Position = 0;
                f.Write(this.DiskInfo.ToBytes());
            }
        }

        #region [=====加载方法=====]

        // 加载磁盘信息
        private void LoadDiskInfo(string pwd)
        {
            if (this.DiskInfo != null) return;
            string path = this.StoragePath;
            if (egg.IO.CheckFileExists(path))
            {
                var f = this.FileStream = egg.IO.OpenFile(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                // 读取磁盘信息
                f.Position = 0;
                byte[] bsDiskInfo = new byte[Part_Size];
                f.Read(bsDiskInfo, 0, Part_Size);
                this.DiskInfo = new VirtualDiskInfo(bsDiskInfo, pwd);
            }
            else
            {
                // 写入基础信息
                var f = this.FileStream = egg.IO.OpenFile(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
                this.DiskInfo = new VirtualDiskInfo(pwd);
                this.SaveDiskInfo();
            }
        }

        // 加载路径集合
        private void LoadPaths()
        {
            if (_paths is null) _paths = new List<VirtualPathInfo>();
            _paths.Clear();
            // 当空闲区块不大于1，则磁盘尚未存储任何数据
            if (this.DiskInfo.IdleBlock <= 1) return;
            // 循环读取所有的路径信息
            VirtualPosition pos = new VirtualPosition(1, 0);
            while (pos.Block > 0)
            {
                var path = GetPathInfo(pos);
                _paths.Add(path);
                pos = path.NextPathPosition;
            }
        }

        #endregion

        #region [=====操作路径=====]

        // 保存路径信息
        private void SavePathInfo(VirtualPathInfo info)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            // 保存路径信息
            lock (obj)
            {
                f.Position = GetVirtualPositionStart(info.Position);
                f.Write(info.ToBytes());
            }
        }

        // 设置路径信息为删除
        private void DeletePath(VirtualPathInfo info)
        {
            // 设置删除标记
            info.Attribute = (Byte)(info.Attribute & ~VirtualFileAttribute.VF_Enable);
            // 保存删除
            SavePathInfo(info);
        }

        // 设置路径信息为删除
        private void DeleteFolderWithChildren(VirtualFolderInfo info)
        {
            // 删除所有目录下的文件
            var files = _paths.Where(
                d => d.FolderId == info.Id
                    && d.Enable
                    && d.Type == VirtualPathType.File
                    );
            foreach (var file in files)
            {
                DeletePath(file);
            }
            // 删除所有目录下的文件夹
            var folders = _paths.Where(
                d => d.FolderId == info.Id
                    && d.Enable
                    && d.Type == VirtualPathType.Folder
                    );
            foreach (var folder in folders)
            {
                DeleteFolderWithChildren((VirtualFolderInfo)folder);
            }
            // 删除文件夹
            DeletePath(info);
        }

        // 添加一个目录
        private VirtualFolderInfo AddFolder(long folderId, string name)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            if (name.IsEmpty()) throw new VirtualDiskException("名称不能为空.");
            // 判断路径是否已经存在
            if (_paths.Where(d => d.FolderId == folderId && d.Name == name && d.Enable).Any())
                throw new VirtualDiskException($"名称'{name}'已经存在.");
            // 新建信息
            VirtualFolderInfo info = new VirtualFolderInfo(this.DiskInfo) { FolderId = folderId, Name = name };
            // 定位新目录并处理磁盘信息
            var disk = this.DiskInfo;
            var pos = this.DiskInfo.LastPath;
            // 上一条路径记录
            VirtualPathInfo? beforePath = null;
            // 判断是否为第一条信息
            if (pos.Block < 1)
            {
                pos.Block = disk.IdleBlock;
                pos.Index = 0;
                disk.IdleBlock++;
            }
            else
            {
                // 获取最后一条路径记录
                beforePath = GetPathInfo(pos);
                pos.Index++;
                if (pos.Index >= Part_Count)
                {
                    pos.Block = disk.IdleBlock;
                    pos.Index = 0;
                    disk.IdleBlock++;
                }
                // 填充新地址到上一条路径
                beforePath.NextPathPosition = pos;
            }
            // 设置Id及位置信息
            disk.MaxIdIndex++;
            info.Id = disk.MaxIdIndex;
            info.Position = pos;
            disk.LastPath = pos;
            // 保存路径信息
            SavePathInfo(info);
            // 保存上一条路径信息
            if (beforePath != null)
            {
                // 写入硬件
                SavePathInfo(beforePath);
                // 更新缓存
                _paths.Find(d => d.Id == beforePath.Id).NextPathPosition = beforePath.NextPathPosition;
            }
            // 保存磁盘信息
            SaveDiskInfo();
            // 添加到路径列表
            _paths.Add(info);
            return info;
        }

        // 添加一个文件
        private VirtualFileInfo AddFile(long folderId, string name)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            if (name.IsEmpty()) throw new VirtualDiskException("名称不能为空.");
            // 判断路径是否已经存在
            if (_paths.Where(d => d.FolderId == folderId && d.Name == name && d.Enable).Any())
                throw new VirtualDiskException($"名称'{name}'已经存在.");
            // 新建信息
            VirtualFileInfo info = new VirtualFileInfo(this.DiskInfo) { FolderId = folderId, Name = name };
            // 定位新目录并处理磁盘信息
            var disk = this.DiskInfo;
            var pos = this.DiskInfo.LastPath;
            // 上一条路径记录
            VirtualPathInfo? beforePath = null;
            // 判断是否为第一条信息
            if (pos.Block < 1)
            {
                pos.Block = disk.IdleBlock;
                pos.Index = 0;
                disk.IdleBlock++;
            }
            else
            {
                // 获取最后一条路径记录
                beforePath = GetPathInfo(pos);
                pos.Index++;
                if (pos.Index >= Part_Count)
                {
                    pos.Block = disk.IdleBlock;
                    pos.Index = 0;
                    disk.IdleBlock++;
                }
                // 填充新地址到上一条路径
                beforePath.NextPathPosition = pos;
            }
            // 设置Id及位置信息
            disk.MaxIdIndex++;
            info.Id = disk.MaxIdIndex;
            info.Position = pos;
            disk.LastPath = pos;
            // 保存路径信息
            SavePathInfo(info);
            // 保存上一条路径信息
            if (beforePath != null)
            {
                // 写入硬件
                SavePathInfo(beforePath);
                // 更新缓存
                _paths.Find(d => d.Id == beforePath.Id).NextPathPosition = beforePath.NextPathPosition;
            }
            // 保存磁盘信息
            SaveDiskInfo();
            // 添加到路径列表
            _paths.Add(info);
            return info;
        }

        // 添加一个文件
        private VirtualFileInfo CreateOrGetFile(string path)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            // 获取标准路径
            path = GetStandardPath(path);
            if (path.Length < 2) throw new VirtualDiskException("尚未指定路径.");
            // 目录信息
            string[] paths = path.Substring(1).Split('/');
            // 获取根目录
            long folderId = 0;
            // 遍历所有子目录
            for (int i = 0; i < paths.Length - 1; i++)
            {
                // 判断路径是否存在
                VirtualFolderInfo? info = GetChildFolderInfo(folderId, paths[i]);
                if (info is null) throw new VirtualDiskException($"路径'{path}'的目录信息不存在.");
                folderId = info.Id;
            }
            // 添加文件
            string fileName = paths[paths.Length - 1];
            var fileInfo = GetChildFileInfo(folderId, fileName);
            if (fileInfo is null)
            {
                // 添加文件信息并获取
                fileInfo = AddFile(folderId, fileName);
            }
            return fileInfo;
        }

        #endregion

        #region [=====获取路径=====]

        // 获取标准路径
        private string GetStandardPath(string path)
        {
            // 判断是否为空
            if (path.IsEmpty()) throw new VirtualDiskException("路径不能为空.");
            // 将反斜杠转化为斜杠
            path = path.Replace('\\', '/');
            if (path[0] != '/') throw new VirtualDiskException("路径必须以'/'开头.");
            // 去重
            while (path.IndexOf("//") >= 0) path.Replace("//", "/");
            // 去尾
            if (path.Length > 1 && path.EndsWith('/')) path = path.Substring(0, path.Length - 1);
            return path;
        }

        // 获取父路径
        private string GetParentPath(string path)
        {
            // 获取标准路径
            path = GetStandardPath(path);
            if (path.Length < 2) throw new VirtualDiskException("根目录无法获取父路径");
            StringBuilder sb = new StringBuilder();
            // 目录信息
            string[] paths = path.Substring(1).Split('/');
            // 拼接目录
            for (int i = 0; i < paths.Length - 1; i++)
            {
                sb.Append('/');
                sb.Append(paths[i]);
            }
            return sb.ToString();
        }

        // 获取名称
        private string GetName(string path)
        {
            // 获取标准路径
            path = GetStandardPath(path);
            if (path.Length < 2) throw new VirtualDiskException("根目录无法获取名称");
            StringBuilder sb = new StringBuilder();
            // 目录信息
            string[] paths = path.Split('/');
            return paths.Last();
        }

        // 获取开始位置
        private long GetVirtualPositionStart(VirtualPosition pos)
        {
            return pos.Block * Page_Size + pos.Index * Part_Size;
        }

        // 获取路径信息
        private VirtualPathInfo GetPathInfo(VirtualPosition pos)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            // 读取数据块
            byte[] bs = new byte[Part_Size];
            f.Position = GetVirtualPositionStart(pos);
            f.Read(bs, 0, bs.Length);
            switch (bs[0])
            {
                case (byte)VirtualPathType.Folder:
                    return new VirtualFolderInfo(this.DiskInfo, bs) { Position = pos };
                case (byte)VirtualPathType.File:
                    return new VirtualFileInfo(this.DiskInfo, bs) { Position = pos };
                default: throw new VirtualDiskException($"不支持的路径类型'0x{bs[0].ToString("x2")}'.");
            }
        }

        // 获取目录信息
        private VirtualFolderInfo? GetVirtualFolderInfo(string path)
        {
            // 获取标准路径
            path = GetStandardPath(path);
            if (path.Length < 2) throw new VirtualDiskException("尚未指定路径.");
            // 目录信息
            string[] paths = path.Substring(1).Split('/');
            // 目录id
            VirtualFolderInfo? info = GetChildFolderInfo(0, paths[0]);
            if (info is null) return null;
            // 遍历所有子目录
            for (int i = 1; i < paths.Length; i++)
            {
                if (info != null) info = GetChildFolderInfo(info.Id, paths[i]);
                if (info is null) return null;
            }
            return info;
        }

        // 获取目录信息
        private VirtualFileInfo? GetVirtualFileInfo(string path)
        {
            // 获取标准路径
            path = GetStandardPath(path);
            if (path.Length < 2) throw new VirtualDiskException("尚未指定路径.");
            // 目录信息
            string[] paths = path.Substring(1).Split('/');
            // 当在根目录下时，直接返回结果
            if (paths.Length == 1) return GetChildFileInfo(0, paths[0]);
            // 目录id
            VirtualFolderInfo? info = GetChildFolderInfo(0, paths[0]);
            if (info is null) return null;
            // 遍历所有子目录
            for (int i = 1; i < paths.Length - 1; i++)
            {
                if (info != null) info = GetChildFolderInfo(info.Id, paths[i]);
                if (info is null) return null;
            }
            return GetChildFileInfo(info.Id, paths[paths.Length - 1]);
        }

        // 获取子目录信息
        private VirtualFolderInfo? GetChildFolderInfo(long folderId, string name)
        {
            return (VirtualFolderInfo?)_paths.Where(
                d => d.FolderId == folderId
                    && d.Name == name
                    && d.Enable
                    && d.Type == VirtualPathType.Folder
                    ).FirstOrDefault();
        }

        // 获取文件信息
        private VirtualFileInfo? GetChildFileInfo(long folderId, string name)
        {
            return (VirtualFileInfo?)_paths.Where(
                d => d.FolderId == folderId
                    && d.Name == name
                    && d.Enable
                    && d.Type == VirtualPathType.File
                    ).FirstOrDefault();
        }

        #endregion

        #region [=====公共方法=====]

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="VirtualDiskException"></exception>
        public FileInfo GetFileInfo(string path)
        {
            var info = GetVirtualFileInfo(path);
            if (info is null) throw new VirtualDiskException($"文件'{path}'不存在.");
            return new FileInfo(info);
        }

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckFolderExists(string path)
        {
            return GetVirtualFolderInfo(path) != null;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckFileExists(string path)
        {
            return GetVirtualFileInfo(path) != null;
        }

        /// <summary>
        /// 获取子目录集合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetFolders(string path)
        {
            // 获取标准路径
            path = GetStandardPath(path);
            // 获取目录信息
            long folderId = 0;
            if (path.Length > 1)
            {
                if (path[path.Length - 1] == '/') path = path.Substring(0, path.Length - 1);
                VirtualFolderInfo? info = GetVirtualFolderInfo(path);
                if (info is null) throw new VirtualDiskException($"目录'{path}'不存在.");
                folderId = info.Id;
                path += "/";
            }
            // 获取所有目录名称
            return _paths.Where(
                d => d.FolderId == folderId
                    && d.Enable
                    && d.Type == VirtualPathType.Folder
                    ).Select(d => path + d.Name).ToArray();
        }

        /// <summary>
        /// 获取子目录集合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetFiles(string path)
        {
            // 获取标准路径
            path = GetStandardPath(path);
            // 获取目录信息
            long folderId = 0;
            if (path.Length > 1)
            {
                if (path[path.Length - 1] == '/') path = path.Substring(0, path.Length - 1);
                VirtualFolderInfo? info = GetVirtualFolderInfo(path);
                if (info is null) throw new VirtualDiskException($"目录'{path}'不存在.");
                folderId = info.Id;
                path += "/";
            }
            // 获取所有目录名称
            return _paths.Where(
                d => d.FolderId == folderId
                    && d.Enable
                    && d.Type == VirtualPathType.File
                    ).Select(d => path + d.Name).ToArray();
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        public void CreateFolder(string path)
        {
            // 获取标准路径
            path = GetStandardPath(path);
            if (path.Length < 2) throw new VirtualDiskException("尚未指定路径.");
            // 目录信息
            string[] paths = path.Substring(1).Split('/');
            // 获取根目录
            long folderId = 0;
            // 遍历所有子目录
            for (int i = 0; i < paths.Length; i++)
            {
                // 判断路径是否存在
                VirtualFolderInfo? info = GetChildFolderInfo(folderId, paths[i]);
                if (info != null)
                {
                    folderId = info.Id;
                }
                else
                {
                    // 不存在时添加
                    folderId = AddFolder(folderId, paths[i]).Id;
                }
            }
        }

        /// <summary>
        /// 从流中写入内容到虚拟磁盘中
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fs"></param>
        /// <exception cref="VirtualDiskException"></exception>
        public void WriteFileIn(string path, FileStream fs)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            // 创建或获取文件信息
            var fileInfo = CreateOrGetFile(path);
            if (fs is null)
            {
                fileInfo.DataLength = 0;
                SavePathInfo(fileInfo);
                return;
            }
            long blocks = (long)Math.Ceiling(fs.Length / (double)Page_Size);
            // 当原有的数据块不够时，直接申请新的数据块
            if (fileInfo.DataBlockCount < blocks)
            {
                fileInfo.DataBlockStart = this.DiskInfo.IdleBlock;
                fileInfo.DataBlockCount = blocks;
                this.DiskInfo.IdleBlock += blocks;
                this.SaveDiskInfo();
            }
            // 填入数据长度并保存路径信息
            fileInfo.DataLength = fs.Length;
            SavePathInfo(fileInfo);
            // 写入文件
            lock (obj)
            {
                byte[] buffer = new byte[4096];
                int len = 0;
                f.Position = fileInfo.DataBlockStart * Page_Size;
                fs.Position = 0;
                do
                {
                    len = fs.Read(buffer, 0, buffer.Length);
                    if (len > 0) f.Write(buffer, 0, len);
                } while (len > 0);
            }
        }

        /// <summary>
        /// 复制外部文件到虚拟磁盘中
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filePath"></param>
        public void CopyFileIn(string path, string filePath)
        {
            if (!egg.IO.CheckFileExists(filePath)) throw new VirtualDiskException($"文件'{filePath}'不存在.");
            using (var f = egg.IO.OpenFile(filePath, FileMode.Open, FileAccess.Read))
            {
                WriteFileIn(path, f);
            }
        }

        /// <summary>
        /// 将字节数组写入到虚拟磁盘中
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        /// <exception cref="VirtualDiskException"></exception>
        public void WriteFileIn(string path, byte[] bytes)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            // 创建或获取文件信息
            var fileInfo = CreateOrGetFile(path);
            if (bytes is null)
            {
                fileInfo.DataLength = 0;
                SavePathInfo(fileInfo);
                return;
            }
            long blocks = (long)Math.Ceiling(bytes.Length / (double)Page_Size);
            // 当原有的数据块不够时，直接申请新的数据块
            if (fileInfo.DataBlockCount < blocks)
            {
                fileInfo.DataBlockStart = this.DiskInfo.IdleBlock;
                fileInfo.DataBlockCount = blocks;
                this.DiskInfo.IdleBlock += blocks;
                this.SaveDiskInfo();
            }
            // 填入数据长度并保存路径信息
            fileInfo.DataLength = bytes.Length;
            SavePathInfo(fileInfo);
            // 写入文件
            lock (obj)
            {
                f.Position = fileInfo.DataBlockStart * Page_Size;
                f.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 将文本写入到虚拟磁盘中
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public void WriteFileIn(string path, string content)
        {
            WriteFileIn(path, Encoding.UTF8.GetBytes(content));
        }

        /// <summary>
        /// 读取文件到字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte[] ReadFileToBytes(string path)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            var info = GetVirtualFileInfo(path);
            if (info is null) throw new VirtualDiskException($"文件'{path}'不存在.");
            byte[] bytes = new byte[info.DataLength];
            f.Position = info.DataBlockStart * Page_Size;
            f.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 读取文件到字符串
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadFileToText(string path)
        {
            return Encoding.UTF8.GetString(ReadFileToBytes(path));
        }

        /// <summary>
        /// 读取文件到字节数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void ReadFileToStream(string path, FileStream fs)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            var info = GetVirtualFileInfo(path);
            if (info is null) throw new VirtualDiskException($"文件'{path}'不存在.");
            long lenRead = 0;
            byte[] buffer = new byte[4096];
            int len = 0;
            f.Position = info.DataBlockStart * Page_Size;
            fs.Position = 0;
            do
            {
                long rest = info.DataLength - lenRead;
                len = f.Read(buffer, 0, buffer.Length < rest ? buffer.Length : (int)rest);
                if (len > 0)
                {
                    fs.Write(buffer, 0, len);
                    lenRead += len;
                }
            } while (len > 0);
        }

        /// <summary>
        /// 复制虚拟磁盘中的文件到外部
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filePath"></param>
        public void CopyFileOut(string path, string filePath)
        {
            using (var f = egg.IO.OpenFile(filePath, FileMode.Create, FileAccess.Write))
            {
                ReadFileToStream(path, f);
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="pathSource"></param>
        /// <param name="pathTarget"></param>
        /// <param name="isOverWrite"></param>
        /// <exception cref="VirtualDiskException"></exception>
        public void CopyFile(string pathSource, string pathTarget, bool isOverWrite = false)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            var source = GetVirtualFileInfo(pathSource);
            if (source is null) throw new VirtualDiskException($"未找到文件'{pathSource}'.");
            // 判断目标路径是否存在同名的文件夹
            if (GetVirtualFolderInfo(pathTarget) != null) throw new VirtualDiskException($"已存在目录'{pathTarget}'，无法复制");
            var target = GetVirtualFileInfo(pathTarget);
            if (target is null)
            {
                target = CreateOrGetFile(pathTarget);
            }
            else
            {
                if (!isOverWrite) throw new VirtualDiskException($"文件'{pathTarget}'已存在.");
            }
            // 为新文件申请区块
            target.DataBlockStart = this.DiskInfo.IdleBlock;
            target.DataBlockCount = source.DataBlockCount;
            SavePathInfo(target);
            this.DiskInfo.IdleBlock += target.DataBlockCount;
            this.SaveDiskInfo();
            // 获取开始位置
            long posStartSource = source.DataBlockStart * Page_Size;
            long posStartTarget = target.DataBlockStart * Page_Size;
            long copyLen = 0;
            byte[] buffer = new byte[4096];
            int len = 0;
            // 开始复制
            do
            {
                // 开始复制一个区块
                lock (obj)
                {
                    long rest = source.DataLength - copyLen;
                    f.Position = posStartSource + copyLen;
                    len = f.Read(buffer, 0, buffer.Length < rest ? buffer.Length : (int)rest);
                    if (len > 0)
                    {
                        f.Position = posStartTarget + copyLen;
                        f.Write(buffer, 0, len);
                        copyLen += len;
                    }
                }
            } while (len > 0 && copyLen < source.DataLength);

        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="VirtualDiskException"></exception>
        public void DeleteFile(string path)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            var info = GetVirtualFileInfo(path);
            if (info is null) throw new VirtualDiskException($"未找到文件'{path}'.");
            // 设置删除标记
            DeletePath(info);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isDeleteChild"></param>
        /// <exception cref="VirtualDiskException"></exception>
        public void DeleteFolder(string path, bool isDeleteChild = false)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            var info = GetVirtualFolderInfo(path);
            if (info is null) throw new VirtualDiskException($"未找到目录'{path}'.");
            // 判断是否为空目录
            if (_paths.Where(d => d.FolderId == info.Id && d.Enable).Any())
            {
                if (!isDeleteChild) throw new VirtualDiskException($"目录'{path}'不为空.");
                DeleteFolderWithChildren(info);
                return;
            }
            // 设置删除标记
            DeletePath(info);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="pathSource"></param>
        /// <param name="pathTarget"></param>
        /// <param name="isOverWrite"></param>
        /// <exception cref="VirtualDiskException"></exception>
        public void MoveFile(string pathSource, string pathTarget, bool isOverWrite = false)
        {
            var f = this.FileStream;
            if (f is null) throw new VirtualDiskException("文件流不可用.");
            var source = GetVirtualFileInfo(pathSource);
            if (source is null) throw new VirtualDiskException($"未找到文件'{pathSource}'.");
            // 判断目标路径是否存在同名的文件夹
            if (GetVirtualFolderInfo(pathTarget) != null) throw new VirtualDiskException($"已存在目录'{pathTarget}'，无法移动");
            string pathTargetParent = GetParentPath(pathTarget);
            var targetParent = GetVirtualFolderInfo(pathTargetParent);
            if (targetParent is null) throw new VirtualDiskException($"已存在路径'{pathTarget}'的目录不存在，无法移动");
            var targetName = GetName(pathTarget);
            var target = GetChildFileInfo(targetParent.Id, targetName);
            // 当文件存在目标文件存在时，则删除目标文件
            if (target != null)
            {
                if (!isOverWrite) throw new VirtualDiskException($"文件'{pathTarget}'已存在.");
                // 删除已有的目标文件
                DeletePath(target);
            }
            // 修改数据并保存
            source.FolderId = targetParent.Id;
            if (source.Name != targetName) source.Name = targetName;
            SavePathInfo(source);
        }

        #endregion

        /// <summary>
        /// 虚拟盘
        /// </summary>
        /// <param name="path">存储路径</param>
        /// <param name="password">访问密码</param>
        public Disk(string path, string password)
        {
            // 赋值保存地址
            this.StoragePath = path;
            // 加载磁盘信息
            LoadDiskInfo(password);
            // 加载路径
            LoadPaths();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.FileStream != null)
            {
                try { this.FileStream.Dispose(); } catch { }
            }
            GC.SuppressFinalize(this);
        }
    }
}
