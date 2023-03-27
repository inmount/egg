// See https://aka.ms/new-console-template for more information

string code = "return(let(a, \"Hello World\"), a)"; 
Console.WriteLine(code);
using (var script = Egg.Lark.ScriptParser.Parse(code))
using (var funcs = new Egg.Lark.ScriptFunctions())
{
    using (var engine = new Egg.Lark.ScriptEngine(script, funcs))
    {
        Console.WriteLine(engine.Execute());
    }
}