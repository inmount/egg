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
            MemeryUnits.Object math = new MemeryUnits.Object();
            engine.SetProcessVariable("math", math);
            // 绝对值
            math["abs"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.abs";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Abs(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 反余弦值
            math["acos"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.acos";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Acos(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 反正弦值
            math["asin"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.asin";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Asin(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 反正切值
            math["atan"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.atan";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Atan(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 从x 坐标轴到点的角度
            math["atan2"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.atan2";
                var y = args["y"];
                var x = args["x"];
                if (y.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'y'不支持类型{y.UnitType.ToString()}");
                if (x.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'x'不支持类型{x.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Atan2(y.ToNumber(), x.ToNumber()));
            }, egg.Strings.Create("y", "x"));
            // 将数字向上舍入为最接近的整数
            math["ceiling"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.ceiling";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Ceiling(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 余弦值
            math["cos"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.cos";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Cos(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 计算指数值
            math["exp"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.exp";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Exp(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 将数字向下舍入为最接近的整数
            math["floor"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.floor";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Floor(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 计算自然对数
            math["log"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.log";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Log(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 返回两个整数中较大的一个
            math["max"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.max";
                var num1 = args["num1"];
                var num2 = args["num2"];
                if (num1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num1'不支持类型{num1.UnitType.ToString()}");
                if (num2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num2'不支持类型{num2.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Max(num1.ToNumber(), num2.ToNumber()));
            }, egg.Strings.Create("num1", "num2"));
            // 返回两个整数中较大的一个
            math["min"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.min";
                var num1 = args["num1"];
                var num2 = args["num2"];
                if (num1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num1'不支持类型{num1.UnitType.ToString()}");
                if (num2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num2'不支持类型{num2.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Min(num1.ToNumber(), num2.ToNumber()));
            }, egg.Strings.Create("num1", "num2"));
            // 反余弦值
            math["pow"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.pow";
                var x = args["x"];
                var y = args["y"];
                if (x.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'x'不支持类型{x.UnitType.ToString()}");
                if (y.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'y'不支持类型{y.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Pow(x.ToNumber(), y.ToNumber()));
            }, egg.Strings.Create("x","y"));
            // 返回一个0.0 与1.0 之间的伪随机数。  
            math["random"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                //string fnName = "math.random";
                return MemeryUnits.Number.Create(r.NextDouble());
            }, egg.Strings.Create());
            // 四舍五入为最接近的整数
            math["round"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.round";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Round(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 计算正弦值
            math["sin"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.sin";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Sin(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 计算平方根
            math["sqrt"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.sqrt";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Sqrt(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 计算正切值
            math["tan"] = new MemeryUnits.NativeFunction((egg.KeyValues<MemeryUnits.Unit> args) => {
                string fnName = "math.tan";
                var num = args["num"];
                if (num.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"{fnName}函数的参数'num'不支持类型{num.UnitType.ToString()}");
                return MemeryUnits.Number.Create(System.Math.Tan(num.ToNumber()));
            }, egg.Strings.Create("num"));
            // 欧拉(Euler) 常数，自然对数的底（大约为2.718）
            math["E"] = MemeryUnits.Number.Create(System.Math.E);
            // 反余弦值
            math["PI"] = MemeryUnits.Number.Create(System.Math.PI);
        }
    }
}
