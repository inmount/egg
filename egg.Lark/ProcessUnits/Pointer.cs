using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.ProcessUnits {

    /// <summary>
    /// 存储指针
    /// </summary>
    public class Pointer : Unit {

        /// <summary>
        /// 获取关联的存储器
        /// </summary>
        public MemeryUnits.Function Function { get; private set; }

        /// <summary>
        /// 获取索引
        /// </summary>
        public long IntPtr { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        public Pointer(MemeryUnits.Function fn, long pointer) : base(fn.MemeryPool, UnitTypes.Pointer) {
            this.Function = fn;
            this.IntPtr = pointer;
        }

        /// <summary>
        /// 获取一个空指针
        /// </summary>
        public static Pointer None { get { return new Pointer(null, 0); } }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <returns></returns>
        protected override MemeryUnits.Unit OnGetMemeryUnit() {
            return this.MemeryPool.GetUnitByHandle(this.IntPtr);
        }

    }
}
