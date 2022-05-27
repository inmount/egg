using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 数值对象操作器
    /// </summary>
    public class JsonBoolOperator {

        private JsonUnit _json;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="jsonUnit"></param>
        public JsonBoolOperator(JsonUnit jsonUnit) {
            _json = jsonUnit;
        }

        /// <summary>
        /// 获取或设置数值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool this[int index] {
            get {
                if (_json.Count <= index + 1) {
                    return _json.Bool(index);
                }
                return false;
            }
            set {
                _json.Bool(index, value);
            }
        }

        /// <summary>
        /// 获取或设置数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool this[string key] {
            get {
                if (_json.Keys.Contains(key)) {
                    return _json.Bool(key);
                }
                return false;
            }
            set {
                _json.Bool(key, value);
            }
        }

    }
}
