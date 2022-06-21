using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test {
    internal static class EggLog {

        internal static void Test() {
            Random random = new Random();
            using (egg.SqliteLog.Logger logger = new egg.SqliteLog.Logger("X:\\a.dlog")) {
                logger.LogInfo("Test", "Test.Info", $"信息数字:{random.NextDouble()}");
                logger.LogWarnning("Test", "Test.Warnning", $"警告数字:{random.NextDouble()}");
                logger.LogError("Test", "Test.Error", $"错误数字:{random.NextDouble()}");
                Console.WriteLine("Log OK");
                var logs = logger.GetRecords(0);
                for (int i = 0; i < logs.Count; i++) {
                    var log = logs[i];
                    Console.WriteLine();
                    Console.WriteLine($"=====[{i}]=====");
                    Console.WriteLine($"Id:{log.Id}");
                    Console.WriteLine($"Object:{log.ObjectName}");
                    Console.WriteLine($"Event:{log.EventName}");
                    Console.WriteLine($"Type:{log.TypeName}");
                    Console.WriteLine($"Time:{log.Time.ToString()}");
                    Console.WriteLine(log.Detail);
                }
            }
        }

    }
}
