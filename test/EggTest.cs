using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg;

namespace test {
    internal class EggTest {

        public EggTest() {
            //// Config测试
            //using (var cfg = eggs.IO.OpenConfigFile("X:\\temp.cfg")) {
            //    cfg["Default"]["Name"] = "测试文档";
            //    cfg["Default"]["Version"] = "1.0";
            //    cfg.Save();
            //}
            //using (var cfg = eggs.IO.OpenConfigFile("X:\\temp.cfg")) {
            //    Console.WriteLine(cfg["Default"]["Speed"]);
            //}
            // json测试
            var obj = new egg.Serializable.Json.Object();
            string json = "{\"Timestamp\":\"1653910554\",\"Token\":\"9eb51ee9 - aaba - 4d3c - 9e7e - 93f31e024efc\",\"Form\":{}}";
            //var obj = eggs.Json.Parse(json);
            obj.Json("header", json);
            Console.WriteLine(obj.SerializeToString());
            egg.KeyStrings sz = null;
            Console.WriteLine($"sz:{sz.IsNull()}");
        }

    }
}
