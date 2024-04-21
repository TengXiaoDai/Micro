namespace Micro.Comm
{
    public class ConsulOption
    {
        /// <summary>
        /// 当前服务的ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 当前服务的端口
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 微服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 注册的Consul 的地址
        /// </summary>
        public string ConsulHost { get; set; }
        /// <summary>
        /// 注册Consul的数据中心
        /// </summary>
        public string ConsulDataCenter { get; set; }
    }
}
