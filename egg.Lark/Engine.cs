using System;

namespace egg.Lark {

    /// <summary>
    /// 百灵鸟脚本引擎
    /// </summary>
    public class Engine : IDisposable {

        // 主函数
        private MemeryUnits.Function main;
        private egg.KeyValues<MemeryUnits.Unit> vars;

        /// <summary>
        /// 实例化对象
        /// </summary>
        public Engine() {
            main = new MemeryUnits.Function(this, null, "step");
            vars = new KeyValues<MemeryUnits.Unit>();
        }

        /// <summary>
        /// 添加函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public MemeryUnits.Function AddFunction(string name, Params args = null) {
            return (MemeryUnits.Function)main.Params.AddFunction(name, args).GetMemeryUnit();
        }

        /// <summary>
        /// 设置初始化变量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetVariable(string name, MemeryUnits.Unit value) {
            vars[name] = value;
        }

        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public MemeryUnits.Unit GetVariable(string name) {
            return main.GetVarValue(name);
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        public void Execute() {
            main.Execute(vars);
        }

        public void Dispose() {
            main.Dispose();
        }
    }
}
