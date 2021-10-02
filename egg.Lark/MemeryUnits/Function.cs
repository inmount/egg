using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Lark.MemeryUnits {
    /// <summary>
    /// 数值对象
    /// </summary>
    public class Function : Unit {

        // 获取中断标志
        private bool isBreak;

        // 获取返回标志
        private bool isReturn;

        // 变量集合
        private egg.KeyValues<MemeryUnits.Unit> vars;

        /// <summary>
        /// 获取值
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取值
        /// </summary>
        public Params Params { get; private set; }

        /// <summary>
        /// 获取关联引擎
        /// </summary>
        public Engine Engine { get; private set; }

        /// <summary>
        /// 获取值
        /// </summary>
        public Function Parent { get; private set; }

        /// <summary>
        /// 获取值
        /// </summary>
        public Memery Memery { get; private set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="parent"></param>
        public Function(Engine engine, Function parent, string name, Params args = null) : base(UnitTypes.Function) {
            this.Name = name;
            if (eggs.IsNull(args)) {
                this.Params = new Params();
            } else {
                this.Params = args;
            }
            this.Params.Function = this;
            this.Parent = parent;
            this.Engine = engine;
            this.Memery = new Memery(this);
            vars = new KeyValues<Unit>();
            isBreak = false;
            isReturn = false;
        }

        protected override void OnDispose() {
            base.OnDispose();
            this.Memery.Dispose();
            this.Params.Clear();
            vars.Clear();
        }

        // 初始化变量
        private void InitVars() {
            vars.Clear();
        }

        // 设置中断
        internal void SetBreak() {
            isBreak = true;
            if (this.Parent == null) return;
            if (this.Name != "for" && this.Name != "while") this.Parent.SetBreak();
        }

        // 设置中断
        internal void SetReturn() {
            isReturn = true;
            if (this.Parent == null) return;
            if (this.Name != "func") this.Parent.SetReturn();
        }

        // 设置变量值
        internal bool SetVarValue(string name, MemeryUnits.Unit value, bool isForcibly = false) {
            // 处理强制赋值
            if (isForcibly) {
                vars[name] = value;
                return true;
            }
            // 处理回溯赋值
            if (vars.ContainsKey(name)) {
                vars[name] = value;
                return true;
            } else {
                if (this.Parent == null) return false;
                return this.Parent.SetVarValue(name, value);
            }
        }

        // 获取变量值
        internal MemeryUnits.Unit GetVarValue(string name) {
            if (vars.ContainsKey(name)) {
                return vars[name];
            } else {
                if (this.Parent == null) throw new Exception($"变量'{name}'尚未赋值");
                return this.Parent.GetVarValue(name);
            }
        }

        // 检查变量
        internal bool CheckVar(string name) {
            if (vars.ContainsKey(name)) {
                return true;
            } else {
                if (this.Parent == null) return false;
                return this.Parent.CheckVar(name);
            }
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        public MemeryUnits.Unit Execute(egg.KeyValues<MemeryUnits.Unit> keys = null) {
            isBreak = false;
            isReturn = false;
            // 初始化变量
            this.InitVars();
            if (!eggs.IsNull(keys)) {
                foreach (var item in keys) {
                    vars[item.Key] = item.Value;
                }
            }
            switch (this.Name) {
                case "step": // 所有参数按顺序执行
                    #region [====顺序执行====]
                    for (int i = 0; i < this.Params.Count; i++) {
                        var fn = this.Params[i].GetMemeryUnit();
                        if (fn.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("step的参数必须为函数");
                        var res = ((MemeryUnits.Function)fn).Execute();
                        if (isBreak) return new MemeryUnits.None();
                        if (isReturn) return res;
                    }
                    #endregion
                    return new MemeryUnits.None();
                case "let": // 赋值操作
                    #region [====赋值操作====]
                    if (this.Params.Count != 2) throw new Exception("let函数只允许两个参数");
                    if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("let函数第一个参数只接受变量");
                    string name = ((ProcessUnits.Define)this.Params[0]).Name;
                    MemeryUnits.Unit value = this.Params[1].GetMemeryUnit();
                    if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                    if (eggs.IsNull(this.Parent)) throw new Exception("let函数未找到父函数");
                    if (!this.Parent.SetVarValue(name, value)) {
                        this.Parent.SetVarValue(name, value, true);
                    }
                    #endregion
                    return new MemeryUnits.None();
                case "func": // 函数定义操作
                    #region [====函数定义操作====]
                    Params args = new Params();
                    for (int i = 0; i < this.Params.Count - 1; i++) {
                        if (this.Params[i].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("func函数除最后一个参数，其他参数只接受变量");
                        args.Add(this.Params[i]);
                    }
                    if (this.Params[this.Params.Count - 1].GetMemeryUnit().UnitType != UnitTypes.Function) throw new Exception("func函数最后一个参数只接受函数");
                    args.Add(this.Params[this.Params.Count - 1]);
                    #endregion
                    return new MemeryUnits.Function(this.Engine, this, "", args);
                case "if": // 分支定义操作
                    #region [====分支定义操作====]
                    if (this.Params.Count < 2) throw new Exception("if函数最少包含两个参数");
                    if (this.Params.Count > 3) throw new Exception("if函数最多包含三个参数");
                    value = this.Params[0].GetMemeryUnit();
                    if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                    if (value.UnitType != UnitTypes.Number) throw new Exception("if函数条件参数需要定义为数值");
                    if (((MemeryUnits.Number)value).Value > 0) {
                        var func = this.Params[1].GetMemeryUnit();
                        if (func.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("if函数执行参数需要定义为函数");
                        var res = ((MemeryUnits.Function)func).Execute();
                        if (isReturn) return res;
                    } else {
                        if (this.Params.Count < 3) return new MemeryUnits.None();
                        var func = this.Params[2].GetMemeryUnit();
                        if (func.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("if函数执行参数需要定义为函数");
                        var res = ((MemeryUnits.Function)func).Execute();
                        if (isReturn) return res;
                    }
                    return new MemeryUnits.None();
                #endregion
                case "while": // 循环定义操作
                    #region [====循环定义操作====]
                    if (this.Params.Count != 2) throw new Exception("while函数必须要两个参数");
                    value = this.Params[0].GetMemeryUnit();
                    if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                    if (value.UnitType != UnitTypes.Number) throw new Exception("while函数条件参数需要定义为数值");
                    while (((MemeryUnits.Number)value).Value > 0) {
                        var func = this.Params[1].GetMemeryUnit();
                        if (func.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("while函数执行参数需要定义为函数");
                        var res = ((MemeryUnits.Function)func).Execute();
                        if (isReturn) return res;
                        if (isBreak) break;
                        // 读取下一次条件
                        value = this.Params[0].GetMemeryUnit();
                        if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                        if (value.UnitType != UnitTypes.Number) throw new Exception("while函数条件参数需要定义为数值");
                    }
                    return new MemeryUnits.None();
                #endregion
                case "for": // for循环定义操作
                    #region [====循环定义操作====]
                    if (this.Params.Count < 2) throw new Exception("for函数必须要五个参数");
                    KeyValues<Unit> lsArgs = new KeyValues<Unit>();
                    if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("for函数第一个参数只接受变量");
                    string forVar = ((ProcessUnits.Define)this.Params[0]).Name;
                    // 获取起始值
                    var forStart = this.Params[1].GetMemeryUnit();
                    if (forStart.UnitType == MemeryUnits.UnitTypes.Function) forStart = ((MemeryUnits.Function)forStart).Execute();
                    if (forStart.UnitType != UnitTypes.Number) throw new Exception("for函数起始条件参数需要定义为数值");
                    // 获取终止值
                    var forEnd = this.Params[2].GetMemeryUnit();
                    if (forEnd.UnitType == MemeryUnits.UnitTypes.Function) forEnd = ((MemeryUnits.Function)forEnd).Execute();
                    if (forEnd.UnitType != UnitTypes.Number) throw new Exception("for函数终止条件参数需要定义为数值");
                    // 获取步长值
                    var forStep = this.Params[3].GetMemeryUnit();
                    if (forStep.UnitType == MemeryUnits.UnitTypes.Function) forStep = ((MemeryUnits.Function)forStep).Execute();
                    if (forStep.UnitType != UnitTypes.Number) throw new Exception("for函数步长条件参数需要定义为数值");
                    for (double i = ((MemeryUnits.Number)forStart).Value; i <= ((MemeryUnits.Number)forEnd).Value; i += ((MemeryUnits.Number)forStep).Value) {
                        var func = this.Params[4].GetMemeryUnit();
                        if (func.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("if函数执行参数需要定义为函数");
                        lsArgs[forVar] = new MemeryUnits.Number(i);
                        var res = ((MemeryUnits.Function)func).Execute(lsArgs);
                        if (isReturn) return res;
                        if (isBreak) break;
                    }
                    return new MemeryUnits.None();
                #endregion
                case "+": // 加法操作
                    #region [====加法操作====]
                    if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                    MemeryUnits.Unit value1 = this.Params[0].GetMemeryUnit();
                    MemeryUnits.Unit value2 = this.Params[1].GetMemeryUnit();
                    // 兼容函数执行
                    if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                    if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                    if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("加法函数参数必须为数值，请使用num()函数转换");
                    if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("加法函数参数必须为数值，请使用num()函数转换");
                    return new MemeryUnits.Number(((MemeryUnits.Number)value1).Value + ((MemeryUnits.Number)value2).Value);
                #endregion
                case "-": // 减法操作
                    #region [====减法操作====]
                    if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                    value1 = this.Params[0].GetMemeryUnit();
                    value2 = this.Params[1].GetMemeryUnit();
                    // 兼容函数执行
                    if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                    if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                    if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("减法函数参数必须为数值，请使用num()函数转换");
                    if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("减法函数参数必须为数值，请使用num()函数转换");
                    return new MemeryUnits.Number(((MemeryUnits.Number)value1).Value - ((MemeryUnits.Number)value2).Value);
                #endregion
                case "*": // 乘法操作
                    #region [====乘法操作====]
                    if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                    value1 = this.Params[0].GetMemeryUnit();
                    value2 = this.Params[1].GetMemeryUnit();
                    // 兼容函数执行
                    if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                    if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                    if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                    if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                    return new MemeryUnits.Number(((MemeryUnits.Number)value1).Value * ((MemeryUnits.Number)value2).Value);
                #endregion
                case "/": // 除法操作
                    #region [====除法操作====]
                    if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                    value1 = this.Params[0].GetMemeryUnit();
                    value2 = this.Params[1].GetMemeryUnit();
                    // 兼容函数执行
                    if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                    if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                    if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                    if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                    return new MemeryUnits.Number(((MemeryUnits.Number)value1).Value / ((MemeryUnits.Number)value2).Value);
                #endregion
                case "#": // 注释
                    return new MemeryUnits.None();
                case "return": // 执行返回数据
                    this.Parent.SetReturn();
                    if (this.Params.Count > 0) {
                        var returnVar= this.Params[0].GetMemeryUnit();
                        if (returnVar.UnitType == MemeryUnits.UnitTypes.Function) returnVar = ((MemeryUnits.Function)returnVar).Execute();
                        return returnVar;
                    } else {
                        return new MemeryUnits.None();
                    }
                case "break": // 执行中断
                    this.Parent.SetBreak();
                    return new MemeryUnits.None();
                default:
                    // 查询自定义函数
                    MemeryUnits.Unit fun = new MemeryUnits.None();
                    if (this.CheckVar(this.Name)) fun = this.GetVarValue(this.Name);
                    if (fun.UnitType == UnitTypes.Function) {
                        MemeryUnits.Function func = (MemeryUnits.Function)fun;
                        if (this.Params.Count >= func.Params.Count) throw new Exception($"函数'{this.Name}'只允许{func.Params.Count - 1}个参数");
                        egg.KeyValues<MemeryUnits.Unit> pairs = new KeyValues<Unit>();
                        for (int i = 0; i < func.Params.Count - 1; i++) {
                            var valSource = this.Params[i].GetMemeryUnit();
                            if (valSource.UnitType == UnitTypes.Function) valSource = ((MemeryUnits.Function)valSource).Execute();
                            pairs[((ProcessUnits.Define)func.Params[i]).Name] = valSource;
                        }
                        return ((MemeryUnits.Function)func.Params[func.Params.Count - 1].GetMemeryUnit()).Execute(pairs);
                    } else {
                        var func = this.Engine.GetFunction(this.Name);
                        if (eggs.IsNull(func)) throw new Exception($"函数'{this.Name}'未定义");
                        List<Unit> ls = new List<Unit>();
                        for (int i = 0; i < this.Params.Count; i++) {
                            var valSource = this.Params[i].GetMemeryUnit();
                            if (valSource.UnitType == UnitTypes.Function) valSource = ((MemeryUnits.Function)valSource).Execute();
                            ls.Add(valSource);
                        }
                        return func(ls);
                    }
                    //return new MemeryUnits.None();
            }
        }
    }
}
