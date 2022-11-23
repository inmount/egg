using Egg.EFCore;
using Egg.Test.Console.Entities;
using Egg.VirtualDisk;
using Microsoft.EntityFrameworkCore;
using SqliteEFCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console.VirtualDisk
{
    internal class Test
    {

        // 输出所有目录
        private static void OutputFolders(Disk disk, string path)
        {
            string[] folders = disk.GetFolders(path);
            string[] files = disk.GetFiles(path);
            foreach (var file in files)
            {
                var info = disk.GetFileInfo(file);
                System.Console.WriteLine($"      {file} {info.Length}");
            }
            foreach (var folder in folders)
            {
                System.Console.WriteLine($"[DIR] {folder}");
                OutputFolders(disk, folder);
            }
        }

        public static void Run()
        {
            using (var disk = new Egg.VirtualDisk.Disk("E:\\text.evd", "123456"))
            {
                string path = "/a/b/c";
                System.Console.WriteLine($"CheckFolderExists('{path}'): {disk.CheckFolderExists(path)}");
                if (!disk.CheckFolderExists(path)) disk.CreateFolder(path);
                System.Console.WriteLine($"CheckFolderExists('{path}'): {disk.CheckFolderExists(path)}");
                string path1 = "/a/b/c1";
                if (!disk.CheckFolderExists(path1)) disk.CreateFolder(path1);
                string path2 = "/a/b2/c2";
                if (!disk.CheckFolderExists(path2)) disk.CreateFolder(path2);
                string path3 = "/a/b3/c3";
                if (!disk.CheckFolderExists(path3)) disk.CreateFolder(path3);
                string path4 = @"\Projects\Other\dotnet-dpz3-vdisk-master\dpz3.VDisk";
                if (!disk.CheckFolderExists(path4)) disk.CreateFolder(path4);
                string path5 = @"\Projects\Other2\dotnet-dpz3-vdisk-master\dpz3.VDisk";
                if (!disk.CheckFolderExists(path5)) disk.CreateFolder(path5);
                string path6 = @"\Projects\Other3\dotnet-dpz3-vdisk-master\dpz3.VDisk";
                if (!disk.CheckFolderExists(path6)) disk.CreateFolder(path6);
                OutputFolders(disk, "/");
                string file1 = "/Projects/Other3/hello.txt";
                if (!disk.CheckFileExists(file1)) disk.WriteFileIn(file1, "你好，虚拟磁盘");
                System.Console.WriteLine(disk.ReadFileToText(file1));
                if (disk.CheckFileExists(file1)) disk.DeleteFile(file1);
                string file2 = "/Projects/Other3/hello2.mp4";
                if (!disk.CheckFileExists(file2)) disk.CopyFileIn(file2, @"E:\Video\密码号\001 问候\001 问候\001 问候.mp4");
                if (disk.CheckFileExists(file2)) disk.CopyFileOut(file2, @"E:\Video\密码号\001 问候\001 问候\001 问候 02.mp4");
                string file3 = "/Projects/Other/hello3.mp4";
                if (!disk.CheckFileExists(file3)) disk.CopyFile(file2, file3);
                if (disk.CheckFileExists(file3)) disk.CopyFileOut(file2, @"E:\Video\密码号\001 问候\001 问候\001 问候 03.mp4");
                string file4 = "/Projects/Other2/hello4.mp4";
                if (disk.CheckFileExists(file3)) disk.MoveFile(file3, file4, true);
                if (disk.CheckFileExists(file4)) disk.CopyFileOut(file2, @"E:\Video\密码号\001 问候\001 问候\001 问候 04.mp4");
            }
        }
    }
}
