public class SnowflakeIdGenerator
{
    private long _lastTimestamp = 0;                       // 上一次生成的时间戳
    private readonly int _dataCenterId;                     // 数据中心ID
    private readonly int _workerId;                         // 工作节点ID
    private long _sequence = 0;                             // 自增序列号

    public SnowflakeIdGenerator(int dataCenterId, int workerId)
    {
        if (dataCenterId < 0 || dataCenterId > 31)
            throw new ArgumentException("Data center ID must be between 0 and 31.");
        if (workerId < 0 || workerId > 31)
            throw new ArgumentException("Worker ID must be between 0 and 31.");

        _dataCenterId = dataCenterId;                       // 初始化数据中心ID
        _workerId = workerId;                               // 初始化工作节点ID
    }

    public long GenerateId()
    {
        long timestamp = GetTimestamp();                    // 获取当前时间戳

        if (timestamp < _lastTimestamp)                      // 如果当前时间戳小于上一次生成的时间戳
            throw new Exception("Clock moved backwards. Refusing to generate ID.");

        if (timestamp == _lastTimestamp)                     // 如果当前时间戳与上一次生成的时间戳相同
        {
            _sequence = (_sequence + 1) & 4095;             // 自增序列号，并将其限制在0～4095之间
            if (_sequence == 0)
                timestamp = WaitNextMillis(_lastTimestamp);  // 如果自增序列号达到上限，等待下一毫秒的时间戳
        }
        else
        {
            _sequence = 0;                                  // 如果当前时间戳发生变化，重置自增序列号
        }

        _lastTimestamp = timestamp;                          // 更新上一次生成的时间戳

        // 生成雪花Id，通过位运算将各个部分组合起来
        return ((timestamp - 1580000000000L) << 22)          // 时间戳部分（中间的 - 1580000000000L 是为了减少占用位数）
                | (_dataCenterId << 17)                       // 数据中心ID部分
                | (_workerId << 12)                           // 工作节点ID部分
                | _sequence;                                  // 自增序列号部分
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.Ticks / 10000 - 62135596800000L; // 获取当前时间的毫秒级时间戳
    }

    private long WaitNextMillis(long lastTimestamp)
    {
        while (GetTimestamp() <= lastTimestamp) ;            // 等待下一毫秒的时间戳

        return GetTimestamp();                              // 返回新的时间戳
    }
}