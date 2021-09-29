using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class Number : Unit {

        /// <summary>
        /// 获取值
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="val"></param>
        public Number(double val) : base(UnitTypes.Number) {
            this.Value = val;
        }
    }
}
