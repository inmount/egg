using System;
using System.Collections.Generic;
using System.Text;

namespace Egg
{
    /*
     最高位是符号位，始终为0，不可用。
     41位的时间序列，精确到毫秒级，41位的长度可以使用69年。时间位还有一个很重要的作用是可以根据时间进行排序。
     10位的机器标识，10位的长度最多支持部署1024个节点。
     12位的计数序列号，序列号即一系列的自增id，可以支持同一节点同一毫秒生成多个ID序号，12位的计数序列号支持每个节点每毫秒产生4096个ID序号。
     */

    /// <summary>
    /// 雪花算法
    /// </summary>
    public class Snowflake
    {
        // 机器id所占的位数
        private const int machineIdBits = 10;
        // 支持的最大机器id，结果是31 (这个移位算法可以很快的计算出几位二进制数所能表示的最大十进制数)
        private const int maxMachineId = (int)(-1L ^ (-1L << machineIdBits));
        // 序列在id中占的位数
        private const int sequenceBits = 12;
        // 机器ID向左移12位
        private const int machineIdShift = sequenceBits;
        // 时间截向左移22位(10+12)
        private const int timestampLeftShift = sequenceBits + machineIdBits;
        // 生成序列的掩码，这里为4095 (0b111111111111=0xfff=4095)
        private const int sequenceMask = (int)(-1L ^ (-1L << sequenceBits));

        // 互斥锁对象
        private static object syncRoot = new object();

        /// <summary>
        /// 机器ID(0~1023)
        /// </summary>
        public int MachineId { get; private set; }
        /// <summary>
        ///  毫秒内序列(0~4095)
        /// </summary>
        public int Sequence { get; private set; }
        /// <summary>
        /// 上次生成ID的时间截
        /// </summary>
        public long LastTimestamp { get; private set; }
        /// <summary>
        /// 开始时间戳
        /// </summary>
        public DateTime StartTime { get; private set; } = new DateTime(2010, 1, 1);

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            Sequence = 0;
            LastTimestamp = -1;
        }

        /// <summary>
        /// 雪花算法, 默认开始时间(2020-01-01), 机器ID为0
        /// </summary>
        public Snowflake()
        {
            this.StartTime = new DateTime(2020, 1, 1);
            this.MachineId = 0;
        }

        /// <summary>
        /// 雪花算法, 默认开始时间(2020-01-01)
        /// </summary>
        /// <param name="machineId">机器ID(0~1023)</param>
        /// <exception cref="Exception"></exception>
        public Snowflake(int machineId)
        {
            if (machineId < 0) throw new Exception($"机器ID范围为0~{maxMachineId}");
            if (machineId > maxMachineId) throw new Exception($"机器ID范围为0~{maxMachineId}");
            this.StartTime = new DateTime(2020, 1, 1);
            this.MachineId = machineId;
        }

        /// <summary>
        /// 雪花算法
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="machineId">机器ID(0~1023)</param>
        /// <exception cref="Exception"></exception>
        public Snowflake(DateTime startTime, int machineId)
        {
            if (machineId < 0) throw new Exception($"机器ID范围为0~{maxMachineId}");
            if (machineId > maxMachineId) throw new Exception($"机器ID范围为0~{maxMachineId}");
            this.StartTime = startTime;
            this.MachineId = machineId;
        }

        #region 核心方法
        /// <summary>
        /// 获得下一个ID，线程安全，19位有序数字
        /// </summary>
        /// <returns></returns>
        public long Next()
        {
            lock (syncRoot)
            {
                long timestamp = GetCurrentTimestamp();
                // 如果时钟回调，则使用上一时钟
                if (timestamp < LastTimestamp) timestamp = LastTimestamp;
                if (timestamp != LastTimestamp) // 时间戳改变，毫秒内序列重置
                {
                    Sequence = 0;

                }
                else if (timestamp == LastTimestamp) // 如果是同一时间生成的，则进行毫秒内序列
                {
                    Sequence = (Sequence + 1) & sequenceMask;
                    if (Sequence == 0) // 毫秒内序列溢出
                    {
                        timestamp = GetNextTimestamp(LastTimestamp); // 阻塞到下一个毫秒,获得新的时间戳
                    }
                }

                // 记录最后的时间截
                LastTimestamp = timestamp;

                // 移位并通过或运算拼到一起组成64位的ID
                var id = (timestamp << timestampLeftShift)
                        | ((long)MachineId << machineIdShift)
                        | (long)Sequence;
                return id;
            }
        }

        /// <summary>
        /// 尝试分析
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time">时间</param>
        /// <param name="machineId">机器ID</param>
        /// <param name="sequence">序列号</param>
        /// <returns></returns>
        public bool TryParse(long id, out DateTime time, out int machineId, out int sequence)
        {
            var timestamp = id >> timestampLeftShift;
            time = StartTime.AddMilliseconds(timestamp);

            var macId = (id ^ (timestamp << timestampLeftShift)) >> machineIdShift;
            //var workId = (id ^ ((timestamp << timestampLeftShift) | (dataId << datacenterIdShift))) >> workerIdShift;
            var seq = id & sequenceMask;

            machineId = (int)macId;
            sequence = (int)seq;

            return true;
        }

        /// <summary>
        /// 阻塞到下一个毫秒，直到获得新的时间戳
        /// </summary>
        /// <param name="lastTimestamp">上次生成ID的时间截</param>
        /// <returns>当前时间戳</returns>
        private long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }
            return timestamp;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        private long GetCurrentTimestamp()
        {
            return (long)(DateTime.Now - StartTime).TotalMilliseconds;
        }
        #endregion
    }
}
