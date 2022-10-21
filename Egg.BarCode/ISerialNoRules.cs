using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 序号生成规则
    /// </summary>
    public interface ISerialNoRules<T> : IRuleBase where T : struct
    {
        /// <summary>
        /// 序号段
        /// </summary>
        List<ISerialNoRule<T>> SerialNoRules { get; set; }
    }
}
