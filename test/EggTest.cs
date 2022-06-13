using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg;

namespace test {
    internal class EggTest {

        public EggTest() {
            // Config测试
            using (var cfg = eggs.IO.OpenConfigDocument("X:\\temp.cfg")) {
                cfg.Document["Default"]["Name"] = "测试文档";
                cfg.Document["Default"]["Version"] = "1.0";
                cfg.Save();
            }
            using (var cfg = eggs.IO.OpenConfigDocument("X:\\temp.cfg")) {
                Console.WriteLine(cfg.Document["Default"]["Speed"]);
            }
            // json测试
            //eggs.Json.EnforceUnicode = true;
            using (var f = eggs.IO.OpenJsonDocument("X:\\temp.json")) {
                var obj = f.Document.RootNode;
                //var obj = new egg.Serializable.Json.Object();
                string json = "{\"Result\":1,\"Datas\":[{\"ID\":\"1\",\"Name\":\"/u65E0/u540D\",\"Score\":\"0\"},{\"ID\":\"2\",\"Name\":\"/u65E0/u540D\",\"Score\":\"0\"}]}";
                //var obj = eggs.Json.Parse(json);
                obj.Json("header", json);
                if (obj.Keys.Contains("Index")) {
                    obj.Number("Index", obj.Number("Index") + 1);
                } else {
                    obj.Number("Index", 1);
                }
                Console.WriteLine(obj.SerializeToString());
                f.Save();
            }
            // xml测试
            Console.WriteLine("Xml");
            using (var f = eggs.IO.OpenXmlDocument("X:\\temp.xml")) {
                var doc = f.Document;
                var root = doc["root"];
                if (root == null) root = doc.AddNode("root");
                var header = root["header"];
                if (header == null) header = root.AddNode("header");
                if (header.Attr["Index"].ToInteger() <= 0) {
                    header.Attr["Index"] = "1";
                } else {
                    header.Attr["Index"] = "" + (header.Attr["Index"].ToInteger() + 1);
                }
                Console.WriteLine(doc.InnerXml);
                f.Save();
            }
            // Html测试
            Console.WriteLine("Html");
            using (var f = eggs.IO.OpenHtmlDocument("X:\\temp.html")) {
                var doc = f.Document;
                var header = doc.Head;
                if (header.Attr["Index"].ToInteger() <= 0) {
                    header.Attr["Index"] = "1";
                } else {
                    header.Attr["Index"] = "" + (header.Attr["Index"].ToInteger() + 1);
                }
                Console.WriteLine(doc.InnerHTML);
                f.Save();
            }
            // Markdown测试
            Console.WriteLine("Markdown测试");
            using (var f = eggs.IO.OpenMarkdownDocument("X:\\temp.md")) {
                var doc = f.Document.Root;
                doc.Children.Add(new egg.Serializable.Markdown.MdTitle(1));
                doc.Children.Add(new egg.Serializable.Markdown.MdText() { Content = "测试" });
                Console.WriteLine(doc.ToHtml());
                Console.WriteLine(doc.ToMarkdown());
                f.Save();
            }
            // 对象为空测试
            egg.KeyStrings sz = null;
            Console.WriteLine($"sz:{sz == null}");
            // Unicode测试
            string us = "我爱北京天安门，OK";
            Console.WriteLine(us.UnicodeCoding());
            Console.WriteLine(us.UnicodeCoding().UnicodeDecoding());
        }

    }
}
