using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 对象单元
    /// </summary>
    public class JsonObject : JsonUnit {

        private egg.KeyList<JsonUnit> _list;

        /// <summary>
        /// 对象实例化
        /// </summary>
        public JsonObject(JsonUnit parent=null) : base(UnitType.Object, parent) {
            _list = new KeyList<JsonUnit>();
        }

        /// <summary>
        /// 获取子对象键值集合
        /// </summary>
        /// <returns></returns>
        protected override ICollection<string> OnGetKeys() {
            return _list.Keys;
        }

        /// <summary>
        /// 获取子对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override JsonUnit OnGetChildItem(string key) {
            if (!_list.ContainsKey(key)) return null;
            return _list[key];
        }

        /// <summary>
        /// 设置子对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="unit"></param>
        protected override void OnSetChildItem(string key, JsonUnit unit) {
            // 当子对象已经存在，则先销毁原有对象
            if (_list.ContainsKey(key)) {
                _list[key].Dispose();
            }
            _list[key] = unit;
        }

        /// <summary>
        /// 获取Json字符串
        /// </summary>
        /// <returns></returns>
        protected override string OnGetJsonString() {
            bool isFirst = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var item in _list) {
                if (isFirst) {
                    isFirst = false;
                } else {
                    sb.Append(",");
                }
                sb.AppendFormat("\"{0}\":{1}", item.Key, item.Value.ToJsonString());
            }
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            _list.Dispose();
            base.OnDispose();
        }

    }
}
