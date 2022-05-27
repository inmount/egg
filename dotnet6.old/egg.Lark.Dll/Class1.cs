using egg.Lark;
namespace egg.Lark.Dll {
    public class Class1 {
        public string Name { get; set; }
        public ScriptEngine.Function test;

        public Class1() {
            this.Name = "Name";
            this.test = new ScriptEngine.Function((ScriptMemeryPool pool, ScriptEngine.FunctionArgs args) => {
                return pool.CreateString($"Class1 Name is {this.Name}").MemeryUnit;
            });
        }
    }
}