using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {

    /// <summary>
    /// 参数集合
    /// </summary>
    public class Params : List<ProcessUnits.Unit> {

        /// <summary>
        /// 获取关联函数
        /// </summary>
        public MemeryUnits.Function Function { get; internal set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="fn"></param>
        public Params() { }

        /// <summary>
        /// 添加函数
        /// </summary>
        /// <returns></returns>
        public ProcessUnits.Pointer AddFunction(string name, Params args = null) {
            MemeryUnits.Function obj = new MemeryUnits.Function(this.Function.Engine, this.Function, name, args);
            ProcessUnits.Pointer ptr = new ProcessUnits.Pointer(this.Function, this.Function.Memery.AddFunction(obj));
            this.Add(ptr);
            return ptr;
        }

        /// <summary>
        /// 添加定义
        /// </summary>
        /// <returns></returns>
        public ProcessUnits.Define AddDefine(string name) {
            ProcessUnits.Define obj = new ProcessUnits.Define(this.Function, name);
            this.Add(obj);
            return obj;
        }

        /// <summary>
        /// 添加数值
        /// </summary>
        /// <returns></returns>
        public ProcessUnits.Pointer AddNumber(double val) {
            ProcessUnits.Pointer ptr = new ProcessUnits.Pointer(this.Function, this.Function.Memery.AddNumber(val));
            this.Add(ptr);
            return ptr;
        }

        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <returns></returns>
        public ProcessUnits.Pointer AddString(string val) {
            ProcessUnits.Pointer ptr = new ProcessUnits.Pointer(this.Function, this.Function.Memery.AddString(val));
            this.Add(ptr);
            return ptr;
        }

    }
}
