using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {

    /// <summary>
    /// 记忆存储器
    /// </summary>
    public class Memery : IDisposable {

        private List<MemeryUnits.Unit> list = new List<MemeryUnits.Unit>();

        /// <summary>
        /// 获取关联函数
        /// </summary>
        public MemeryUnits.Function Function { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="fn"></param>
        public Memery(MemeryUnits.Function fn) {
            this.Function = fn;
            list = new List<MemeryUnits.Unit>();
        }

        /// <summary>
        ///  获取或设置单元
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MemeryUnits.Unit this[int index] {
            get {
                if (index >= list.Count) return new MemeryUnits.None();
                return list[index];
            }
            set {
                if (index >= list.Count) {
                    for (int i = list.Count; i < index; i++) {
                        list.Add(new MemeryUnits.None());
                    }
                    list.Add(value);
                } else {
                    list[index] = value;
                }
            }
        }

        /// <summary>
        /// 清空存储
        /// </summary>
        public void Clear() {
            list.Clear();
        }

        /// <summary>
        /// 添加一个数值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int AddNumber(double val) {
            list.Add(new MemeryUnits.Number(val));
            return list.Count - 1;
        }

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int AddString(string val) {
            list.Add(new MemeryUnits.String(val));
            return list.Count - 1;
        }

        /// <summary>
        /// 添加一个列表
        /// </summary>
        /// <returns></returns>
        public int AddList() {
            list.Add(new MemeryUnits.List());
            return list.Count - 1;
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <returns></returns>
        public int AddObject() {
            list.Add(new MemeryUnits.Object());
            return list.Count - 1;
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <returns></returns>
        public int AddFunction(MemeryUnits.Function fn) {
            list.Add(fn);
            return list.Count - 1;
        }

        public void Dispose() {
            this.Clear();
            //throw new NotImplementedException();
        }
    }
}
