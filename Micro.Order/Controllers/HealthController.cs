using Micro.Comm;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Order.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        public HealthController()
        {

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
    }
}
