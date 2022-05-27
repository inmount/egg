using System;
using System.Collections.Generic;
using System.Text;

namespace egg.Json {

    /// <summary>
    /// 数值对象操作器
    /// </summary>
    public class JsonStringOperator {

        private JsonUnit _json;

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="jsonUnit"></param>
        public JsonStringOperator(JsonUnit jsonUnit) {
            _json = jsonUnit;
        }

        /// <summary>
        /// 获取或设置数值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index] {
            get {
                if (_json.Count <= index + 1) {
                    return _json.String(index);
                }
                return null;
            }
            set {
                _json.String(index, value);
            }
        }

        /// <summary>
        /// 获取或设置数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get {
                if (_json.Keys.Contains(key)) {
                    return _json.String(key);
                }
                return null;
            }
            set {
                _json.String(key, value);
            }
        }

    }
}
