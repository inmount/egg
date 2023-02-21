using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console
{
    internal static class Test
    {
        private static void PrintSnowId(Snowflake snowflake, long id)
        {
            snowflake.TryParse(id, out DateTime t, out int mid, out int seq);
            System.Console.WriteLine($"id={id}, t={t.ToString("yyyy-MM-dd HH:mm:ss.fff")}, mid={mid}, seq={seq}");
        }

        public static void Run()
        {
            // 测试雪花算法
            System.Console.WriteLine("snowflake1");
            var snowflake1 = new Snowflake();
            for (int i = 0; i < 100; i++)
            {
                System.Console.WriteLine(snowflake1.Next());
            }
            System.Console.WriteLine("snowflake2");
            var snowflake2 = new Snowflake(1);
            for (int i = 0; i < 100; i++)
            {
                System.Console.WriteLine(snowflake2.Next());
            }
            PrintSnowId(snowflake2, 412582302871195649);
            PrintSnowId(snowflake2, 412582302875389952);
            System.Console.WriteLine("snowflake3");
            var snowflake3 = new Snowflake(new DateTime(2023, 1, 1), 1);
            for (int i = 0; i < 100; i++)
            {
                System.Console.WriteLine(snowflake3.Next());
            }
            PrintSnowId(snowflake3, 15404248699244547);
            System.Console.WriteLine("snowflake lazy");
            egg.Generator.SnowflakeStartTime = new DateTime(2022, 1, 1);
            egg.Generator.SnowflakeMachineId = 10;
            var snowflake4 = egg.Generator.Snowflake;
            for (int i = 0; i < 100; i++)
            {
                System.Console.WriteLine(snowflake4.Next());
            }
            PrintSnowId(snowflake4, 147683396636680192);
            PrintSnowId(snowflake4, 147683396640874496);
            PrintSnowId(snowflake4, 147683396649263104);
            Encoding encoding = Encoding.GetEncoding("GB2312");
            string res = egg.Terminal.Execute("ping", "www.baidu.com", encoding);
            System.Console.WriteLine(res);
            string res2 = egg.Terminal.Execute("ping", "www.aliyun.com", encoding);
            System.Console.WriteLine(res2);
            //double value = (double)Activator.CreateInstance(Type.GetType("System.Double"));
            //System.Console.WriteLine(value);
            //snowflake3.TryParse(15404248699244547, out DateTime t, out int mid, out int seq);
            //Console.WriteLine($"id=15404248699244547, t={t.ToString("yyyy-MM-dd HH:mm:ss.fff")}, mid={mid}, seq={seq}");
        }
    }
}
