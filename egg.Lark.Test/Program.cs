using System;
using System.Collections.Generic;

namespace egg.Lark.Test {
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Hello World!");
            //double r = System.Console.ReadLine().ToDouble();
            using (egg.Lark.Engine engine = new Engine()) {
                engine.RegFunction("readNumber", (List<MemeryUnits.Unit> args) => {
                    double r = System.Console.ReadLine().ToDouble();
                    return new MemeryUnits.Number(r);
                });
                engine.RegFunction("print", (List<MemeryUnits.Unit> args) => {
                    //double r = System.Console.ReadLine().ToDouble();
                    if (args[0].UnitType == MemeryUnits.UnitTypes.Number)
                        System.Console.Write(((MemeryUnits.Number)args[0]).Value);
                    if (args[0].UnitType == MemeryUnits.UnitTypes.String)
                        System.Console.Write(((MemeryUnits.String)args[0]).Value);
                    return new MemeryUnits.None();
                });
                engine.RegFunction("println", (List<MemeryUnits.Unit> args) => {
                    //double r = System.Console.ReadLine().ToDouble();
                    if (args[0].UnitType == MemeryUnits.UnitTypes.Number)
                        System.Console.WriteLine(((MemeryUnits.Number)args[0]).Value);
                    if (args[0].UnitType == MemeryUnits.UnitTypes.String)
                        System.Console.WriteLine(((MemeryUnits.String)args[0]).Value);
                    return new MemeryUnits.None();
                });
                //engine.SetVariable("r", new MemeryUnits.Number(r));
                //MemeryUnits.Function line1 = engine.AddFunction("let");
                //line1.Params.AddDefine("area");
                //MemeryUnits.Function line2 = (MemeryUnits.Function)line1.Params.AddFunction("*").GetMemeryUnit();
                //line2.Params.AddNumber(3.14);
                //MemeryUnits.Function line3 = (MemeryUnits.Function)line2.Params.AddFunction("*").GetMemeryUnit();
                //line3.Params.AddDefine("r");
                //line3.Params.AddDefine("r");
                //engine.Execute();
                //engine.Execute("let(area,circleArea(r))");
                //System.Console.WriteLine(((MemeryUnits.Number)engine.GetVariable("area")).Value);
                engine.ExecuteFile(@"C:\Users\Win10\Desktop\test.lark");
            }
            System.Console.ReadKey();
        }
    }
}
