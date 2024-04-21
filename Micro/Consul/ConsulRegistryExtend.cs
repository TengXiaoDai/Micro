using Consul;
using log4net;
using Micro.Comm;
using Microsoft.Extensions.Options;

namespace Micro.User.Consul
{
    public static class ConsulRegistryExtend
    {
        public static async Task<WebApplication> UseConsolRegistry(this WebApplication application, IHostApplicationLifetime lifetime)
        {
            
            var logger = LogManager.GetLogger(typeof(ConsulRegistryExtend));
            ConsulOption option = application.Services.GetService<IOptionsMonitor<ConsulOption>>().CurrentValue;
            //如果启动参数设置了，默认用启动参数的，否则用配置中的
            string ip = application.Configuration["ip"] ?? option.Ip;
            string port = application.Configuration["port"] ?? option.Port;
            logger.Info($"{ip}{port}注册");
            string serverId = Guid.NewGuid().ToString();
            using ConsulClient client = new ConsulClient(a =>
            {
                a.Address = new Uri(option.ConsulHost);
                a.Datacenter = option.ConsulDataCenter;
            });
            WriteResult result = await client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                Address = ip,
                Port = int.Parse(port),
                Name = option.ServiceName,
                ID = serverId,
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromMilliseconds(12),
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20),
                    HTTP = $"http://{ip}:{port}/api/Health/heart"
                }
            });
            lifetime.ApplicationStopping.Register(async () =>
            {
                //程序结束的时候注销注册
                await client.Agent.ServiceDeregister(serverId);
                Console.WriteLine("断开注册!");
            });
            return application;
        }
    }
}
