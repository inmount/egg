using System;
using System.Collections.Generic;

namespace Egg.BarCode
{
    /// <summary>
    /// 序号规则
    /// </summary>
    public class SerialNoRules : IRules<SerialNoRule>
    {
        /// <summary>
        /// 规则列表
        /// </summary>
        public List<SerialNoRule> RuleList { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public SerialNoRules()
        {
            this.RuleList = new List<SerialNoRule>();
        }


        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual string GetValue(long index)
        {
            for (int i = 0; i < this.RuleList.Count; i++)
            {
                var rule = this.RuleList[i];
                if (!rule.MaxIndex.HasValue || ((rule.MaxIndex ?? 0) >= index))
                {
                    return rule.GetValue(index);
                }
                else
                {
                    index -= rule.MaxIndex ?? 0;
                }
            }
            return "";
        }

        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public IRules<SerialNoRule> Use(SerialNoRule rule)
        {
            this.RuleList.Add(rule);
            return this;
        }
    }
}
