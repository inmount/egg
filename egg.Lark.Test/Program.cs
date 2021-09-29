using System;

namespace egg.Lark.Test {
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Hello World!");
            double r = System.Console.ReadLine().ToDouble();
            using (egg.Lark.Engine engine = new Engine()) {
                engine.SetVariable("r", new MemeryUnits.Number(r));
                MemeryUnits.Function line1 = engine.AddFunction("let");
                line1.Params.AddDefine("area");
                MemeryUnits.Function line2 = (MemeryUnits.Function)line1.Params.AddFunction("*").GetMemeryUnit();
                line2.Params.AddNumber(3.14);
                MemeryUnits.Function line3 = (MemeryUnits.Function)line2.Params.AddFunction("*").GetMemeryUnit();
                line3.Params.AddDefine("r");
                line3.Params.AddDefine("r");
                engine.Execute();
                System.Console.WriteLine(((MemeryUnits.Number)engine.GetVariable("area")).Value);
            }
            System.Console.ReadKey();
        }
    }
}
