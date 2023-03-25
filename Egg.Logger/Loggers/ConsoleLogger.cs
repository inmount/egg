using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Egg.Log.Loggers
{

    /// <summary>
    /// 控制台输出
    /// </summary>
    public class ConsoleLogger : ILogable
    {

        /// <summary>
        /// 输出内容
        /// </summary>
        /// <param name="entity"></param>
        public void Log(LogInfo entity)
        {
            var color = Console.ForegroundColor;
            //Console.ForegroundColor = ConsoleColor.Blue;
            switch (entity.Level)
            {
                case LogLevel.Fatal: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case LogLevel.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogLevel.Warn: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogLevel.Info: Console.ForegroundColor = ConsoleColor.Green; break;
                default: Console.ForegroundColor = color; break;
            }
            Console.Write($"*");
            Console.Write($"{egg.Time.Now.ToFullDateTimeString()}");
            Console.Write($"*");
            Console.Write($" [{entity.Level.ToString().ToUpper()}]");
            if (entity.Level == LogLevel.Info)
            {
                Console.ForegroundColor = color;
            }
            Console.Write($" {entity.Event} {entity.Message}");
            Console.WriteLine();
            Console.ForegroundColor = color;
        }
    }
}
