using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark {
    /// <summary>
    /// 脚本块
    /// </summary>
    public class Scrope {

        // 段集合
        private egg.KeyValues<Scrope> scropes;

        /// <summary>
        /// 获取脚本引擎
        /// </summary>
        public Engine Engine { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="engine"></param>
        public Scrope(Engine engine) {
            this.Engine = engine;
        }

    }
}
