using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {
    /// <summary>
    /// 脚本存储池
    /// </summary>
    public class ScriptMemeryPool : IDisposable {
        /// <summary>
        /// 获取当前索引器
        /// </summary>
        public long Indexer { get; private set; }

        /// <summary>
        /// 获取一个空值
        /// </summary>
        public MemeryUnits.None None { get; private set; }

        // 列表
        private List<ScriptMemeryItem> list = new List<ScriptMemeryItem>();

        // 注册单元
        internal ScriptMemeryItem Reg(MemeryUnits.Unit unit) {
            ScriptMemeryItem item = new ScriptMemeryItem() {
                Handle = ++Indexer,
                ParentHandle = 0,
                MemeryUnit = unit
            };
            list.Add(item);
            return item;
        }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public MemeryUnits.Unit GetUnitByHandle(long handle) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i].Handle == handle) return list[i].MemeryUnit;
            }
            return this.None;
        }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public ScriptMemeryItem GetMemeryByHandle(long handle) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i].Handle == handle) return list[i];
            }
            return null;
        }

        /// <summary>
        /// 获取存储单元
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public List<ScriptMemeryItem> GetMemeriesByParentHandle(long handle) {
            List<ScriptMemeryItem> res = new List<ScriptMemeryItem>();
            for (int i = 0; i < list.Count; i++) {
                if (list[i].Handle == handle) res.Add(list[i]);
            }
            return res;
        }

        /// <summary>
        /// 实例化一个新的脚本存储池
        /// </summary>
        public ScriptMemeryPool() {
            this.Indexer = 0;
            this.None = new MemeryUnits.None(this);
            this.None.Handle = 0;
        }

        #region [=====创建各种对象=====]

        /// <summary>
        /// 创建一个布尔型数据
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateBoolean(bool val, long parent = 0) {
            MemeryUnits.Boolean unit = new MemeryUnits.Boolean(this, val);
            ScriptMemeryItem item = GetMemeryByHandle(unit.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个数值型存储
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateNumber(double val, long parent = 0) {
            MemeryUnits.Number unit = new MemeryUnits.Number(this, val);
            ScriptMemeryItem item = GetMemeryByHandle(unit.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个数值型存储
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateNumber(int val, long parent = 0) {
            MemeryUnits.Number unit = new MemeryUnits.Number(this, val);
            ScriptMemeryItem item = GetMemeryByHandle(unit.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个数值型存储
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateNumber(long val, long parent = 0) {
            MemeryUnits.Number unit = new MemeryUnits.Number(this, val);
            ScriptMemeryItem item = GetMemeryByHandle(unit.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个数值型存储
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateNumber(float val, long parent = 0) {
            MemeryUnits.Number unit = new MemeryUnits.Number(this, val);
            ScriptMemeryItem item = GetMemeryByHandle(unit.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个数值型存储
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateNumber(byte val, long parent = 0) {
            MemeryUnits.Number unit = new MemeryUnits.Number(this, val);
            ScriptMemeryItem item = GetMemeryByHandle(unit.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个字符串存储
        /// </summary>
        /// <param name="val"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateString(string val, long parent = 0) {
            MemeryUnits.String unit = new MemeryUnits.String(this, val);
            ScriptMemeryItem item = GetMemeryByHandle(unit.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(egg.Strings arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Count; i++) {
                list.Add(this.CreateString(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(string[] arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Length; i++) {
                list.Add(this.CreateString(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(double[] arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Length; i++) {
                list.Add(this.CreateNumber(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(float[] arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Length; i++) {
                list.Add(this.CreateNumber(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(long[] arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Length; i++) {
                list.Add(this.CreateNumber(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(int[] arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Length; i++) {
                list.Add(this.CreateNumber(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(byte[] arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Length; i++) {
                list.Add(this.CreateNumber(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个列表存储
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateList(bool[] arr, long parent = 0) {
            MemeryUnits.List list = new MemeryUnits.List(this);
            for (int i = 0; i < arr.Length; i++) {
                list.Add(this.CreateBoolean(arr[i], list.Handle).MemeryUnit);
            }
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个对象存储
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateObject(long parent = 0) {
            MemeryUnits.Object list = new MemeryUnits.Object(this);
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        /// <summary>
        /// 创建一个原生对象存储
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ScriptMemeryItem CreateNavtiveObject(object obj, long parent = 0) {
            MemeryUnits.NativeObject list = new MemeryUnits.NativeObject(this, obj);
            ScriptMemeryItem item = GetMemeryByHandle(list.Handle);
            item.ParentHandle = parent;
            return item;
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            for (int i = 0; i < list.Count; i++) {
                list[i].Dispose();
            }
            list.Clear();
            list = null;
            //throw new NotImplementedException();
        }
    }
}
