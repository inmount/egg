using Egg.Lark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console.Lark
{
    public class Test
    {
        public static void Run()
        {
            var func = ScriptParser.Parse(@"
                @(
                    let(a, $(""1+2="", +(1, 2))),
                    let(b, *(c, 4)),
                )
            ");
            using (Egg.Lark.ScriptEngine engine = new Egg.Lark.ScriptEngine(func))
            {
                engine.Memory["c"] = 3.0;
                engine.Execute();
                System.Console.WriteLine($"[Lark] a = {engine.Memory["a"]}");
                System.Console.WriteLine($"[Lark] b = {engine.Memory["b"]}");
            }
        }
    }
}
