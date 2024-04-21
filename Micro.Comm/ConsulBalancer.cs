using Consul;
using Microsoft.Extensions.Options;

namespace Micro.Comm
{
    /// <summary>
    /// 负载均衡(为什么有网关了还要写负载均衡，这里主要是微服务和微服务之间的调用)
    /// </summary>
    public class ConsulBalancer
    {
        private readonly ConsulOption _option;
        public ConsulBalancer(IOptionsMonitor<ConsulOption> options)
        {
            _option = options.CurrentValue;
        }
        public AgentService ChooseServer(string serverName)
        {
            using ConsulClient client = new ConsulClient(a =>
            {
                a.Datacenter = _option.ConsulDataCenter;
                a.Address = new Uri(_option.ConsulHost);
            });
            var catalogServices = client.Agent.Services().Result.Response;
            foreach (var service in catalogServices)
            {
                var healthEntries = client.Health.Service(serverName, "dc1").Result;
                var healthyInstances = healthEntries.Response.Where(h => h.Checks.All(c => c.Status == HealthStatus.Passing)).ToList();
                if (healthyInstances != null && healthyInstances.Count > 0)
                    return healthyInstances.OrderBy(a => Guid.NewGuid()).FirstOrDefault().Service;
            }
            return null;
        }
    }
}
