using Egg;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace egg
{
    /// <summary>
    /// 生成器
    /// </summary>
    public static class Generator
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public static DateTime SnowflakeStartTime { get; set; } = new DateTime(2020, 1, 1);

        /// <summary>
        /// 机器ID
        /// </summary>
        public static int SnowflakeMachineId { get; set; } = 0;

        // 定义懒加载对象
        private static Lazy<Egg.Snowflake> _snowflake = new Lazy<Snowflake>(() => { return new Snowflake(SnowflakeStartTime, SnowflakeMachineId); });

        /// <summary>
        /// 雪花算法
        /// </summary>
        public static Snowflake Snowflake => _snowflake.Value;

        /// <summary>
        /// 获取一个新的雪花算法Id
        /// </summary>
        /// <returns></returns>
        public static long GetSnowflakeId() => _snowflake.Value.Next();

        /// <summary>
        /// 获取一个新的Guid
        /// </summary>
        /// <returns></returns>
        public static string GetGuid() => Guid.NewGuid().ToString().Replace("-", "");

    }
}
