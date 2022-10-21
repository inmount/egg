using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 序号生成规则
    /// </summary>
    public interface IRules : IRuleBase
    {
        /// <summary>
        /// 序号段
        /// </summary>
        List<IRuleBase> RuleList { get; set; }

        /// <summary>
        /// 使用规则
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        IRules Use(IRuleBase rule);
    }

    /// <summary>
    /// 序号生成规则
    /// </summary>
    public interface IRules<T> : IRuleBase where T : IRuleBase
    {
        /// <summary>
        /// 序号段
        /// </summary>
        List<T> RuleList { get; set; }

        /// <summary>
        /// 使用规则
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        IRules<T> Use(T rule);
    }
}
