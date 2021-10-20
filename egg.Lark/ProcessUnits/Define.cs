﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.ProcessUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class Define : Unit {

        /// <summary>
        /// 获取关联的存储器
        /// </summary>
        public MemeryUnits.Function Function { get; private set; }

        /// <summary>
        /// 获取值
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="name"></param>
        public Define(MemeryUnits.Function fn, string name) : base(UnitTypes.Define) {
            this.Function = fn;
            this.Name = name;
        }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <returns></returns>
        protected override MemeryUnits.Unit OnGetMemeryUnit() {
            int idx = this.Name.IndexOf('.');
            if (idx > 0) {
                string name = this.Name.Substring(0, idx);
                string cName = this.Name.Substring(idx + 1);
                if (cName.IsEmpty()) throw new Exception($"不支持空名称属性");
                // 处理空指针
                if (!eggs.IsNull(this.Function.Memery)) {
                    if (this.Function.CheckVar(name)) {
                        MemeryUnits.Unit obj = this.Function.GetVarValue(name);
                        if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{name}'并非对象");
                        return ((MemeryUnits.Object)obj)[cName];
                    }
                }
                //if (this.Function.Engine.CheckVariable(this.Name)) return this.Function.Engine.GetVariable(this.Name);
                throw new Exception($"变量'{name}'尚未赋值");
            } else {
                // 处理空指针
                if (!eggs.IsNull(this.Function.Memery)) {
                    if (this.Function.CheckVar(this.Name)) return this.Function.GetVarValue(this.Name);
                }
                //if (this.Function.Engine.CheckVariable(this.Name)) return this.Function.Engine.GetVariable(this.Name);
                throw new Exception($"变量'{this.Name}'尚未赋值");
            }
        }
    }
}
