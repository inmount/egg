using egg;
using System;

namespace test {
    class Program {
        static void Main(string[] args) {
            // 测试为空判断
            egg.Serializable.Json.Object t = null;
            Console.WriteLine($"strings:{eggs.Object.IsNull(t)}");
            Console.WriteLine($"strings:{t.IsNull()}");

            // egg组件测试
            EggTest eggTest = new EggTest();

            // CloudDB组件测试
            //EggCloudDB cloudDB = new EggCloudDB();

            // 读取控制台
            Console.ReadKey();
        }
    }
}
