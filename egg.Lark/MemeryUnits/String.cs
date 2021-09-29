using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class String : Unit {

        /// <summary>
        /// 获取值
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="val"></param>
        public String(string val) : base(UnitTypes.String) {
            this.Value = val;
        }
    }
}
