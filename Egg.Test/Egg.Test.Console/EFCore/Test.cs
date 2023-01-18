using Egg.EFCore;
using Egg.EFCore.Dbsets;
using Egg.Test.Console.Entities;
using Microsoft.EntityFrameworkCore;
using SqliteEFCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console.EFCore
{
    internal static class Test
    {
        public static void Run()
        {
            egg.Logger.Info("Egg.Test.Console.EFCore", "Test");
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseNpgsql(Consts.Connect_String);
            var dbContext = new CoreDbContext(optionsBuilder.Options);
            var reps = dbContext.GetRepositories();
            //var dbContexts = new RepositoryDbContexts(optionsBuilder.Options);
            //dbContexts.Use<CoreDbContext>();
            var people2Repository = new Repository<People2, string>(dbContext);

            var query = from p in people2Repository.Query()
                        select p;
            //query.Where()

            egg.Logger.Info(query.ToQueryString());
            var list = query.ToList();

            foreach (var p in list)
            {
                egg.Logger.Info($"{{id: \"{p.Id}\", Age: {p.Age}, Detail: \"{p.Detail}\"}}", "People");
            }

            egg.Logger.Info($"Count: {list.Count}", "People");

            using (UnitOfWork uow = new UnitOfWork(dbContext))
            {
                Random rnd = new Random();
                var people2 = new People2()
                {
                    Age = rnd.Next(100),
                    Detail = "OK"
                };
                people2Repository.Insert(people2);
                people2Repository.Update().Use(d => new { d.Sex, d.Age }).Set(new People2() { Sex = true, Age = 2 }, d => d.flt == 0 && d.Age > 10);
                uow.Complete();
            }
        }
    }
}
