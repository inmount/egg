// See https://aka.ms/new-console-template for more information

using Egg;
using Egg.Log.Loggers;
using Egg.Test.Console;

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