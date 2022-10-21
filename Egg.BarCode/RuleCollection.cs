using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 生成规则集合
    /// </summary>
    public class RuleCollection : IRuleCollection
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual Dictionary<string, IRuleBase> KeyRules { get; set; }

        /// <summary>
        /// 对象实例化
        /// </summary>
        public RuleCollection()
        {
            this.KeyRules = new Dictionary<string, IRuleBase>();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetValue(long index)
        {
            if (KeyRules is null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.KeyRules)
            {
                sb.Append(item.Value.GetValue(index));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 添加规则
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IRuleCollection Use(string key, IRuleBase rule)
        {
            this.KeyRules[key] = rule;
            return this;
        }
    }
}
