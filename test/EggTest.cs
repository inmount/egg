using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg;

namespace test {
    internal class EggTest {

        public EggTest() {
            using (var cfg = eggs.IO.OpenConfigFile("X:\\temp.cfg")) {
                cfg["Default"]["Name"] = "测试文档";
                cfg["Default"]["Version"] = "1.0";
                cfg.Save();
            }
            using (var cfg = eggs.IO.OpenConfigFile("X:\\temp.cfg")) {
                Console.WriteLine(cfg["Default"]["Speed"]);
            }
        }

    }
}
