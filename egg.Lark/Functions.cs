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
            engine.RegFunction("num", (List<MemeryUnits.Unit> args) => {
                if (args.Count != 1) throw new Exception("num函数只支持一个参数");
                if (args[0].UnitType != MemeryUnits.UnitTypes.String) throw new Exception("num函数的参数只支持字符串");
                return new MemeryUnits.Number(((MemeryUnits.String)args[0]).Value.ToDouble());
            });
            // 转化为数值
            engine.RegFunction("str", (List<MemeryUnits.Unit> args) => {
                if (args.Count != 1) throw new Exception("str函数只支持一个参数");
                if (args[0].UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("str函数的参数只支持数值");
                return new MemeryUnits.Number(((MemeryUnits.String)args[0]).Value.ToDouble());
            });
        }
    }
}
