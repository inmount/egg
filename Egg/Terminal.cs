using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Egg
{
    /// <summary>
    /// 模拟终端
    /// </summary>
    public class Terminal : IDisposable
    {
        // 定义常量
        private const string Normal_Chars = "~!@#$%^&*()_+-=QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm{}:|\"<>?[]\\;',./1234567890";
        private const int Char_Enter = (byte)'\n';

        // 定义全局变量
        private readonly List<int> _chars;
        private StreamWriter? _input;
        private readonly Encoding _encoding;

        /// <summary>
        /// 输出缓存区
        /// </summary>
        public List<string> Buffer { get; }

        /// <summary>
        /// 工作目录
        /// </summary>
        public string WorkPath { get; }

        // 检测并创建新行
        private int CheckOrCreateLine(int line)
        {
            if (line < 0)
            {
                this.Buffer.Add("");
                line = this.Buffer.Count - 1;
            }
            return line;
        }

        // 读取
        private int Read(StreamReader reader, List<int> chars, StringBuilder sb, int line)
        {
            int res = reader.Read();
            if (res > 255)
            {
                // 处理双字节
                sb.Append((char)res);
                line = CheckOrCreateLine(line);
                this.Buffer[line] = sb.ToString();
            }
            else if (chars.Contains(res))
            {
                // 处理常规字符
                sb.Append((char)res);
                line = CheckOrCreateLine(line);
                this.Buffer[line] = sb.ToString();
            }
            else
            {
                switch (res)
                {
                    case Char_Enter:
                        // 回车
                        this.Buffer.Add("");
                        line = this.Buffer.Count - 1;
                        sb.Clear();
                        break;
                    case 0x09:
                        // Tab
                        line = CheckOrCreateLine(line);
                        var len = this.Buffer[line].Length;
                        sb.Append(new string(' ', 8 - len % 8));
                        this.Buffer[line] = sb.ToString();
                        break;
                    case '\r':
                    case ' ':
                        // 常规处理
                        sb.Append((char)res);
                        line = CheckOrCreateLine(line);
                        this.Buffer[line] = sb.ToString();
                        break;
                    default:
                        sb.Append($"[0x{res.ToString("x2")}]");
                        sb.Append((char)res);
                        line = CheckOrCreateLine(line);
                        this.Buffer[line] = sb.ToString();
                        break;
                }
            }
            return line;
        }

        /// <summary>
        /// 模拟终端
        /// </summary>
        public Terminal(string? path = null, Encoding? encoding = null)
        {
            // 建立缓存区
            this.Buffer = new List<string>();
            path = path.IsEmpty() ? egg.Assembly.WorkingDirectory : path;
            this.WorkPath = path ?? string.Empty;
            // 处理标准字符
            _chars = new List<int>();
            for (int i = 0; i < Normal_Chars.Length; i++) _chars.Add(Normal_Chars[i]);
            // 字符编码
            _encoding = encoding ?? Encoding.Default;
        }

        /// <summary>
        /// 设置输入内容
        /// </summary>
        /// <param name="content"></param>
        public void SetInput(string content)
        {
            _input?.Write(content);
        }

        /// <summary>
        /// 清空缓存区
        /// </summary>
        public void BufferClear() => this.Buffer.Clear();

        // 初始化启动器
        private void InitializeProcessStartInfo(ProcessStartInfo processStartInfo)
        {
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.StandardInputEncoding = _encoding;
            processStartInfo.StandardOutputEncoding = _encoding;
            processStartInfo.StandardErrorEncoding = _encoding;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WorkingDirectory = this.WorkPath;
        }

        /// <summary>
        /// 执行命令行程序
        /// </summary>
        /// <returns></returns>
        public string Execute(ProcessStartInfo processStartInfo)
        {
            // 初始化执行环境
            InitializeProcessStartInfo(processStartInfo);
            var process = Process.Start(processStartInfo);
            if (process is null) throw new Exception($"程序'{processStartInfo.FileName}'执行失败");
            _input = process.StandardInput;
            var output = process.StandardOutput;
            var error = process.StandardError;
            int outputLine = -1;
            int errorLine = -1;
            int startLine = this.Buffer.Count;
            StringBuilder sbOutput = new StringBuilder();
            StringBuilder sbError = new StringBuilder();
            Thread thread = new Thread(() =>
            {
                try
                {
                    while (!process.HasExited && !output.EndOfStream)
                    {
                        outputLine = Read(output, _chars, sbOutput, outputLine);
                    }
                }
                catch { }
            });
            thread.Start();
            Thread thread2 = new Thread(() =>
            {
                try
                {
                    while (!process.HasExited && !error.EndOfStream)
                    {
                        errorLine = Read(error, _chars, sbError, errorLine);
                    }
                }
                catch { }
            });
            thread2.Start();
            process.WaitForExit();
            try
            {
                while (!output.EndOfStream)
                {
                    outputLine = Read(output, _chars, sbOutput, outputLine);
                }
            }
            catch { }
            try
            {
                //while (!process.HasExited)
                while (!error.EndOfStream)
                {
                    errorLine = Read(error, _chars, sbError, errorLine);
                }
            }
            catch { }
            // 输出执行内容
            StringBuilder sb = new StringBuilder();
            for (int i = startLine; i <= this.Buffer.Count - 1; i++)
            {
                sb.AppendLine(this.Buffer[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 执行命令行程序
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Execute(string filePath, string? args)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo()
            {
                FileName = filePath,
                Arguments = args ?? string.Empty,
            };
            return Execute(processStartInfo);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
