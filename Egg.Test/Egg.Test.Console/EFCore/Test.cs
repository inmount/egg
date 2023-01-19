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
        public static async void Run()
        {
            float? a = null;
            var at = a.GetType();
            var ass = at.IsNumeric();
            egg.Logger.Info("Egg.Test.Console.EFCore", "Test");
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseNpgsql(Consts.Connect_String);
            var dbContext = new CoreDbContext(optionsBuilder.Options);
            var reps = dbContext.GetRepositories();
            //var dbContexts = new RepositoryDbContexts(optionsBuilder.Options);
            //dbContexts.Use<CoreDbContext>();
            var people2Repository = new Repository<People2, string>(dbContext);

            List<string> names = new List<string>()
            {
                "aaa","bbb"
            };
            int minAge = 10;

            // 更新数据
            await people2Repository.UpdateAsync(new People2()
            {
                Age = 18,
                Sex = true,
                Name = "Test",
            },
            d => ((d.Id != "111" || d.Id != null) && d.Age > minAge && names.Contains(d.Name ?? "")),
            d => new { d.Sex, d.Age });

            var query = from p in people2Repository.Query()
                        where (p.Id != "111" || p.Id != null) && p.Age > minAge && names.Contains(p.Name ?? "")
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
