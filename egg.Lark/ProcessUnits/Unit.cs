using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.ProcessUnits {

    /// <summary>
    /// 单元类型
    /// </summary>
    public enum UnitTypes {
        /// <summary>
        /// 空值
        /// </summary>
        None = 0x00,
        /// <summary>
        /// 指针
        /// </summary>
        Pointer = 0x01,
        /// <summary>
        /// 定义
        /// </summary>
        Define = 0x11,
        /// <summary>
        /// 代码
        /// </summary>
        Function = 0x21
    }

    /// <summary>
    /// 处理单元
    /// </summary>
    public abstract class Unit {
        /// <summary>
        /// 单元类型
        /// </summary>
        public UnitTypes UnitType { get; private set; }

        /// <summary>
        /// 获取存储池
        /// </summary>
        public ScriptMemeryPool MemeryPool { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="unitType"></param>
        public Unit(ScriptMemeryPool pool, UnitTypes unitType) {
            this.MemeryPool = pool;
            this.UnitType = unitType;
        }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <returns></returns>
        protected virtual MemeryUnits.Unit OnGetMemeryUnit() { return this.MemeryPool.None; }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <returns></returns>
        public MemeryUnits.Unit GetMemeryUnit() { return OnGetMemeryUnit(); }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <returns></returns>
        public ScriptMemeryItem GetMemeryItem() { return OnGetMemeryUnit().ToMemeryItem(); }
    }
}
