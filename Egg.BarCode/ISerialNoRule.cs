using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.BarCode
{
    /// <summary>
    /// 序号生成规则
    /// </summary>
    public interface ISerialNoRule<T> : IRuleBase 
        where T : struct 
    {
        /// <summary>
        /// 最小值
        /// </summary>
        T Min { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        T? Max { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        T Step { get; set; }
        /// <summary>
        /// 最大序号
        /// </summary>
        long? MaxIndex { get; }
        /// <summary>
        /// 空间长度
        /// </summary>
        int Space { get; set; }
    }
}
