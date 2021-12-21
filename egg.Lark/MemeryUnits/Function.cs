﻿using System;
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
        internal egg.KeyValues<long> Variables { get; private set; }

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
        public ScriptEngine Engine { get; private set; }

        /// <summary>
        /// 单元类型
        /// </summary>
        public Function ParentFunction { get; internal set; }

        /// <summary>
        /// 实例化对象
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="pool"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="parent"></param>
        public Function(ScriptEngine engine, ScriptMemeryPool pool, Function parent, string name, Params args = null) : base(pool, UnitTypes.Function) {
            this.Name = name;
            if (eggs.IsNull(args)) {
                this.Params = new Params();
            } else {
                this.Params = args;
            }
            this.Params.Function = this;
            this.ParentFunction = parent;
            this.Engine = engine;
            this.Variables = new KeyValues<long>();
            isBreak = false;
            isReturn = false;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void OnDispose() {
            base.OnDispose();
            this.Params.Clear();
            this.Variables.Clear();
        }

        // 初始化变量
        private void InitVars() {
            this.Variables.Clear();
        }

        // 设置中断
        internal void SetBreak() {
            isBreak = true;
            if (this.ParentFunction == null) return;
            if (this.Name != "for" && this.Name != "while") this.ParentFunction.SetBreak();
        }

        // 设置中断
        internal void SetReturn() {
            isReturn = true;
            if (this.ParentFunction == null) return;
            if (this.Name != "func" && this.Name != "function") this.ParentFunction.SetReturn();
        }

        // 设置变量值
        internal bool SetVariableValue(string name, MemeryUnits.Unit value, bool isForcibly = false) {
            // 处理强制赋值
            if (isForcibly) {
                this.Variables[name] = value.Handle;
                return true;
            }
            // 处理回溯赋值
            if (this.Variables.ContainsKey(name)) {
                this.Variables[name] = value.Handle;
                return true;
            } else {
                if (this.ParentFunction == null) return false;
                return this.ParentFunction.SetVariableValue(name, value);
            }
        }

        // 获取变量值
        internal MemeryUnits.Unit GetVariableValue(string name) {
            if (this.Variables.ContainsKey(name)) {
                return this.MemeryPool.GetUnitByHandle(this.Variables[name]);
            } else {
                if (this.ParentFunction == null) throw new Exception($"变量'{name}'尚未赋值");
                return this.ParentFunction.GetVariableValue(name);
            }
        }

        // 获取变量值
        internal egg.Strings GetVariables() {
            egg.Strings list = new egg.Strings();
            foreach (var key in this.Variables.Keys) {
                list.Add(key);
            }
            return list;
        }

        // 检查变量
        internal bool CheckVariable(string name) {
            if (this.Variables.ContainsKey(name)) {
                return true;
            } else {
                if (this.ParentFunction == null) return false;
                return this.ParentFunction.CheckVariable(name);
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
                    this.Variables[item.Key] = item.Value.Handle;
                }
            }
            try {
                switch (this.Name) {
                    case "step": // 所有参数按顺序执行
                        #region [====顺序执行====]
                        for (int i = 0; i < this.Params.Count; i++) {
                            var fn = this.Params[i].GetMemeryUnit();
                            if (fn.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("step的参数必须为函数");
                            var res = ((MemeryUnits.Function)fn).Execute();
                            if (isBreak) return this.MemeryPool.None;
                            if (isReturn) return res;
                        }
                        #endregion
                        return this.MemeryPool.None;
                    case "let": // 赋值操作
                        #region [====赋值操作====]
                        if (this.Params.Count != 2) throw new Exception("let函数只允许两个参数");
                        if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("let函数第一个参数只接受变量");
                        string name = ((ProcessUnits.Define)this.Params[0]).Name;
                        MemeryUnits.Unit value = this.Params[1].GetMemeryUnit();
                        if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                        if (eggs.IsNull(this.ParentFunction)) throw new Exception("let函数未找到父函数");
                        int idx = name.IndexOf('.');
                        if (idx >= 0) {
                            string objNameParent = name.Substring(0, idx);
                            string objNameChild = name.Substring(idx + 1);
                            MemeryUnits.Unit obj = this.GetVariableValue(objNameParent);
                            if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{objNameParent}'路径并非对象");
                            ((MemeryUnits.Object)obj)[objNameChild] = value;
                        } else {
                            if (!this.ParentFunction.SetVariableValue(name, value)) {
                                this.ParentFunction.SetVariableValue(name, value, true);
                            }
                        }
                        #endregion
                        return this.MemeryPool.None;
                    case "set": // 赋值数组操作
                        #region [====赋值数组操作====]
                        if (this.Params.Count != 3) throw new Exception("set函数只允许三个参数");
                        if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("set函数第一个参数只接受变量");
                        name = ((ProcessUnits.Define)this.Params[0]).Name;
                        var index = this.Params[1].GetMemeryUnit();
                        if (index.UnitType == MemeryUnits.UnitTypes.Function) index = ((MemeryUnits.Function)index).Execute();
                        if (index.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception($"set函数第二个参数只接受数值");
                        value = this.Params[2].GetMemeryUnit();
                        if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                        if (eggs.IsNull(this.ParentFunction)) throw new Exception("set函数未找到父函数");
                        idx = name.IndexOf('.');
                        if (idx >= 0) {
                            string objNameParent = name.Substring(0, idx);
                            string objNameChild = name.Substring(idx + 1);
                            MemeryUnits.Unit obj = this.GetVariableValue(objNameParent);
                            if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{objNameParent}'路径并非对象");
                            MemeryUnits.Unit ls = ((MemeryUnits.Object)obj)[objNameChild];
                            if (ls.UnitType != MemeryUnits.UnitTypes.List) throw new Exception($"变量'{name}'路径并非数组");
                            ((MemeryUnits.List)ls)[(int)((MemeryUnits.Number)index).Value] = value;
                        } else {
                            MemeryUnits.Unit ls = this.GetVariableValue(name);
                            if (ls.UnitType != MemeryUnits.UnitTypes.List) throw new Exception($"变量'{name}'路径并非数组");
                            ((MemeryUnits.List)ls)[(int)((MemeryUnits.Number)index).Value] = value;
                        }
                        #endregion
                        return this.MemeryPool.None;
                    case "get": // 获取数组值
                        #region [====获取数组值====]
                        if (this.Params.Count != 2) throw new Exception("set函数只允许2个参数");
                        if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("set函数第一个参数只接受变量");
                        name = ((ProcessUnits.Define)this.Params[0]).Name;
                        index = this.Params[1].GetMemeryUnit();
                        if (index.UnitType == MemeryUnits.UnitTypes.Function) index = ((MemeryUnits.Function)index).Execute();
                        if (eggs.IsNull(this.ParentFunction)) throw new Exception("set函数未找到父函数");
                        idx = name.IndexOf('.');
                        if (idx >= 0) {
                            string objNameParent = name.Substring(0, idx);
                            string objNameChild = name.Substring(idx + 1);
                            MemeryUnits.Unit obj = this.GetVariableValue(objNameParent);
                            if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{objNameParent}'路径并非对象");
                            MemeryUnits.Unit ls = ((MemeryUnits.Object)obj)[objNameChild];
                            if (ls.UnitType != MemeryUnits.UnitTypes.List) throw new Exception($"变量'{name}'路径并非数组");
                            return ((MemeryUnits.List)ls)[(int)((MemeryUnits.Number)index).Value];
                        } else {
                            MemeryUnits.Unit ls = this.GetVariableValue(name);
                            if (ls.UnitType != MemeryUnits.UnitTypes.List) throw new Exception($"变量'{name}'路径并非数组");
                            return ((MemeryUnits.List)ls)[(int)((MemeryUnits.Number)index).Value];
                        }
                    #endregion
                    case "import": // 导入操作
                        #region [====导入操作====]
                        if (this.Params.Count != 1) throw new Exception("import函数只允许1个参数");
                        MemeryUnits.Unit path = this.Params[0].GetMemeryUnit();
                        if (path.UnitType == MemeryUnits.UnitTypes.Function) path = ((MemeryUnits.Function)path).Execute();
                        if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception("import函数第一个参数只接受字符串");
                        this.Engine.Include(((MemeryUnits.String)path).Value);
                        #endregion
                        return this.MemeryPool.None;
                    case "path": // 导入操作
                        #region [====导入操作====]
                        if (this.Params.Count != 1) throw new Exception("path函数只允许1个参数");
                        path = this.Params[0].GetMemeryUnit();
                        if (path.UnitType == MemeryUnits.UnitTypes.Function) path = ((MemeryUnits.Function)path).Execute();
                        if (path.UnitType != MemeryUnits.UnitTypes.String) throw new Exception("path函数第一个参数只接受字符串");
                        this.Engine.AddPath(((MemeryUnits.String)path).Value);
                        #endregion
                        return this.MemeryPool.None;
                    case "func": // 函数定义操作
                        #region [====函数定义操作====]
                        if (this.Params.Count < 1) throw new Exception("func函数必须包含一个参数");
                        Params args = new Params();
                        for (int i = 0; i < this.Params.Count - 1; i++) {
                            if (this.Params[i].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("func函数除最后一个参数，其他参数只接受变量");
                            args.Add(this.Params[i]);
                        }
                        if (this.Params[this.Params.Count - 1].GetMemeryUnit().UnitType != UnitTypes.Function) throw new Exception("func函数最后一个参数只接受函数");
                        args.Add(this.Params[this.Params.Count - 1]);
                        #endregion
                        return new MemeryUnits.Function(this.Engine, this.MemeryPool, this, "", args);
                    case "function": // 全局函数定义操作
                        #region [====全局函数定义操作====]
                        if (this.Params.Count < 2) throw new Exception("function函数必须包含两个参数");
                        var funName = this.Params[0].GetMemeryUnit();
                        if (funName.UnitType == MemeryUnits.UnitTypes.Function) funName = ((MemeryUnits.Function)funName).Execute();
                        if (funName.UnitType != MemeryUnits.UnitTypes.String) throw new Exception("function函数第一个参数只接受字符串");
                        args = new Params();
                        for (int i = 1; i < this.Params.Count - 1; i++) {
                            if (this.Params[i].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("function函数除最后一个参数，其他参数只接受变量");
                            args.Add(this.Params[i]);
                        }
                        if (this.Params[this.Params.Count - 1].GetMemeryUnit().UnitType != UnitTypes.Function) throw new Exception("function函数最后一个参数只接受函数");
                        args.Add(this.Params[this.Params.Count - 1]);
                        this.Engine.SetProcessVariable(((MemeryUnits.String)funName).Value, new MemeryUnits.Function(this.Engine, this.MemeryPool, null, "", args));
                        #endregion
                        return this.MemeryPool.None;
                    case "object": // 定义一个对象操作
                        #region [====全局函数定义操作====]
                        if (this.Params.Count != 1) throw new Exception("function函数必须包含两个参数");
                        if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("object函数参数只接受变量");
                        string objName = ((ProcessUnits.Define)this.Params[0]).Name;
                        idx = objName.IndexOf('.');
                        if (idx >= 0) {
                            string objNameParent = objName.Substring(0, idx);
                            string objNameChild = objName.Substring(idx + 1);
                            MemeryUnits.Unit obj = this.GetVariableValue(objNameParent);
                            if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{objNameParent}'并非对象");
                            ((MemeryUnits.Object)obj)[objNameChild] = this.MemeryPool.CreateObject(this.Handle).MemeryUnit;
                        } else {
                            var obj = this.MemeryPool.CreateObject(this.Handle).MemeryUnit;
                            if (!this.ParentFunction.SetVariableValue(objName, obj)) {
                                this.ParentFunction.SetVariableValue(objName, obj, true);
                            }
                        }
                        #endregion
                        return this.MemeryPool.None;
                    case "list": // 定义一个对象操作
                        #region [====全局函数定义操作====]
                        if (this.Params.Count != 1) throw new Exception("function函数必须包含两个参数");
                        if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception("object函数参数只接受变量");
                        string lsName = ((ProcessUnits.Define)this.Params[0]).Name;
                        idx = lsName.IndexOf('.');
                        if (idx >= 0) {
                            string objNameParent = lsName.Substring(0, idx);
                            string objNameChild = lsName.Substring(idx + 1);
                            MemeryUnits.Unit obj = this.GetVariableValue(objNameParent);
                            if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{objNameParent}'并非对象");
                            ((MemeryUnits.Object)obj)[objNameChild] = this.MemeryPool.CreateList(this.Handle).MemeryUnit;
                        } else {
                            var ls = this.MemeryPool.CreateList(this.Handle).MemeryUnit;
                            if (!this.ParentFunction.SetVariableValue(lsName, ls)) {
                                this.ParentFunction.SetVariableValue(lsName, ls, true);
                            }
                        }
                        #endregion
                        return this.MemeryPool.None;
                    case "if": // 分支定义操作
                        #region [====分支定义操作====]
                        if (this.Params.Count < 2) throw new Exception("if函数最少包含两个参数");
                        if (this.Params.Count > 3) throw new Exception("if函数最多包含三个参数");
                        value = this.Params[0].GetMemeryUnit();
                        if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                        if (value.UnitType == UnitTypes.Number) value = this.MemeryPool.CreateBoolean(value.ToNumber() > 0, this.Handle).MemeryUnit;
                        if (value.UnitType != UnitTypes.Boolean) throw new Exception("if函数条件参数需要定义为布尔值");
                        if (value.ToBoolean()) {
                            var funcIf = this.Params[1].GetMemeryUnit();
                            if (funcIf.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("if函数执行参数需要定义为函数");
                            var res = ((MemeryUnits.Function)funcIf).Execute();
                            if (isReturn) return res;
                        } else {
                            if (this.Params.Count < 3) return this.MemeryPool.None;
                            var funcIf = this.Params[2].GetMemeryUnit();
                            if (funcIf.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("if函数执行参数需要定义为函数");
                            var res = ((MemeryUnits.Function)funcIf).Execute();
                            if (isReturn) return res;
                        }
                        return this.MemeryPool.None;
                    #endregion
                    case "while": // 循环定义操作
                        #region [====循环定义操作====]
                        if (this.Params.Count != 2) throw new Exception("while函数必须要两个参数");
                        value = this.Params[0].GetMemeryUnit();
                        if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                        if (value.UnitType == UnitTypes.Number) value = this.MemeryPool.CreateBoolean(value.ToNumber() > 0, this.Handle).MemeryUnit;
                        if (value.UnitType != UnitTypes.Boolean) throw new Exception("while函数条件参数需要定义为布尔值");
                        while (((MemeryUnits.Boolean)value).Value) {
                            var funcWhile = this.Params[1].GetMemeryUnit();
                            if (funcWhile.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("while函数执行参数需要定义为函数");
                            var res = ((MemeryUnits.Function)funcWhile).Execute();
                            if (isReturn) return res;
                            if (isBreak) break;
                            // 读取下一次条件
                            value = this.Params[0].GetMemeryUnit();
                            if (value.UnitType == MemeryUnits.UnitTypes.Function) value = ((MemeryUnits.Function)value).Execute();
                            if (value.UnitType != UnitTypes.Number) throw new Exception("while函数条件参数需要定义为数值");
                        }
                        return this.MemeryPool.None;
                    #endregion
                    case "for": // for循环定义操作
                        #region [====循环定义操作====]
                        if (this.Params.Count < 5) throw new Exception("for函数必须要五个参数");
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
                            var funcFor = this.Params[4].GetMemeryUnit();
                            if (funcFor.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("if函数执行参数需要定义为函数");
                            lsArgs[forVar] = this.MemeryPool.CreateNumber(i, this.Handle).MemeryUnit;
                            var res = ((MemeryUnits.Function)funcFor).Execute(lsArgs);
                            if (isReturn) return res;
                            if (isBreak) break;
                        }
                        return this.MemeryPool.None;
                    #endregion
                    case "+": // 加法操作
                    case "add": // 加法操作
                        #region [====加法操作====]
                        if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                        MemeryUnits.Unit value1 = this.Params[0].GetMemeryUnit();
                        MemeryUnits.Unit value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("加法函数参数必须为数值，请使用num()函数转换");
                        if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("加法函数参数必须为数值，请使用num()函数转换");
                        return this.MemeryPool.CreateNumber(value1.ToNumber() + value2.ToNumber(), this.Handle).MemeryUnit;
                    #endregion
                    case "-": // 减法操作
                    case "minus": // 减法操作
                        #region [====减法操作====]
                        if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("减法函数参数必须为数值，请使用num()函数转换");
                        if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("减法函数参数必须为数值，请使用num()函数转换");
                        return this.MemeryPool.CreateNumber(value1.ToNumber() - value2.ToNumber(), this.Handle).MemeryUnit;
                    #endregion
                    case "*": // 乘法操作
                    case "mul": // 乘法操作
                        #region [====乘法操作====]
                        if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                        if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                        return this.MemeryPool.CreateNumber(value1.ToNumber() * value2.ToNumber(), this.Handle).MemeryUnit;
                    #endregion
                    case "/": // 除法操作
                    case "div": // 除法操作
                        #region [====除法操作====]
                        if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                        if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("乘法函数参数必须为数值，请使用num()函数转换");
                        return this.MemeryPool.CreateNumber(value1.ToNumber() / value2.ToNumber(), this.Handle).MemeryUnit;
                    #endregion
                    case "rem": // 取余操作
                        #region [====取余操作====]
                        if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("取余函数参数必须为数值，请使用num()函数转换");
                        if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("取余函数参数必须为数值，请使用num()函数转换");
                        return this.MemeryPool.CreateNumber((long)(value1.ToNumber()) % (long)(value2.ToNumber()), this.Handle).MemeryUnit;
                    #endregion
                    case "divi": // 整除操作
                        #region [====整除操作====]
                        if (this.Params.Count != 2) throw new Exception("加法函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("整除函数参数必须为数值，请使用num()函数转换");
                        if (value2.UnitType != MemeryUnits.UnitTypes.Number) throw new Exception("整除函数参数必须为数值，请使用num()函数转换");
                        return this.MemeryPool.CreateNumber((long)value1.ToNumber() / (long)value2.ToNumber(), this.Handle).MemeryUnit;
                    #endregion
                    case "equal": // 判断相等操作
                        #region [====判断相等操作====]
                        if (this.Params.Count != 2) throw new Exception($"{this.Name}函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != value2.UnitType) throw new Exception("等于函数的两个参数必须类型相同");
                        switch (value1.UnitType) {
                            case UnitTypes.None: return this.MemeryPool.CreateBoolean(true, this.Handle).MemeryUnit;
                            case UnitTypes.Boolean: return this.MemeryPool.CreateBoolean(((MemeryUnits.Boolean)value1).Value == ((MemeryUnits.Boolean)value2).Value, this.Handle).MemeryUnit;
                            case UnitTypes.Number: return this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value == ((MemeryUnits.Number)value2).Value, this.Handle).MemeryUnit;
                            case UnitTypes.String: return this.MemeryPool.CreateBoolean(((MemeryUnits.String)value1).Value == ((MemeryUnits.String)value2).Value, this.Handle).MemeryUnit;
                            default: throw new Exception($"{this.Name}函数不知道'{value1.UnitType.ToString()}'类型参数");
                        }
                    #endregion
                    case "small": // 判断相等操作
                        #region [====判断相等操作====]
                        if (this.Params.Count != 2) throw new Exception($"{this.Name}函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != value2.UnitType) throw new Exception("等于函数的两个参数必须类型相同");
                        switch (value1.UnitType) {
                            case UnitTypes.None: return this.MemeryPool.CreateBoolean(false, this.Handle).MemeryUnit;
                            case UnitTypes.Number: return this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value < ((MemeryUnits.Number)value2).Value, this.Handle).MemeryUnit;
                            default: throw new Exception($"{this.Name}函数不知道'{value1.UnitType.ToString()}'类型参数");
                        }
                    #endregion
                    case "large": // 判断相等操作
                        #region [====判断相等操作====]
                        if (this.Params.Count != 2) throw new Exception($"{this.Name}函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != value2.UnitType) throw new Exception("等于函数的两个参数必须类型相同");
                        switch (value1.UnitType) {
                            case UnitTypes.None: return this.MemeryPool.CreateBoolean(false, this.Handle).MemeryUnit;
                            case UnitTypes.Number: return this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value > ((MemeryUnits.Number)value2).Value, this.Handle).MemeryUnit;
                            default: throw new Exception($"{this.Name}函数不知道'{value1.UnitType.ToString()}'类型参数");
                        }
                    #endregion
                    case "small_equal": // 判断相等操作
                        #region [====判断相等操作====]
                        if (this.Params.Count != 2) throw new Exception($"{this.Name}函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != value2.UnitType) throw new Exception("等于函数的两个参数必须类型相同");
                        switch (value1.UnitType) {
                            case UnitTypes.None: return this.MemeryPool.CreateBoolean(false, this.Handle).MemeryUnit;
                            case UnitTypes.Number: return this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value <= ((MemeryUnits.Number)value2).Value, this.Handle).MemeryUnit;
                            default: throw new Exception($"{this.Name}函数不知道'{value1.UnitType.ToString()}'类型参数");
                        }
                    #endregion
                    case "large_equal": // 判断相等操作
                        #region [====判断相等操作====]
                        if (this.Params.Count != 2) throw new Exception($"{this.Name}函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType != value2.UnitType) throw new Exception("等于函数的两个参数必须类型相同");
                        switch (value1.UnitType) {
                            case UnitTypes.None: return this.MemeryPool.CreateBoolean(false, this.Handle).MemeryUnit;
                            case UnitTypes.Number: return this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value >= ((MemeryUnits.Number)value2).Value, this.Handle).MemeryUnit;
                            default: throw new Exception($"{this.Name}函数不知道'{value1.UnitType.ToString()}'类型参数");
                        }
                    #endregion
                    case "and": // 与操作
                        #region [====与操作====]
                        if (this.Params.Count != 2) throw new Exception("and函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType == MemeryUnits.UnitTypes.Number) value1 = this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value > 0, this.Handle).MemeryUnit;
                        if (value1.UnitType != MemeryUnits.UnitTypes.Boolean) throw new Exception("加法函数参数必须为布尔值，请使用num()函数转换");
                        if (value2.UnitType == MemeryUnits.UnitTypes.Number) value2 = this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value2).Value > 0, this.Handle).MemeryUnit;
                        if (value2.UnitType != MemeryUnits.UnitTypes.Boolean) throw new Exception("加法函数参数必须为布尔值，请使用num()函数转换");
                        return this.MemeryPool.CreateBoolean(((MemeryUnits.Boolean)value1).Value && ((MemeryUnits.Boolean)value2).Value, this.Handle).MemeryUnit;
                    #endregion
                    case "or": // 或操作
                        #region [====或操作====]
                        if (this.Params.Count != 2) throw new Exception("and函数只允许两个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        value2 = this.Params[1].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value2.UnitType == MemeryUnits.UnitTypes.Function) value2 = ((MemeryUnits.Function)value2).Execute();
                        if (value1.UnitType == MemeryUnits.UnitTypes.Number) value1 = this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value > 0, this.Handle).MemeryUnit;
                        if (value1.UnitType != MemeryUnits.UnitTypes.Boolean) throw new Exception("加法函数参数必须为布尔值，请使用num()函数转换");
                        if (value2.UnitType == MemeryUnits.UnitTypes.Number) value2 = this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value2).Value > 0, this.Handle).MemeryUnit;
                        if (value2.UnitType != MemeryUnits.UnitTypes.Boolean) throw new Exception("加法函数参数必须为布尔值，请使用num()函数转换");
                        return this.MemeryPool.CreateBoolean(((MemeryUnits.Boolean)value1).Value || ((MemeryUnits.Boolean)value2).Value, this.Handle).MemeryUnit;
                    #endregion
                    case "not": // 与或操作
                        #region [====与或操作====]
                        if (this.Params.Count != 1) throw new Exception("and函数只允许1个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        // 兼容函数执行
                        if (value1.UnitType == MemeryUnits.UnitTypes.Function) value1 = ((MemeryUnits.Function)value1).Execute();
                        if (value1.UnitType == MemeryUnits.UnitTypes.Number) value1 = this.MemeryPool.CreateBoolean(((MemeryUnits.Number)value1).Value > 0, this.Handle).MemeryUnit;
                        if (value1.UnitType != MemeryUnits.UnitTypes.Boolean) throw new Exception("加法函数参数必须为布尔值，请使用num()函数转换");
                        return this.MemeryPool.CreateBoolean(!((MemeryUnits.Boolean)value1).Value, this.Handle).MemeryUnit;
                    #endregion
                    case "join": // 字符串连接
                        #region [====字符串连接操作====]
                        if (this.Params.Count < 2) throw new Exception("字符串连接函数至少需要两个参数");
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < this.Params.Count; i++) {
                            MemeryUnits.Unit val = this.Params[i].GetMemeryUnit();
                            // 兼容函数执行
                            if (val.UnitType == MemeryUnits.UnitTypes.Function) val = ((MemeryUnits.Function)val).Execute();
                            switch (val.UnitType) {
                                case UnitTypes.Boolean:
                                    sb.Append(((MemeryUnits.Boolean)val).Value.ToString());
                                    break;
                                case UnitTypes.String:
                                    sb.Append(((MemeryUnits.String)val).Value);
                                    break;
                                case UnitTypes.Number:
                                    sb.Append(((MemeryUnits.Number)val).Value);
                                    break;
                                case UnitTypes.None: break;
                                default: throw new Exception($"join函数不知道'{val.UnitType.ToString()}'类型参数");
                            }
                        }
                        return this.MemeryPool.CreateString(sb.ToString(), this.Handle).MemeryUnit;
                    #endregion
                    case "number": // 转化为数值
                        #region [====转化为数值====]
                        if (this.Params.Count != 1) throw new Exception("转化为数值函数仅支持一个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        if (value1.UnitType == MemeryUnits.UnitTypes.String) return this.MemeryPool.CreateNumber(((MemeryUnits.String)value1).Value.ToDouble(), this.Handle).MemeryUnit;
                        if (value1.UnitType == MemeryUnits.UnitTypes.Boolean) return this.MemeryPool.CreateNumber(((MemeryUnits.Boolean)value1).Value ? 1 : 0, this.Handle).MemeryUnit;
                        throw new Exception($"num函数的参数不支持类型{value1.UnitType.ToString()}");
                    #endregion
                    case "string": // 转化为字符串
                        #region [====转化为字符串====]
                        if (this.Params.Count != 1) throw new Exception("转化为数值函数仅支持一个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        if (value1.UnitType == MemeryUnits.UnitTypes.Number) return this.MemeryPool.CreateString(((MemeryUnits.Number)value1).Value.ToString(), this.Handle).MemeryUnit;
                        if (value1.UnitType == MemeryUnits.UnitTypes.Boolean) return this.MemeryPool.CreateString(((MemeryUnits.Boolean)value1).Value ? "true" : "false", this.Handle).MemeryUnit;
                        throw new Exception($"str函数的参数不支持类型{value1.UnitType.ToString()}");
                    #endregion
                    case "#": // 注释
                        return this.MemeryPool.None;
                    case "return": // 执行返回数据
                        this.ParentFunction.SetReturn();
                        if (this.Params.Count > 0) {
                            var returnVar = this.Params[0].GetMemeryUnit();
                            if (returnVar.UnitType == MemeryUnits.UnitTypes.Function) returnVar = ((MemeryUnits.Function)returnVar).Execute();
                            return returnVar;
                        } else {
                            return this.MemeryPool.None;
                        }
                    case "break": // 执行中断
                        this.ParentFunction.SetBreak();
                        return this.MemeryPool.None;
                    case "try": // 安全模式运行
                        #region [====安全模式运行====]
                        if (this.Params.Count < 1) throw new Exception($"{this.Name}函数至少包含一个参数");
                        value1 = this.Params[0].GetMemeryUnit();
                        if (value1.UnitType != UnitTypes.Function) throw new Exception($"{this.Name}函数的第一个参数不支持'{value1.UnitType.ToString()}'类型");
                        try {
                            return ((MemeryUnits.Function)value1).Execute();
                        } catch (Exception ex) {
                            var err = this.MemeryPool.CreateObject(this.Handle).ToObject();
                            err.Str("detail", ex.ToString());
                            this.SetVariableValue("ex", err, true);
                            if (this.Params.Count > 1) {
                                value2 = this.Params[1].GetMemeryUnit();
                                if (value2.UnitType != UnitTypes.Function) throw new Exception($"{this.Name}函数的第二个参数不支持'{value2.UnitType.ToString()}'类型");
                                return ((MemeryUnits.Function)value2).Execute();
                            }
                        }
                        // this.ParentFunction.SetBreak();
                        return this.MemeryPool.None;
                    #endregion
                    case "throw": // 触发错误
                        #region [====触发错误====]
                        if (this.Params.Count < 1) throw new Exception($"{this.Name}函数至少包含一个参数");
                        ScriptMemeryItem arg1 = this.Params[0].GetMemeryUnit().ToMemeryItem();
                        if (arg1.IsFunction()) arg1 = arg1.ToFunction().Execute().ToMemeryItem();
                        if (!arg1.IsString()) throw new Exception($"{this.Name}函数的第一个参数不支持'{arg1.MemeryUnitTypeName}'类型");
                        throw new Exception(arg1.ToString());
                    #endregion
                    case "foreach": // foreach循环定义操作
                        #region [====循环定义操作====]
                        if (this.Params.Count < 3) throw new Exception($"{this.Name}函数必须要3个参数");
                        if (this.Params[0].UnitType != ProcessUnits.UnitTypes.Define) throw new Exception($"{this.Name}函数第一个参数只接受变量");
                        forVar = ((ProcessUnits.Define)this.Params[0]).Name;
                        lsArgs = new KeyValues<Unit>();
                        // 获取起始值
                        var forList = this.Params[1].GetMemeryUnit();
                        if (forList.UnitType == MemeryUnits.UnitTypes.Function) forList = ((MemeryUnits.Function)forList).Execute();
                        if (forList.UnitType != UnitTypes.List) throw new Exception($"{this.Name}函数第二参数需要定义为列表");
                        var forListUnit = (MemeryUnits.List)forList;
                        for (int i = 0; i < forListUnit.Count; i++) {
                            var funcFor = this.Params[2].GetMemeryUnit();
                            if (funcFor.UnitType != MemeryUnits.UnitTypes.Function) throw new Exception("if函数执行参数需要定义为函数");
                            lsArgs[forVar] = forListUnit[i];
                            var res = ((MemeryUnits.Function)funcFor).Execute(lsArgs);
                            if (isReturn) return res;
                            if (isBreak) break;
                        }
                        return this.MemeryPool.None;
                    #endregion
                    default:
                        // 查询自定义函数
                        idx = this.Name.IndexOf('.');
                        if (idx >= 0) {
                            string objNameParent = this.Name.Substring(0, idx);
                            string objNameChild = this.Name.Substring(idx + 1);
                            MemeryUnits.Unit obj = this.GetVariableValue(objNameParent);
                            if (obj.UnitType != MemeryUnits.UnitTypes.Object) throw new Exception($"变量'{objNameParent}'并非对象");
                            MemeryUnits.Unit fun = ((MemeryUnits.Object)obj)[objNameChild];
                            if (!Lark.ScriptEngine.IsFunction(fun)) throw new Exception($"函数'{this.Name}'未定义");
                            return Lark.ScriptEngine.ExecuteFunction(this, fun);
                        } else {
                            MemeryUnits.Unit fun = this.MemeryPool.None;
                            if (this.CheckVariable(this.Name)) fun = this.GetVariableValue(this.Name);
                            if (fun.UnitType == UnitTypes.None) fun = this.Engine.GetProcessVariable(this.Name);
                            if (!Lark.ScriptEngine.IsFunction(fun)) throw new Exception($"函数'{this.Name}'未定义");
                            return Lark.ScriptEngine.ExecuteFunction(this, fun);
                        }
                }
            } catch (Exception ex) {
                throw new Exception($"{this.Name}->{ex.Message}", ex);
            }
        }
    }
}
