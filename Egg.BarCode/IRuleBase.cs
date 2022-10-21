using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 基础规则
    /// </summary>
    public interface IRuleBase
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        string GetValue(long index);
    }
}
