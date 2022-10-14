// See https://aka.ms/new-console-template for more information

using Egg;
using Egg.EFCore;
using Egg.EFCore.Sqlite;
using Egg.Log.Loggers;
using Egg.Test.Console;
using Egg.Test.Console.Entities;
using Microsoft.EntityFrameworkCore;
using SqliteEFCore.DbContexts;

// 初始化日志管理器
egg.Logger
    .Use(new FileLogger(egg.IO.GetExecutionPath("logs")))
    .Use<ConsoleLogger>()
    .Use<VsLogger>();

Console.WriteLine("Hello, World!");

ClsTestObject? obj = null;
var obj1 = obj ?? new ClsTestObject();
ClsTestObject? obj2 = null;
var obj3 = obj2?.ToNotNull<ClsTestObject>();

egg.Logger.Debug("a=" + obj1?.a);
egg.Logger.Info("obj2=" + obj2?.IsNull());
egg.Logger.Warn("obj3.a=" + obj3?.a, "ClsTestObject");

var now = egg.Time.Now;
var day30 = now.AddDays(30);
egg.Logger.Error(now.ToDateTimeString());
egg.Logger.Fatal("" + now.ToUnixTimeSeconds());
var now2 = egg.Time.Parse(now.ToUnixTimeSeconds());
Console.WriteLine(now2.ToDateTimeString());
var ts = day30 - now;
Console.WriteLine(ts);
var dt2 = egg.Time.Parse(day30.ToUnixTimeSeconds());
Console.WriteLine(dt2.ToDateTimeString());
var ts2 = dt2 - now;
Console.WriteLine(ts2);

var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
optionsBuilder.UseSqlite($"Data Source=D:\\core.db");
var context = new CoreDbContext(optionsBuilder.Options);
egg.Logger.Info(context.Database.ProviderName, "DbContext");
egg.Logger.Info(context.GetDbType(), "DbContext");
egg.Logger.Info(context.Peoples.ToQueryString(), "DbContext");
egg.Logger.Info("EnsureCreatedSqlite:" + context.EnsureCreatedSqlite(), "DbContext");
egg.Logger.Info(context.Database.GetDbConnection().DataSource, "DbContext");

SqliteCreater creater = new SqliteCreater();
//egg.Logger.Info(creater.GetEnsureCreatedSql(context), "DbContext");

var list = from p in context.People2s
           select p;

foreach (var p in list)
{
    egg.Logger.Info($"{{id: \"{p.Id}\", Age: {p.Age}, Detail: \"{p.Detail}\"}}", "People");
}

Random rnd = new Random();
var people = new People2()
{
    Age = rnd.Next(100),
    Detail = "OK"
};
context.People2s.Add(people);
context.SaveChanges();