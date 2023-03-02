using Egg.Data;
using Egg.Data.Sqlite;
using Egg.Test.Console.Entities;
using Microsoft.EntityFrameworkCore;
using SqliteEFCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console.Data
{
    internal static class Test
    {
        public static void Run()
        {
            var info = SqliteConnectionInfo.Create(egg.IO.GetExecutionPath("data.db"));
            using (var conn = new DatabaseConnection(info))
            {

                var people2Repository = new Egg.Data.Repository<People2, string>(conn);
                using (UnitOfWork uow = conn.BeginUnitOfWork())
                {
                    Random rnd = new Random();
                    var people2 = new People2()
                    {
                        Age = rnd.Next(100),
                        Detail = "OK"
                    };
                    //people2Repository.Insert(people2);
                    people2Repository.Update(d => new { d.Sex, d.Age })
                        .Set(new People2() { Sex = true, Age = 2 }, d => d.flt == 0 && d.Age > 10)
                        .Set(new People2() { Sex = false, Age = 2 }, d => d.flt == 1 && d.Age != 20);
                    //people2Repository.Update().Set(new People2() { Sex = true, Age = 2 }, d => d.flt == 0 && d.Age > 10);
                    System.Console.WriteLine(uow.GetSqlString());
                    //uow.Complete();
                }
            }
        }
    }
}
