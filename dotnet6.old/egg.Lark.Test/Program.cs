using System;
using System.Collections.Generic;

namespace egg.Lark.Test {
    class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Hello World!");
            //double r = System.Console.ReadLine().ToDouble();
            using (egg.Lark.ScriptMemeryPool pool = new ScriptMemeryPool()) {
                using (egg.Lark.ScriptEngine engine = new ScriptEngine(pool)) {
                    var sys = pool.CreateObject().ToObject();
                    sys["readNumber"] = new MemeryUnits.NativeFunction(pool, (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                        double r = System.Console.ReadLine().ToDouble();
                        return pool.CreateNumber(r, sys.Handle).MemeryUnit;
                    });
                    sys["print"] = new MemeryUnits.NativeFunction(pool, (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                        var content = args["content"];
                        if (content.UnitType == MemeryUnits.UnitTypes.Number)
                            System.Console.Write(((MemeryUnits.Number)content).Value);
                        if (content.UnitType == MemeryUnits.UnitTypes.String)
                            System.Console.Write(((MemeryUnits.String)content).Value);
                        return pool.None;
                    }, egg.Strings.Create("content"));
                    sys["println"] = new MemeryUnits.NativeFunction(pool, (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                        var content = args["content"];
                        if (content.UnitType == MemeryUnits.UnitTypes.Number)
                            System.Console.WriteLine(((MemeryUnits.Number)content).Value);
                        if (content.UnitType == MemeryUnits.UnitTypes.String)
                            System.Console.WriteLine(((MemeryUnits.String)content).Value);
                        return pool.None;
                    }, egg.Strings.Create("content"));
                    //engine.RegFunction("readNumber", (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                    //    double r = System.Console.ReadLine().ToDouble();
                    //    return new MemeryUnits.Number(r);
                    //});
                    //engine.RegFunction("print", (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                    //    //double r = System.Console.ReadLine().ToDouble();
                    //    var content = args["content"];
                    //    if (content.UnitType == MemeryUnits.UnitTypes.Number)
                    //        System.Console.Write(((MemeryUnits.Number)content).Value);
                    //    if (content.UnitType == MemeryUnits.UnitTypes.String)
                    //        System.Console.Write(((MemeryUnits.String)content).Value);
                    //    return new MemeryUnits.None();
                    //}, egg.Strings.Create("content"));
                    //engine.RegFunction("println", (ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                    //    //double r = System.Console.ReadLine().ToDouble();
                    //    var content = args["content"];
                    //    if (content.UnitType == MemeryUnits.UnitTypes.Number)
                    //        System.Console.WriteLine(((MemeryUnits.Number)content).Value);
                    //    if (content.UnitType == MemeryUnits.UnitTypes.String)
                    //        System.Console.WriteLine(((MemeryUnits.String)content).Value);
                    //    return new MemeryUnits.None();
                    //}, egg.Strings.Create("content"));
                    engine.SetVariable("sys", sys);
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
                    engine.ExecuteFile(@"X:\temp\lark\test2.lark");
                }
            }

            System.Console.ReadKey();
        }
    }
}
