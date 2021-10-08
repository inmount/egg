using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {
    /// <summary>
    /// 内置函数集合
    /// </summary>
    public static class Functions {
        /// <summary>
        /// 注册器
        /// </summary>
        /// <param name="engine"></param>
        public static void Reg(Engine engine) {
            // 转化为数值
            engine.RegFunction("num", (egg.KeyValues<MemeryUnits.Unit> args) => {
                var n = args["n"];
                if (n.UnitType != MemeryUnits.UnitTypes.String) throw new Exception("num函数的参数'n'只支持字符串");
                return new MemeryUnits.Number(((MemeryUnits.String)n).Value.ToDouble());
            }, egg.Strings.Create("n"));
            // 转化为数值
            engine.RegFunction("str", (egg.KeyValues<MemeryUnits.Unit> args) => {
                var s = args["s"];
                if (s.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("str函数的参数's'只支持数值");
                return new MemeryUnits.Number(((MemeryUnits.String)s).Value.ToDouble());
            }, egg.Strings.Create("s"));
        }
    }
}
