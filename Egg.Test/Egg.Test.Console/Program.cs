// See https://aka.ms/new-console-template for more information

using Egg;
using Egg.BarCode;
using Egg.EFCore;
using Egg.Log.Loggers;
using Egg.Test.Console;
using Egg.Test.Console.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SqliteEFCore.DbContexts;
using System.Data;
using System.Diagnostics;

// 注册字符编码
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// 初始化日志管理器
egg.Logger.GetCurrentLogger()
    .Use((string message) =>
    {
        Debug.WriteLine(message);
    })
    .Use(new FileLogger(egg.IO.GetExecutionPath("logs")))
    .Use<ConsoleLogger>();

Console.WriteLine(egg.Security.DESEncrypt("qwertyuiop", "12345678"));
Console.WriteLine(egg.Security.DESDecrypt("1UnHSOMKjAjGkwR2Xgh11g==", "12345678"));

// 运行测试
Test.Run();

// 运行Data测试
//Egg.Test.Console.Data.Test.Run();

//Rules rules = new Rules();
//rules.Use(new FixedString("XS-"))
//    .Use(new DateTimeRule("yyyyMMdd"))
//    .Use(new FixedString("-"))
//    .Use(new SerialNoRules()
//        .Use(new SerialNoRule() { Min = 1, Max = 105, Space = 4 })
//        .Use(new SerialNoRule() { Min = 106, Step = 2, Space = 4 })
//        )
//    .Use(new FixedString("-"))
//    .Use(new RandomStringRule("1234567890", 4));

//// 生成条码
//for (int i = 101; i <= 112; i++)
//{
//    Console.WriteLine(egg.BarCodeGenerator.Generate(rules, i));
//}

//RuleCollection rc = new RuleCollection();
//rc.Use("dt", new DateTimeRule("yyyyMMdd"))
//    .Use("sn", new SerialNoRules()
//        .Use(new SerialNoRule() { Min = 1, Max = 105, Space = 4 })
//        .Use(new SerialNoRule() { Min = 106, Step = 2, Space = 4 })
//        );
//People2 p2 = new People2();
//p2.Age = 50;

//// 生成条码
//for (int i = 101; i <= 112; i++)
//{
//    Console.WriteLine(egg.BarCodeGenerator.Generate("XS$(Age)-$(dt)-$(sn)", rc, i, p2));
//}


//ClsTestObject? obj = null;
//var obj1 = obj ?? new ClsTestObject();
//ClsTestObject? obj2 = null;
//var obj3 = obj2?.ToNotNull<ClsTestObject>();

//egg.Logger.Debug("a=" + obj1?.a);
//egg.Logger.Info("obj2=" + obj2?.IsNull());
//egg.Logger.Warn("obj3.a=" + obj3?.a, "ClsTestObject");

//byte bb = 1 | 2 | 4 | 8;
//egg.Logger.Info($"~{bb}={~bb} => {(bb & ~4)}");

//Random rand = new Random();
//for (int i = 0; i < 10; i++)
//{
//    var num = rand.NextDouble();
//    string sha512 = num.ToString().GetSha512();
//    egg.Logger.Info($"{num} -> [{sha512.Length}]{sha512}");
//}

// 测试EFCore
//Egg.Test.Console.EFCore.Test.Run();

// 测试VirtualDisk
//Egg.Test.Console.VirtualDisk.Test.Run();

//var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
//optionsBuilder.UseSqlite($"Data Source=D:\\core.db");

//var context = new DbContext(optionsBuilder.Options);
//var people2Repository = new RepositoryBase<People2, string>(context);
//egg.Logger.Info(context.Database.ProviderName, "DbContext");
//egg.Logger.Info(context.GetDbType(), "DbContext");
//egg.Logger.Info(context.Peoples.ToQueryString(), "DbContext");
//egg.Logger.Info("EnsureCreated:" + context.EnsureCreatedSqlite(), "Sqlite");
//egg.Logger.Info("EnsureCreated:" + context.EnsureCreatedPostgreSQL(), "PostgreSQL");
//egg.Logger.Info(context.Database.GetDbConnection().DataSource, "DbContext");

//SqliteCreater sqlite = new SqliteCreater();
//egg.Logger.Info(sqlite.GetEnsureCreatedSql(context), "DbContext");

//PostgreSQLCreater postgreSQL = new PostgreSQLCreater();
//egg.Logger.Info(postgreSQL.GetEnsureCreatedSql(context), "DbContext");

//var query = from p in people2Repository.Query()
//            select p;

//Console.WriteLine(query.ToQueryString());
//var list = query.ToList();

//foreach (var p in list)
//{
//    egg.Logger.Info($"{{id: \"{p.Id}\", Age: {p.Age}, Detail: \"{p.Detail}\"}}", "People");
//}

//Random rnd = new Random();
//var people2 = new People2()
//{
//    Age = rnd.Next(100),
//    Detail = "OK"
//};
//people2Repository.Insert(people2);
//context.People2s.Add(people);
//context.SaveChanges();

// 测试lark
Egg.Test.Console.Lark.Test.Run();