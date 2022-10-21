using System;

namespace Egg.BarCode
{
    /// <summary>
    /// 序号规则
    /// </summary>
    public class SerialNoRule : ISerialNoRule<long>
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public long Min { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public long? Max { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public long Step { get; set; } = 1;
        /// <summary>
        /// 最大索引
        /// </summary>
        public long? MaxIndex => Max.HasValue ? ((Max - Min) / Step + 1) : null;
        /// <summary>
        /// 空间占位
        /// </summary>
        public int Space { get; set; } = 1;

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual string GetValue(long index)
        {
            if (MaxIndex.HasValue)
            {
                if (index > MaxIndex) throw new ArgumentOutOfRangeException(nameof(index));
            }
            return (Min + Step * (index - 1)).ToString().PadLeft(Space, '0');
        }
    }
}
