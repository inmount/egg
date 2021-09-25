using System;

namespace egg.Lark.Test {
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Hello World!");
            egg.Lark.Engine engine = new Engine();
            var test = engine.AddScrope("test");
        }
    }
}
