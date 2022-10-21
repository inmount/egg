using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 生成规则集合
    /// </summary>
    public class Rules : IRules
    {
        /// <summary>
        /// 规则列表
        /// </summary>
        public List<IRuleBase> RuleList { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public Rules()
        {
            this.RuleList = new List<IRuleBase>();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetValue(long index)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.RuleList.Count; i++)
            {
                sb.Append(this.RuleList[i].GetValue(index));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public IRules Use(IRuleBase rule)
        {
            this.RuleList.Add(rule);
            return this;
        }
    }
}
