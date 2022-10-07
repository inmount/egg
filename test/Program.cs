using egg;
using System;

namespace test {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine($"ExecutionFilePath={eggs.Assembly.ExecutionFilePath}");
            Console.WriteLine($"Name={eggs.Assembly.Name}");
            Console.WriteLine($"Version={eggs.Assembly.Version}");

            string path = eggs.IO.GetExecutionPath("setting.json");
            eggs.Configure.Use<egg.Configure.SchemaConfigureCollection>().AddJsonFile(path);
            var node = eggs.Configure.GetSection<ConfigureNode>("node");
            Console.WriteLine($"Node={node.Name},{node.exclude.Count}");

            //// 测试为空判断
            //egg.Serializable.Json.Object t = null;
            //Console.WriteLine($"strings:{eggs.Object.IsNull(t)}");
            //Console.WriteLine($"strings:{t.IsNull()}");

            // egg组件测试
            //EggTest eggTest = new EggTest();
            //EggJsonTest.Test();

            // CloudDB组件测试
            //EggCloudDB cloudDB = new EggCloudDB();

            // 测试MVC
            //EggMvc egg = new EggMvc();

            // 测试Log
            EggLog.Test();

            // 测试DB
            //EggDB.Test();

            // 读取控制台
            try { Console.ReadKey(); } catch { }
        }
    }
}
