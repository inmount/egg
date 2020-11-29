using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 数值单元类型
    /// </summary>
    public class JsonNumber : JsonUnit {

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JsonNumber(JsonUnit parent) : base(UnitType.Number, parent) {
            this.Value = 0;
        }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected override double OnGetNumber() {
            return this.Value;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetString() {
            return "" + this.Value;
        }

        /// <summary>
        /// 设置数值
        /// </summary>
        /// <param name="value"></param>
        protected override void OnSetNumber(double value) {
            this.Value = value;
        }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetJsonString() {
            return "" + this.Value;
        }

    }
}
