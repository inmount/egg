using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SqliteEFCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console.DbContexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        public CoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            //optionsBuilder.UseSqlite($"D:\\core.db");
            optionsBuilder.UseNpgsql(Consts.Connect_String);

            return new CoreDbContext(optionsBuilder.Options);
        }
    }
}
