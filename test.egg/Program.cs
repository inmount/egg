using egg.JsonBean;
using System;

namespace test {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            Classmate cm1;
            Classmate cm2 = new Classmate();
            Classmate cm3 = new Classmate();
            using (Classmate nv = new Classmate()) {
                nv.Name = "2-1";
                People jim = new People();
                jim.Name = "Jim";
                jim.Age = 18;
                jim.IsMale = true;
                nv.Peoples.Add(jim);
                nv.Peoples.Add(new People() {
                    Name = "Han Meimei",
                    Age = 17,
                    IsMale = false
                });
                Console.WriteLine(cm2.ToJson());
                cm2.SetJson(nv.ToJson());
                cm2.Name = "2-2";
                cm2.Manager = new People();
                Console.WriteLine(nv.ToJson());
                Console.WriteLine(cm2.ToJson());
                cm2.CloneTo(cm3);
                cm3.Name = "2-3";
                Console.WriteLine("------------");
                Console.WriteLine(nv.ToJson());
                Console.WriteLine(cm2.ToJson());
                eggs.DebugLine("============begin=========");
                Console.WriteLine(cm3.ToJson());
                eggs.DebugLine("============end=========");
                cm1 = (Classmate)nv.Clone();
            }
            cm3["Item"] = new JString("OK");
            cm3.Rename = "3-3";
            Classmate cm4 = new Classmate();
            cm3.CloneTo(cm4);
            cm4.Name = "2-4";
            Console.WriteLine("------------");
            Console.WriteLine(cm1.ToJson());
            Console.WriteLine(cm2.ToJson());
            Console.WriteLine(cm3.ToJson());
            Console.WriteLine(cm4.ToJson());
            var jim2 = new People();
            jim2.CloneFrom((JObject)cm4.Peoples[1]);
            Console.WriteLine(jim2.Name);
            Classmate cm5 = (Classmate)Parser.Parse(cm4.ToJson(), typeof(Classmate));
            cm5.Name = "2-5";
            Console.WriteLine("------------");
            Console.WriteLine(cm5.ToJson());
        }
    }
}
