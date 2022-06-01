using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {
    /// <summary>
    /// 脚本存储池元素
    /// </summary>
    public class ScriptMemeryItem : IDisposable {
        /// <summary>
        /// 获取句柄
        /// </summary>
        public long Handle { get; internal set; }

        /// <summary>
        /// 获取父对象句柄
        /// </summary>
        public long ParentHandle { get; internal set; }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        public MemeryUnits.Unit MemeryUnit { get; internal set; }

        /// <summary>
        /// 获取存储单元类型
        /// </summary>
        public MemeryUnits.UnitTypes MemeryUnitType { get { return this.MemeryUnit.UnitType; } }

        /// <summary>
        /// 获取存储单元类型
        /// </summary>
        public string MemeryUnitTypeName { get { return this.MemeryUnit.UnitType.ToString(); } }

        #region [=====快速判断接口=====]

        /// <summary>
        /// 判断是否为函数
        /// </summary>
        /// <returns></returns>
        public bool IsFunction() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.Function || this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.NativeFunction; }

        /// <summary>
        /// 判断是否为对象
        /// </summary>
        /// <returns></returns>
        public bool IsObject() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.Object; }

        /// <summary>
        /// 判断是否为原生对象
        /// </summary>
        /// <returns></returns>
        public bool IsNaviteObject() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.NativeObject; }

        /// <summary>
        /// 判断是否为列表
        /// </summary>
        /// <returns></returns>
        public bool IsList() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.List; }

        /// <summary>
        /// 判断是否为字符串
        /// </summary>
        /// <returns></returns>
        public bool IsString() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.String; }


        /// <summary>
        /// 判断是否为数值
        /// </summary>
        /// <returns></returns>
        public bool IsNumber() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.Number; }

        /// <summary>
        /// 判断是否为布尔
        /// </summary>
        /// <returns></returns>
        public bool IsBoolean() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.Boolean; }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsNone() { return this.MemeryUnit.UnitType == MemeryUnits.UnitTypes.None; }

        #endregion

        #region [=====快速处理接口=====]

        /// <summary>
        /// 获取布尔值
        /// </summary>
        /// <returns></returns>
        public bool ToBoolean() { return MemeryUnit.ToBoolean(); }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <returns></returns>
        public double ToNumber() { return MemeryUnit.ToNumber(); }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        public new string ToString() { return MemeryUnit.ToString(); }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public MemeryUnits.Object ToObject() {
            if (MemeryUnit.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"存储单元类型'{MemeryUnit.UnitType.ToString()}'不支持转化为对象。");
            return (MemeryUnits.Object)MemeryUnit;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public MemeryUnits.List ToList() {
            if (MemeryUnit.UnitType != MemeryUnits.UnitTypes.List) throw new Exception($"存储单元类型'{MemeryUnit.UnitType.ToString()}'不支持转化为列表。");
            return (MemeryUnits.List)MemeryUnit;
        }

        /// <summary>
        /// 获取函数
        /// </summary>
        /// <returns></returns>
        public MemeryUnits.Function ToFunction() {
            if (MemeryUnit.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception($"存储单元类型'{MemeryUnit.UnitType.ToString()}'不支持转化为函数。");
            return (MemeryUnits.Function)MemeryUnit;
        }

        /// <summary>
        /// 获取函数
        /// </summary>
        /// <returns></returns>
        public MemeryUnits.NativeFunction ToNativeFunction() {
            if (MemeryUnit.UnitType != MemeryUnits.UnitTypes.NativeFunction) throw new Exception($"存储单元类型'{MemeryUnit.UnitType.ToString()}'不支持转化为函数。");
            return (MemeryUnits.NativeFunction)MemeryUnit;
        }

        /// <summary>
        /// 获取函数
        /// </summary>
        /// <returns></returns>
        public object ToNativeObject() {
            if (MemeryUnit.UnitType != MemeryUnits.UnitTypes.NativeObject) throw new Exception($"存储单元类型'{MemeryUnit.UnitType.ToString()}'不支持转化为函数。");
            return ((MemeryUnits.NativeObject)MemeryUnit).Object;
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            this.MemeryUnit.Dispose();
        }
    }
}
