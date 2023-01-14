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
            string path = egg.IO.GetExecutionPath("Lark/test.lark");
            string script = egg.IO.ReadUtf8FileContent(path);
            //System.Console.WriteLine(script);
            var func = ScriptParser.Parse(script);
            System.Console.WriteLine(func.ToString());
            using (ScriptFunctions funcs = new ScriptFunctions())
            {
                funcs.Reg<TestFuntion>();
                using (Egg.Lark.ScriptEngine engine = new Egg.Lark.ScriptEngine(func, funcs))
                {
                    engine.Memory["c"] = 3;
                    engine.Execute();
                    //System.Console.WriteLine($"[Lark] a = {engine.Memory["a"]}");
                    //System.Console.WriteLine($"[Lark] b = {engine.Memory["b"]}");
                }
            }
        }
    }
}
