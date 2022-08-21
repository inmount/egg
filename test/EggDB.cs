using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using egg.db.Orm;

namespace test {
    internal class EggDB {

        public static void Test() {
            Random r = new Random();
            //OrmTest orm = new OrmTest();
            //orm.Name = "abc";
            //Console.WriteLine(orm["Name"].ToString());
            //orm["Name"] = "def";
            //Console.WriteLine(orm.Name);
            var db = new egg.db.SqliteDatabase() { Path = "X:\\a.db" };
            using (var dbc = new egg.db.Connection(db)) {
                if (!dbc.CheckTable(typeof(OrmTest))) dbc.CreateTable(typeof(OrmTest));
                var row = new OrmTest();
                row.Name = $"NM:{r.NextDouble()}";
                dbc.Insert(row).Exec();
                var Test = row.GetTableDefine();
                Console.WriteLine(dbc.Select(Test).ToSqlString());
                var rows = dbc.Select(Test).GetRows<OrmTest>();
                Console.WriteLine($"rows.Count={rows.Count}");
                for (int i = 0; i < rows.Count; i++) {
                    Console.WriteLine($"rows[{i}].Name={rows[i].Name}");
                }
            }
        }

    }
}
