﻿using System;
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

        /// <summary>
        /// 创建一个新的空对象
        /// </summary>
        /// <returns></returns>
        public static Number Create(double val) { return new Number(val); }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns></returns>
        protected override bool OnGetBoolean() { return this.Value >= 1; }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        protected override double OnGetNumber() { return this.Value; }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetString() { return this.Value.ToString(); }


    }
}