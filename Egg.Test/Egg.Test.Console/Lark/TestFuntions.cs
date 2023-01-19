using Egg.Lark;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egg.Test.Console.Lark
{
    public class TestFuntions : ScriptFunctions
    {
        public TestFuntions() {
            base.Reg("计算", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'!'缺少必要参数");
                return GetValue(this.Engine, args[0]);
            });
        }
    }
}
