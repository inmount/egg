// See https://aka.ms/new-console-template for more information

using Egg;
using Egg.Test.Console;

Console.WriteLine("Hello, World!");

ClsTestObject? obj = null;
var obj1 = obj ?? new ClsTestObject();
ClsTestObject? obj2 = null;
var obj3 = obj2?.ToNotNull<ClsTestObject>();

Console.WriteLine("a=" + obj1?.a);
Console.WriteLine("obj2=" + obj2?.IsNull());
Console.WriteLine("obj3.a=" + obj3?.a);

var now = egg.Time.Now;
var day30 = now.AddDays(30);
Console.WriteLine(now.ToDateTimeString());
Console.WriteLine(now.ToUnixTimeSeconds());
var now2 = egg.Time.Parse(now.ToUnixTimeSeconds());
Console.WriteLine(now2.ToDateTimeString());
var ts = day30 - now;
Console.WriteLine(ts);
var dt2 = egg.Time.Parse(day30.ToUnixTimeSeconds());
Console.WriteLine(dt2.ToDateTimeString());
var ts2 = dt2 - now;
Console.WriteLine(ts2);