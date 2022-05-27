using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 数组单元
    /// </summary>
    public class JsonArray : JsonUnit {

        private List<JsonUnit> _list;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JsonArray(JsonUnit parent = null) : base(UnitType.Array, parent) {
            _list = new List<JsonUnit>();
        }

        /// <summary>
        /// 获取索引对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override JsonUnit OnGetArrayItem(int index) {
            if (index >= _list.Count) return null;
            return _list[index];
        }

        /// <summary>
        /// 设置索引对象
        /// </summary>
        /// <param name="index"></param>
        /// <param name="unit"></param>
        protected override void OnSetArrayItem(int index, JsonUnit unit) {
            if (index < _list.Count) {
                // 索引存在，则释放原有的资源
                _list[index].Dispose();
                _list[index] = unit;
            } else {
                // 索引不存在，则添加索引至当前索引值
                for (int i = _list.Count; i < index; i++) {
                    _list.Add(new JsonString(this));
                }
                _list.Add(unit);
            }
        }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetJsonString() {
            bool isFirst = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in _list) {
                if (isFirst) {
                    isFirst = false;
                } else {
                    sb.Append(",");
                }
                sb.Append(item.ToJsonString());
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// 获取索引对象数量集合
        /// </summary>
        /// <returns></returns>
        protected override int OnGetItemCount() {
            return _list.Count;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {

            for (int i = 0; i < _list.Count; i++) {
                _list[i].Dispose();
            }
            _list.Clear();
            _list = null;

            base.OnDispose();
        }

    }
}
