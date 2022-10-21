// See https://aka.ms/new-console-template for more information

using Egg;
using Egg.BarCode;
using Egg.EFCore;
using Egg.EFCore.Sqlite;
using Egg.Log.Loggers;
using Egg.Test.Console;
using Egg.Test.Console.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SqliteEFCore.DbContexts;
using System.Data;
using System.Diagnostics;

// 初始化日志管理器
egg.Logger
    .Reg((string? message) =>
    {
        Debug.WriteLine(message);
    })
    .Reg(new FileLogger(egg.IO.GetExecutionPath("logs")))
    .Use<ConsoleLogger>()
    .Use<VsLogger>();

Console.WriteLine("Hello, World!");

Rules rules = new Rules();
rules.Use(new FixedString("XS-"))
    .Use(new DateTimeRule("yyyyMMdd"))
    .Use(new FixedString("-"))
    .Use(new SerialNoRules()
        .Use(new SerialNoRule() { Min = 1, Max = 105, Space = 4 })
        .Use(new SerialNoRule() { Min = 106, Step = 2, Space = 4 })
        )
    .Use(new FixedString("-"))
    .Use(new RandomStringRule("1234567890", 4));

// 生成条码
for (int i = 101; i <= 112; i++)
{
    Console.WriteLine(egg.BarCodeGenerator.Generate(rules, i));
}

RuleCollection rc = new RuleCollection();
rc.Use("dt", new DateTimeRule("yyyyMMdd"))
    .Use("sn", new SerialNoRules()
        .Use(new SerialNoRule() { Min = 1, Max = 105, Space = 4 })
        .Use(new SerialNoRule() { Min = 106, Step = 2, Space = 4 })
        );
People2 p2 = new People2();
p2.Age = 50;

// 生成条码
for (int i = 101; i <= 112; i++)
{
    Console.WriteLine(egg.BarCodeGenerator.Generate("XS$(Age)-$(dt)-$(sn)", rc, i, p2));
}


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

// 调试时间
egg.Logger.Debug("" + ((DateTimeOffset)DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).ToDateTimeString(), "Now");
egg.Logger.Debug("" + ((DateTimeOffset)DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)).ToUnixTimeSeconds(), "Now");
egg.Logger.Debug("" + ((DateTimeOffset)DateTime.SpecifyKind(DateTime.Parse("2022-10-16 11:29:56"), DateTimeKind.Utc)).ToDateTimeString(), "2022-10-16 11:29:56");
egg.Logger.Debug("" + ((DateTimeOffset)DateTime.SpecifyKind(DateTime.Parse("2022-10-16 11:29:56"), DateTimeKind.Utc)).ToUnixTimeSeconds(), "2022-10-16 11:29:56");
egg.Logger.Debug("" + ((DateTimeOffset)DateTime.Parse("2022-10-16 11:29:56")).ToUnixTimeSeconds(), "2022-10-16 11:29:56");
egg.Logger.Debug("" + (egg.Time.Parse("2022-10-16 11:29:56")).ToUnixTimeSeconds(), "egg.Time.Parse");

var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
//optionsBuilder.UseSqlite($"Data Source=D:\\core.db");
//var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
optionsBuilder.UseNpgsql(Consts.Connect_String);
var context = new CoreDbContext(optionsBuilder.Options);
var people2Repository = new RepositoryBase<People2, string>(context);
//egg.Logger.Info(context.Database.ProviderName, "DbContext");
//egg.Logger.Info(context.GetDbType(), "DbContext");
//egg.Logger.Info(context.Peoples.ToQueryString(), "DbContext");
//egg.Logger.Info("EnsureCreated:" + context.EnsureCreatedSqlite(), "Sqlite");
egg.Logger.Info("EnsureCreated:" + context.EnsureCreatedPostgreSQL(), "PostgreSQL");
//egg.Logger.Info(context.Database.GetDbConnection().DataSource, "DbContext");

//SqliteCreater sqlite = new SqliteCreater();
//egg.Logger.Info(sqlite.GetEnsureCreatedSql(context), "DbContext");

//PostgreSQLCreater postgreSQL = new PostgreSQLCreater();
//egg.Logger.Info(postgreSQL.GetEnsureCreatedSql(context), "DbContext");

var query = from p in people2Repository.Query()
            select p;

Console.WriteLine(query.ToQueryString());
var list = query.ToList();

foreach (var p in list)
{
    egg.Logger.Info($"{{id: \"{p.Id}\", Age: {p.Age}, Detail: \"{p.Detail}\"}}", "People");
}

Random rnd = new Random();
var people2 = new People2()
{
    Age = rnd.Next(100),
    Detail = "OK"
};
people2Repository.Insert(people2);
//context.People2s.Add(people);
//context.SaveChanges();