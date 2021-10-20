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

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <returns></returns>
        public static String Create(string val) { return new String(val); }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns></returns>
        protected override bool OnGetBoolean() { return this.Value.ToLower() == "true"; }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected override double OnGetNumber() { return double.Parse(this.Value); }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetString() { return this.Value; }
    }
}
