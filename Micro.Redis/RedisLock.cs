using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Redis
{
    public class RedisLock:IDisposable
    {
        private ConnectionMultiplexer _connectionMultiplexer=null;
        private IDatabase _database;
        public RedisLock() 
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            _database = _connectionMultiplexer.GetDatabase(0);
        }

        public void Dispose()
        {
            _connectionMultiplexer?.Close();
        }

        /// <summary>
        /// 加锁
        /// 1.key:锁本身
        /// 2.value:谁假的这把锁
        /// 3.设置过期时间，防止客户端出错未释放锁
        /// </summary>
        public async void Lock() 
        {
            bool flag = await _database.LockTakeAsync("redis_lock", Thread.CurrentThread.ManagedThreadId, TimeSpan.FromSeconds(30));
        }
        /// <summary>
        /// 解锁
        /// </summary>
        public async void Unlock() 
        {
           bool flag = await _database.LockReleaseAsync("redis_lock", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
