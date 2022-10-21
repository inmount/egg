using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 生成规则集合
    /// </summary>
    public interface IRuleCollection : IRuleBase
    {
        /// <summary>
        /// 序号
        /// </summary>
        Dictionary<string, IRuleBase> KeyRules { get; set; }

        /// <summary>
        /// 使用规则
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        IRuleCollection Use(string key, IRuleBase rule);
    }
}
