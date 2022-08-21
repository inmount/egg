using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Json = egg.Serializable.Json;

namespace test {
    internal class EggJsonTest {

        public static void Test() {
            using (var f = eggs.IO.OpenJsonDocument("X:\\test.json")) {
                Json.Object obj = new Json.Object();
                obj.List("items")
                    .Object(0)
                    .String("Name", "Test");
                f.Document.RootNode = obj;
                f.Save();
            }
            using (var f = eggs.IO.OpenJsonDocument("X:\\things.json")) {
                Json.Node obj = f.Document.RootNode;
                foreach (var key in obj.Keys) {
                    Console.WriteLine($"key={key}:{key.Length}:key[0]={(byte)key[0]}");
                }
                Json.List items = (Json.List)obj["items"];
                Console.WriteLine(items.SerializeToString());
                Console.WriteLine(items.Count);
            }
            egg.KeyValues vs = new egg.KeyValues();
            vs["name"] = 6;
            Console.WriteLine(vs["name"].ToString());
        }

    }
}
