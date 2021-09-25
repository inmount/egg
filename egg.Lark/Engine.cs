using System;

namespace egg.Lark {

    /// <summary>
    /// 百灵鸟脚本引擎
    /// </summary>
    public class Engine {

        // 段集合
        private egg.KeyValues<Scrope> scropes;

        public Engine() {
            scropes = new KeyValues<Scrope>();
        }

        /// <summary>
        /// 添加一个新的程序段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Scrope AddScrope(string name) {
            if (scropes.ContainsKey(name)) throw new Exception("程序段名称已经存在");
            Scrope scrope = new Scrope(this);
            scropes[name] = scrope;
            return scrope;
        }

    }
}
