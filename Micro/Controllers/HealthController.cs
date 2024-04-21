using Consul;
using Micro.Comm;
using Microsoft.AspNetCore.Mvc;

namespace Micro.User.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ConsulBalancer _balancer;
        public HealthController(ConsulBalancer balancer)
        {
            _balancer = balancer;
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string heart()
        {
            return "ok";
        }

        [HttpGet]
        public AgentService GetAliveService(string serverName)
        {
            return _balancer.ChooseServer(serverName);
        }
    }
}
