using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.Imports {
    // json操作对象
    internal class Math {
        // 执行注册
        internal static void Reg(ScriptEngine engine) {
            // 建立随机种子
            Random r = new Random();
            // 设置json内置对象
            MemeryUnits.Object math = engine.MemeryPool.CreateObject().ToObject();
            engine.SetProcessVariable("math", math);
            // 绝对值
            math["abs"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.abs";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Abs(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 反余弦值
            math["acos"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.acos";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Acos(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 反正弦值
            math["asin"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.asin";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Asin(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 反正切值
            math["atan"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.atan";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Atan(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 从x 坐标轴到点的角度
            math["atan2"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.atan2";
                var y = args["y"];
                var x = args["x"];
                if (y.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'y'不支持类型{y.UnitType.ToString()}");
                if (x.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'x'不支持类型{x.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Atan2(y.ToNumber(), x.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("y", "x"));
            // 将数字向上舍入为最接近的整数
            math["ceiling"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.ceiling";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Ceiling(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 余弦值
            math["cos"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.cos";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Cos(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 计算指数值
            math["exp"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.exp";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Exp(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 将数字向下舍入为最接近的整数
            math["floor"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.floor";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Floor(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 计算自然对数
            math["log"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.log";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Log(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 返回两个整数中较大的一个
            math["max"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.max";
                var num1 = args["num1"];
                var num2 = args["num2"];
                if (num1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num1'不支持类型{num1.UnitType.ToString()}");
                if (num2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num2'不支持类型{num2.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Max(num1.ToNumber(), num2.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num1", "num2"));
            // 返回两个整数中较大的一个
            math["min"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.min";
                var num1 = args["num1"];
                var num2 = args["num2"];
                if (num1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num1'不支持类型{num1.UnitType.ToString()}");
                if (num2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num2'不支持类型{num2.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Min(num1.ToNumber(), num2.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num1", "num2"));
            // 反余弦值
            math["pow"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.pow";
                var x = args["x"];
                var y = args["y"];
                if (x.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'x'不支持类型{x.UnitType.ToString()}");
                if (y.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'y'不支持类型{y.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Pow(x.ToNumber(), y.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("x","y"));
            // 返回一个0.0 与1.0 之间的伪随机数。  
            math["random"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                //string fnName = "math.random";
                return engine.MemeryPool.CreateNumber(r.NextDouble(), math.Handle).MemeryUnit;
            }, egg.Strings.Create());
            // 四舍五入为最接近的整数
            math["round"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.round";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Round(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 计算正弦值
            math["sin"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.sin";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Sin(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 计算平方根
            math["sqrt"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.sqrt";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Sqrt(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 计算正切值
            math["tan"] = new MemeryUnits.NativeFunction(engine.MemeryPool, (egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.tan";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return engine.MemeryPool.CreateNumber(System.Math.Tan(num.ToNumber()), math.Handle).MemeryUnit;
            }, egg.Strings.Create("num"));
            // 欧拉(Euler) 常数，自然对数的底（大约为2.718）
            math["E"] = engine.MemeryPool.CreateNumber(System.Math.E, math.Handle).MemeryUnit;
            // 反余弦值
            math["PI"] = engine.MemeryPool.CreateNumber(System.Math.PI, math.Handle).MemeryUnit;
        }
    }
}
